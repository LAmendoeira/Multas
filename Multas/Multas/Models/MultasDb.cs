using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class MultasDb : DbContext
    {

        //Definir tabelas da DB
        public virtual DbSet<Viaturas> Viaturas { get; set; }
        public virtual DbSet<Agentes> Agentes { get; set; }
        public virtual DbSet<Condutores> Condutores { get; set; }
        public virtual DbSet<Multas> Multas { get; set; }

        //Construtor
        public MultasDb() : base("MultasDbConnectionString")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}