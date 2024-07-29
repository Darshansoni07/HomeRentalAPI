using HomeRent.Dto;
using HomeRent.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HomeRent.IRepository
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Properties>> GetProperties();
        Task<Properties> GetPropertyById(int id);
        Task<Properties> CreateProperty(Properties property);
        Task<Properties> UpdateProperty(int id, Properties property);
        Task DeleteProperty(int id);
        Task DeletePropertyImage(int id, string image);
        //Task<bool> RequestPropertyAccess(int properetyId);
        Task<bool> ApprovePropertyAccess(int propertyId);
        Task<List<PropertyReqDto>> GetPropertyWithRequest();
    }
}
