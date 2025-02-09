﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyLibrary.ViewModels
{
    public class BooksVM
    {
        public List<BookVM> Books { get; set; } = new List<BookVM>();

        public BookVM? FirstBook { get; set; }
        public bool IsSet {  get; set; } = false;
        [Range(1,int.MaxValue)]
        public int Height { get; set; }
        public string? SetOfBooksName { get; set; } = null;
        public int CategoryId { get; set; }



        public int TotalWidth 
        {
            get
            {
                return Books.Sum(x => x.Width) + (FirstBook?.Width ?? 0);
            }
        }
    }
}
