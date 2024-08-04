using MyLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(1, int.MaxValue)]
        public int Width { get; set; }
       /* [Range(1, int.MaxValue)]
        public int Height { get; set; }*/
    }
}
