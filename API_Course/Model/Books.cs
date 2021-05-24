using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Model
{
    [Table("Books")]
    public class Books : baseEntity
    {
        public string Title { get; set; }
        [Column("Author")]
        public string Author { get; set; }
        [Column("Price")]
        public decimal Price { get; set; }
        [Column("Launch_Date")]
        public DateTime LaunchDate { get; set; }
    }
}
