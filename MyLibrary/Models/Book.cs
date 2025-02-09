﻿using MyLibrary.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Models
{
    [Table("Books")]
    public class Book
    {
        public Book(BookVM bookVm ,int height, int shelfId , int? setOfBooksId = null)
        {
            Id = bookVm.Id;
            Name = bookVm.Name;
            Width = bookVm.Width;
            Height = height;
            ShelfId = shelfId;
            SetOfBooksId = setOfBooksId;
        }

        public Book() { }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int? SetOfBooksId { get; set; }
        [Display(Name ="Is a part of a set")]
        public SetOfBooks? SetOfBooks { get; set; }
        public int ShelfId { get; set; }
        public Shelf Shelf { get; set; }

    }
}
