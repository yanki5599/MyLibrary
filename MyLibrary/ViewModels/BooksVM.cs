namespace MyLibrary.ViewModels
{
    public class BooksVM
    {
        public List<BookVM> BooksVMs { get; set; } = new List<BookVM>();
        public int Height { get; set; }
        public string? SetOfBooksName { get; set; } = null;
        public int CategoryId { get; set; }

        public int TotalWidth 
        {
            get
            {
                return BooksVMs.Sum(x => x.Width);
            }
        }
    }
}
