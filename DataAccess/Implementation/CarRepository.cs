using Microsoft.EntityFrameworkCore;
using ShowRoomAPI.DataAccess.Interface;
using ShowRoomAPI.Models.Entitas;

namespace ShowRoomAPI.DataAccess.Implementation
{
    public class CarRepository : ICarRepository
    {
        private readonly ShowRoomDataContext _dbContext;
        public CarRepository(ShowRoomDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Car>> GetAllAsync()
        {
            return _dbContext.Cars.ToListAsync();
        }

        public async Task<Car> GetById(string id)
        {
            var car = await _dbContext.Cars.FirstOrDefaultAsync(m => m.SerialNo == id);
            if (car != null) return car;

            return new Car();
        }

        public Task<Car> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsCanDelete(Car entity)
        {
            _dbContext.Cars.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCanSave(Car entity)
        {
            _dbContext.Cars.Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCanUpdate(Car entity)
        {
            _dbContext.Cars.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
