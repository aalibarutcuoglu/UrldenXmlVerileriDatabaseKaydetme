namespace XmlProje.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class XmlProjeDBContext : DbContext
    {
        public XmlProjeDBContext()
            
        {
            Database.Connection.ConnectionString = "Server=***;Database=***;UID=***;PWD=***";
        }

        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Stok> Stok { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Urun> Urun { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>()
                .Property(e => e.Fiyat)
                .HasPrecision(19, 4);
        }
    }
}
