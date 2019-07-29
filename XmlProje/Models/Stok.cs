namespace XmlProje.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stok")]
    public partial class Stok
    {
        public int ID { get; set; }

        public string Label { get; set; }

        public string Kod { get; set; }

        public string Ozellik { get; set; }

        public int? UrunID { get; set; }

        public virtual Urun Urun { get; set; }
    }
}
