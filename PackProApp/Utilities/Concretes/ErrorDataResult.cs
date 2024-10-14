namespace PackProApp.Utilities.Concretes
{
    public class ErrorDataResult<T> : DataResult<T> where T : class
    {
        public ErrorDataResult() : base(default, false) { }
        public ErrorDataResult(string messages) : base(default, false, messages) { }
        public ErrorDataResult(T data, string messages) : base(data, false, messages) { }




    }
}
