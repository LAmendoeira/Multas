using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Viaturas
    {
        public int ID { get; set; }
        public string Matricula { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cor { get; set; }
        public string NomeDono { get; set; }
        public string MoradaDono { get; set; }
        public string CodPostalDono { get; set; }

        //Lista de multas associadas
        public virtual ICollection<Multas> ListaMultas { get; set; }

        public Viaturas()
        {
            ListaMultas = new HashSet<Multas>();
        }
    }
}