namespace MyLibrary.Exceptions
{
    public class NoAvailableShelf : Exception
    {

        public NoAvailableShelf(string errorMsg):base(errorMsg) { }
    }
}
