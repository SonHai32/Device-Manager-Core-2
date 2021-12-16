using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceManagerCore.Models;
namespace DeviceManagerCore.Respository
{
    public interface IDeviceRespository
    {
        Task<IEnumerable<Device>> GetAllDevice();
        Task<IEnumerable<Device>> GetById(int id);
        Task<bool> AddNew(Device device);
        Task<bool> Update(Device device);
        Task<bool> Delete(int[] id);
    }
}
