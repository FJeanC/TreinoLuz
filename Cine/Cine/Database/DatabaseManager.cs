using Cine.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Database
{
    public class DatabaseManager : IDatabase
    {
        public IDatabase database { get; set; }

        public DatabaseManager(IDatabase db) {
            database = db;
        }
        public bool AddMovie(Cinema cinema, Filme novoFilme)
        {
           bool movieAdded = database.AddMovie(cinema, novoFilme); ;
            if(movieAdded)
            {
                bool relation = database.createRelationCinemaFilme(cinema, novoFilme);
                if(relation)
                {
                    return true;
                }
            }
            return false;

        }

        public bool Create(IExibivel data)
        {
            bool created = database.Create(data);
            return created;
        }

        public void CriaTabelas()
        {
            database.CriaTabelas();
        }

        public bool Delete(int cod, IExibivel data)
        {
            bool deleted = database.Delete(cod, data);                             
            return deleted;
        }

        public void DropaTabelas()
        {
            database.DropaTabelas();
        }

        public void ExecuteQuery(string query)
        {
            database.ExecuteQuery(query);
        }

        public ObservableCollection<Filme> getFilmesDoCinema(Cinema cinemaSelecionado)
        {
            return database.getFilmesDoCinema(cinemaSelecionado);
        }

        public void PreencheCinema(IDataReader dr, ObservableCollection<Cinema> listReturn)
        {
            database.PreencheCinema(dr, listReturn);    
        }

        public void PreencheFilme(IDataReader dr, ObservableCollection<Filme> listReturn)
        {
            database.PreencheFilme(dr, listReturn); 
        }

        public ObservableCollection<Cinema> ReadCinemas()
        {
            return database.ReadCinemas();         
        }

        public bool Update(IExibivel data)
        {
            bool updated = database.Update(data);                  
            return updated;
        }

        public bool createRelationCinemaFilme(Cinema cinema, Filme novoFilme)
        {
            return database.createRelationCinemaFilme(cinema, novoFilme);
        }
    }
}
