using Cine.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Cine
{
    public class Cinema : BaseNotify, IExibivel
    {
        private ObservableCollection<Filme> listaFilmes;
        private int numeroDeSalas;
        private string cidade;
        private string nome;
        private int _id;
        public Cinema()
        {
            listaFilmes = new ObservableCollection<Filme>();

        }
        public Cinema(int id, ObservableCollection<Filme> listaFilmes, string nome, string cidade, int numeroDeSalas)
        {
            this.ID = id;
            this.listaFilmes = listaFilmes;
            this.numeroDeSalas = numeroDeSalas;
            this.cidade = cidade;
            this.nome = nome;
        }
        public int NumeroDeSalas { get { return numeroDeSalas; } set { numeroDeSalas = value; Notify(nameof(NumeroDeSalas)); } }
        public string Cidade { get { return cidade; } set { cidade = value; Notify(nameof(Cidade)); } }
        public string Nome { get { return nome; } set { nome = value; Notify(nameof(Nome)); } }
        public ObservableCollection<Filme> ListaFilmes { get { return listaFilmes; } set { listaFilmes = value; Notify(nameof(listaFilmes)); } }
        public int ID { get { return _id; } set { _id = value; } }
        public Cinema ShallowCopy()
        {
            return (Cinema)this.MemberwiseClone();
        }

    }


}
