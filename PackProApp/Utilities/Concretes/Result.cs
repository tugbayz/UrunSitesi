using PackProApp.Utilities.Interfaces;
using IResult = PackProApp.Utilities.Interfaces.IResult;

namespace PackProApp.Utilities.Concretes
{
    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        public Result() 
        {
            IsSuccess = false;
            Message = string.Empty;
        }

        public Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public Result(bool isSuccess, string message) : this(isSuccess)
        {
            Message = message;
        }

    }
}
