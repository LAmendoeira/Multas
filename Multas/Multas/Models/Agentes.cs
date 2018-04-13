using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage ="O {0} do agente é necessário.")]
        [StringLength(40, ErrorMessage ="Apenas pode conter {1} caracteres.")]
        [RegularExpression("[A-ZÂÊÎÔÛÁÉÍÓÚ][a-záéíóúàèìòùãõâêîôûç]+(( | e | de | da | das )[A-ZÂÊÎÔÛÁÉÍÓÚ][a-záéíóúàèìòùãõâêîôûç]+){1,3}", ErrorMessage ="Ver regras")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A {0} do agente é necessária.")]
        public string Fotografia { get; set; }

        [Required(ErrorMessage = "Indique a que {0} pertence o agente.")]
        [RegularExpression("[A-Za-z0-9çé -]+", ErrorMessage = "Apenas são aceites caracteres alfanuméricos")]
        public string Esquadra { get; set; }

        //Lista de multas associadas
        public virtual ICollection<Multas> ListaMultas { get; set; }

        public Agentes()
        {
            ListaMultas = new HashSet<Multas>();
        }

    }
}