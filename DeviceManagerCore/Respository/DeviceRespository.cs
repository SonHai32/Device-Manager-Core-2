using DeviceManagerCore.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagerCore.Respository
{


    public class DeviceRespository : IDeviceRespository
    {
        public async Task<bool> AddNew(Device device)
        {
            using var dbContext = new DeviceDbContext();
            var result = await dbContext.Devices.AddAsync(device);
            bool success = dbContext.SaveChanges() > 0;
            return success;

        }

        public async Task<bool> Delete(int[] id)
        {
            using var dbContext = new DeviceDbContext();
            dbContext.Devices.RemoveRange(dbContext.Devices.Where(r => id.Contains(r.DeviceId)));
            bool success = await dbContext.SaveChangesAsync() > 0;
            return success;
        }

        public async Task<IEnumerable<Device>> GetAllDevice()
        {
            using var dbContext = new DeviceDbContext();
            var result = dbContext.Devices.ToListAsync();
            return await result;
        }

        public async Task<IEnumerable<Device>> GetById(int id)
        {
            using var dbContext = new DeviceDbContext();
            var result = dbContext.Devices.Where(d => d.DeviceId == id).ToListAsync();
            return await result;
        }

        public async Task<bool> Update(Device device)
        {
            using var dbContext = new DeviceDbContext();
            dbContext.Update<Device>(device);

            bool success = await dbContext.SaveChangesAsync() > 0;

            return success;
        }
    }
}
