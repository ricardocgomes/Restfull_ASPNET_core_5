using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Model
{
    public class baseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
