namespace PackProApp.Utilities.Interfaces
{
    public interface IDataResult<T> : IResult where T : class
    {
        public T? Data { get; }
    }
}
