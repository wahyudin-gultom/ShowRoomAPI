namespace ShowRoomAPI.DataAccess.Interface
{
    public interface IRepository<T>
    {
        Task<bool> IsCanSave(T entity);
        Task <bool> IsCanUpdate(T entity);
        Task <bool> IsCanDelete(T entity);
        Task <List<T>> GetAllAsync();
        Task<T> GetById(string id);
        Task<T> GetById(int id);
    }
}
