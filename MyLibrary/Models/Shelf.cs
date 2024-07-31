using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models
{
    [Table("Shelves")]
    public class Shelf
    {
        [Key]
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CategoryId { get; set; }
        public int AvailableSpace { get; set; }
        public Category? Category { get; set; }
        public List<Book>? Books { get; set; }

        public Shelf()
        {
            AvailableSpace = Width;//?
        }
    }
}
