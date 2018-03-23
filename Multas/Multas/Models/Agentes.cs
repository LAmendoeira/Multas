using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Fotografia { get; set; }
        public string Esquadra { get; set; }

        //Lista de multas associadas
        public virtual ICollection<Multas> ListaMultas { get; set; }

        public Agentes()
        {
            ListaMultas = new HashSet<Multas>();
        }

    }
}