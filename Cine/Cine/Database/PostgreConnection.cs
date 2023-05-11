using Cine;
using Cine.Database;
using Cine.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cine
{
    public class PostgreConnection : IDatabase
    {
        private string user = "postgres";
        private string password = "mysecretpassword";
        private string host = "localhost";
        private string port = "5432";
        private string database = "postgres";
        private string query { get; set; }
        public string connString { get; set; }

        public PostgreConnection()
        {
            connString = $"User ID = {user}; Password = {password}; Host = {host}; Port = {port}; Database = {database};";
            //DropaTabelas();
            CriaTabelas();
        }
        public ObservableCollection<Cinema> ReadCinemas()
        {
            ObservableCollection<Cinema> listReturn = new ObservableCollection<Cinema>();
            try
            {
                getCinemas(listReturn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return listReturn;
        }
        public bool AddMovie(Cinema cinema, Filme novoFilme)
        {
            try
            {
                Create(novoFilme);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public bool createRelationCinemaFilme(Cinema cinema, Filme novoFilme)
        {
            string commandString = $"INSERT INTO filmes_cinema (filme_id, cinema_id) VALUES ((SELECT cod FROM filmes WHERE nome = '{novoFilme.Nome}'), {cinema.ID})";
            try
            {
                ExecuteQuery(commandString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        public void getCinemas(ObservableCollection<Cinema> listReturn)
        {
            try
            {
                string commandString = $"select * from cinemas";
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, conn))
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                PreencheCinema(dr, listReturn);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        public ObservableCollection<Filme> getFilmesDoCinema(Cinema cinemaSelecionado)
        {

            ObservableCollection<Filme> filmesReturn = new ObservableCollection<Filme>();
            try
            {
                string commandString = $@"SELECT 
                                            cod, nome, 
                                            diretor, duracao_minutos 
                                          FROM 
                                            filmes 
                                          JOIN 
                                            filmes_cinema ON filmes.cod = filmes_cinema.filme_id 
                                          WHERE 
                                            filmes_cinema.cinema_id = {cinemaSelecionado.ID};";
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(commandString, conn))
                    {
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                PreencheFilme(dr, filmesReturn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return filmesReturn;

        }
        public void PreencheCinema(IDataReader dr, ObservableCollection<Cinema> listReturn)
        {
            while (dr.Read())
            {
                listReturn.Add(new Cinema(
                    dr.GetInt32(0),
                    new ObservableCollection<Filme>(),
                    dr.GetString(1),
                    dr.GetString(2),
                    dr.GetInt32(3)
                ));
            }

        }
        public void PreencheFilme(IDataReader dr, ObservableCollection<Filme> listReturn)
        {
            while (dr.Read())
            {
                listReturn.Add(new Filme(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetInt32(3)
                ));
            }
        }
        public void DropaTabelas()
        {
            query = @" DROP TABLE IF EXISTS filmes_cinema;
                       DROP TABLE IF EXISTS cinemas;
                       DROP TABLE IF EXISTS filmes;";
            ExecuteQuery(query);
        }
        public void CriaTabelas()
        {
            query = $@"
                    CREATE TABLE IF NOT EXISTS filmes (
                    cod SERIAL PRIMARY KEY,
                    nome VARCHAR(255) NOT NULL,
                    diretor VARCHAR(255) NOT NULL,
                    duracao_minutos INT NOT NULL
                    );";

            ExecuteQuery(query);

            query = $@"
                    CREATE TABLE IF NOT EXISTS cinemas (
                        cod SERIAL PRIMARY KEY,
                        nome VARCHAR(255) NOT NULL,
                        cidade VARCHAR(255) NOT NULL,
                        numero_salas INT NOT NULL
                    );";

            ExecuteQuery(query);

            query = $@"
                CREATE TABLE IF NOT EXISTS filmes_cinema (
                    cinema_id INT NOT NULL,
                    filme_id INT NOT NULL,
                    PRIMARY KEY (cinema_id, filme_id),
                    FOREIGN KEY (cinema_id) REFERENCES cinemas(cod) ON DELETE CASCADE,
                    FOREIGN KEY (filme_id) REFERENCES filmes(cod) ON DELETE CASCADE
                );";

            ExecuteQuery(query);
        }
        public void ExecuteQuery(string query)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool Update(IExibivel data)
        {
            try
            {
                if (data is Cinema)
                {
                    Cinema cinema = (Cinema)data;
                    query = $@"
                        UPDATE cinemas
                        SET nome = '{cinema.Nome}', cidade = '{cinema.Cidade}', numero_salas = '{cinema.NumeroDeSalas}' 
                        WHERE cod = '{cinema.ID}';";
                }
                else if (data is Filme)
                {
                    Filme filme = (Filme)data;
                    query = $@"
                        UPDATE filmes
                        SET nome = '{filme.Nome}', diretor = '{filme.Diretor}', duracao_minutos = '{filme.DuracaoMinutos}' 
                        WHERE cod = '{filme.ID}';";
                }

                ExecuteQuery(query);
            } catch (Exception ex) {
                MessageBox.Show("Erro no update", ex.Message);
                return false;
            }
            return true;
        }
        public bool Delete(int cod, IExibivel data)
        {
            if (data is Cinema)
            {
                query = $"DELETE FROM cinemas WHERE cod = {cod}";

            }
            else if (data is Filme)
            {
                query = $"DELETE FROM filmes WHERE cod = {cod}";

            } 

           try
            {
                ExecuteQuery(query);
            } catch(Exception ex)
            {
                MessageBox.Show("Erro no delete",ex.Message);
                return false;
            }
            return true;
        }
        public bool Create(IExibivel data)
        {
            if (data is Cinema cinema)
            {
                query = $"INSERT INTO cinemas (nome, cidade, numero_salas) VALUES ('{cinema.Nome}', '{cinema.Cidade}', {cinema.NumeroDeSalas})";
            }
            else if (data is Filme filme)
            {
                query = $"INSERT INTO filmes (nome, diretor, duracao_minutos) VALUES ('{filme.Nome}', '{filme.Diretor}', {filme.DuracaoMinutos})";
            }

            try
            {
                ExecuteQuery(query);
            } catch (Exception ex)
            {
                MessageBox.Show("Erro no Create", ex.Message);
                return false;
            }
            return true;
        }
    }
}
