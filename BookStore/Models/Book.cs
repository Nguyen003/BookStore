namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Book")]
    public partial class Book
    {
        public int BookID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public int AuthorID { get; set; }

        public decimal? Price { get; set; }

        [StringLength(200)]
        public string Images { get; set; }

        public int CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public DateTime? Published { get; set; }

        public int? ViewCount { get; set; }

        public virtual Author Author { get; set; }

        public virtual Category Category { get; set; }
    }
}
