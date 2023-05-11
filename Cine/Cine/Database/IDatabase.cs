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
    public interface IDatabase
    {
        void ExecuteQuery(string query);
        bool Update(IExibivel data);
        bool Delete(int cod, IExibivel data);
        ObservableCollection<Filme> getFilmesDoCinema(Cinema cinemaSelecionado);
        bool AddMovie(Cinema cinema, Filme novoFilme);
        void CriaTabelas();
        void DropaTabelas();
        bool Create(IExibivel data);
        ObservableCollection<Cinema> ReadCinemas();
        void PreencheCinema(IDataReader dr, ObservableCollection<Cinema> listReturn);
        void PreencheFilme(IDataReader dr, ObservableCollection<Filme> listReturn);
        bool createRelationCinemaFilme(Cinema cinema, Filme novoFilme);
    }
}
