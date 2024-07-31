using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models
{
    [Table("SetsOfBooks")]
    public class SetOfBooks
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        List<Book>? Books { get; set; }
    }
}
