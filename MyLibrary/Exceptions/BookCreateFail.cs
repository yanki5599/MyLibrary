namespace MyLibrary.Exceptions
{
    public class BookCreateFail : Exception
    {

        public BookCreateFail(string errorMsg):base(errorMsg) { }
    }
}
