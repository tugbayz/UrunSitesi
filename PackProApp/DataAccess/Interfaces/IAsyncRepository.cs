namespace PackProApp.DataAccess.Interfaces
{
    public interface IAsyncRepository
    {
        Task<int> SaveChangeAsync();
    }
}
