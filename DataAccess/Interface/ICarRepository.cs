using ShowRoomAPI.Models.Entitas;

namespace ShowRoomAPI.DataAccess.Interface
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<bool> IsCanSave(Car entity);
        Task <bool> IsCanUpdate(Car entity);
        Task <bool> IsCanDelete(Car entity);
        Task <List<Car>> GetAllAsync();
        Task<Car> GetById(string id);
        Task<Car> GetById(int id);
    }
}
