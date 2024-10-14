namespace PackProApp.Utilities.Concretes
{
    public class SuccessDataResult<T> : DataResult<T> where T : class
    {
        public SuccessDataResult() : base(default, true) { }
        public SuccessDataResult(string messages) : base(default, true, messages) { }
        public SuccessDataResult(T data, string messages) : base(data, true, messages) { }
    }
}
