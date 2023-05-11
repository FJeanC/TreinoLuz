using Cine.Database;
using Cine.Utils;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Cine
{
    public class MainWindowVM : BaseNotify
    {
        public ObservableCollection<Cinema> listaDeCinemas { get; set; }
        public ICommand Add { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Update { get; private set; }
        public ICommand AddMovies { get; private set; }
        public ICommand RemoveMovies { get; private set; }
        public ICommand UpdateMovies { get; private set; }
        public Filme FilmeSelecionado { get; set; }
        public Cinema _cinemaSelecionado { get; set; }
       
        private IDatabase conn;
        public MainWindowVM()
        {
            conn = new DatabaseManager(new PostgreConnection());
            listaDeCinemas = new ObservableCollection<Cinema>(conn.ReadCinemas());
            IniciaComandos();
        }
        public Cinema CinemaSelecionado
        {
            get { return _cinemaSelecionado; }
            set
            {
                _cinemaSelecionado = value;
                if (value != null)
                {
                    _cinemaSelecionado.ListaFilmes = conn.getFilmesDoCinema(_cinemaSelecionado);
                    Notify();
                }
            }
        }
        public void IniciaComandos()
        {
            Add = new RelayCommand((o) =>
            {
                Cinema newCine = new Cinema();
                CinemaScreen cinemaScreen = new CinemaScreen
                {
                    DataContext = newCine
                };

                bool? validation = cinemaScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    conn.Create(newCine);
                    listaDeCinemas = conn.ReadCinemas();
                    Notify(nameof(listaDeCinemas));
                }

            });

            Remove = new RelayCommand((o) =>
            {
                conn.Delete(_cinemaSelecionado.ID, CinemaSelecionado);
                listaDeCinemas = conn.ReadCinemas();
                Notify(nameof(listaDeCinemas));
            });

            Update = new RelayCommand((o) =>
            {
                if (_cinemaSelecionado == null)
                {
                    return;
                }

                Cinema copy = _cinemaSelecionado.ShallowCopy();
                CinemaScreen cineScreen = new CinemaScreen
                {
                    DataContext = copy
                };

                bool? validation = cineScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    conn.Update(copy);
                    listaDeCinemas = conn.ReadCinemas();
                    Notify(nameof(listaDeCinemas));
                }

            });

            AddMovies = new RelayCommand((o) =>
            {
                Filme novoFilme = new Filme();
                CreateUpdateFilme createMovieScreen = new CreateUpdateFilme
                {
                    DataContext = novoFilme
                };

                bool? validation = createMovieScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    conn.AddMovie(CinemaSelecionado, novoFilme);
                    CinemaSelecionado.ListaFilmes = conn.getFilmesDoCinema(CinemaSelecionado);
                    Notify();

                }
            });

            RemoveMovies = new RelayCommand((o) => {
                if (FilmeSelecionado != null)
                {
                    conn.Delete(FilmeSelecionado.ID, FilmeSelecionado);
                    CinemaSelecionado.ListaFilmes = conn.getFilmesDoCinema(CinemaSelecionado);
                    Notify();
                }
            });

            UpdateMovies = new RelayCommand((o) =>
            {
                if (FilmeSelecionado == null)
                {
                    return;
                }
                Filme copy = FilmeSelecionado.ShallowCopy();
                CreateUpdateFilme filmeScreen = new CreateUpdateFilme { DataContext = copy };
                bool? validation = filmeScreen.ShowDialog();
                if (validation.HasValue && validation.Value)
                {
                    conn.Update(copy);
                    CinemaSelecionado.ListaFilmes = conn.getFilmesDoCinema(CinemaSelecionado);
                    Notify();
                }
            });
        }


    }
}


