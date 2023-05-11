using Cine.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;

namespace Cine
{
    public class Filme : BaseNotify, IExibivel
    {
        private string nome;
        private string diretor;
        private int duracaoMinutos;
        private int _id;
        public Filme() { }
        public Filme(int id, string nome, string diretor, int duracaoMinutos)
        {
            _id = id;
            this.nome = nome;
            this.diretor = diretor;
            this.duracaoMinutos = duracaoMinutos;
        }
        public string Nome
        {
            get { return nome; }
            set { nome = value; Notify(nameof(Nome)); }
        }
        public string Diretor
        {
            get { return diretor; }
            set { diretor = value; Notify(nameof(Diretor)); }
        }
        public int DuracaoMinutos
        {
            get { return duracaoMinutos; }
            set
            {
                duracaoMinutos = value;
                Notify(nameof(DuracaoMinutos));
            }
        }
        public int ID { get { return _id; } set { _id = value; } }
        public Filme ShallowCopy()
        {
            return (Filme)this.MemberwiseClone();
        }
    }
}
