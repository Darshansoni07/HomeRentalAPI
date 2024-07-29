using HomeRent.Dbcontext;
using HomeRent.Dto;
using HomeRent.IRepository;
using HomeRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HomeRent.Repository
{    
    public class PropertyRepository : IPropertyRepository
    {
        private readonly AppDbContext _context;
        public PropertyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Properties>> GetProperties()
        {
            return await _context.HReProperties.
                Where(property => property.RequestPropertyApprove)
                .ToListAsync();
        }

        public async Task<Properties> GetPropertyById(int id)
        {
            return await _context.HReProperties.FindAsync(id);
        }

        public async Task<Properties> CreateProperty(Properties property)
        {            
            if (property.UsersId == null)
            {
                Console.WriteLine("Not Found User\n\n\n\n\n\n");                
            } 
            _context.HReProperties.Add(property);
            await _context.SaveChangesAsync();
            return property;
        }

        public async Task<Properties> UpdateProperty(int id, Properties property)
        {
            if (id != property.property_id)
            {
                throw new InvalidOperationException("ID mismatch");
            }

            _context.Entry(property).State = EntityState.Modified;
            //await _context.SaveChangesAsync();

            return property;
        }

        public async Task DeleteProperty(int id)
        {
            var property = await _context.HReProperties.FindAsync(id);

            if (property != null)
            {
                _context.HReProperties.Remove(property);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePropertyImage(int id, string image)
        {
            var property = await _context.HReProperties.FindAsync(id);
            if (property != null)
            {
                string[] img = property.Image_URL.Split(',');

                int indexToDelete = Array.IndexOf(img, image);

                if (indexToDelete != -1) 
                {
                    var tempList = new List<string>(img);
                    tempList.RemoveAt(indexToDelete);
                    img = tempList.ToArray();
                    property.Image_URL = string.Join(",", img);
                    _context.Update(property);
                    await _context.SaveChangesAsync();
                }
            }
        }

        /*public async Task<bool> RequestPropertyAccess(int properetyId)
        {
            var Property = await _context.HReProperties.FindAsync(properetyId);
            if (Property != null)
            {
                Property.RequestPropertyApprove = true; 
                _context.HReProperties.Update(Property);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }*/

        public async Task<bool> ApprovePropertyAccess(int propertyId)
        {
            var user = await _context.HReProperties.FindAsync(propertyId);
            //var email = user.Email;

            /*string recipientEmail = email; // Use the user's email from the database
            string subject = "Agent Access Approved";
            string body = "Dear User,\n\nYour agent access has been approved successfully.\n\nRegards,\nHomeRent Application";

            bool emailSent = SendEmail(recipientEmail, subject, body);*/
          
                if (user != null)
                {
                    user.RequestPropertyApprove = true; //Assuming '1' indicates agent access granted
                    _context.HReProperties.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
           
            return false;
        }

        public async Task<List<PropertyReqDto>> GetPropertyWithRequest()
        {
            return await _context.HReProperties
               .Where(u => !u.RequestPropertyApprove)
               .Select(u => new PropertyReqDto
               {
                   property_id = u.property_id,
                   description = u.description,
                   address = u.address,
                   city = u.city,
                   state = u.state,
                   Image_URL = u.Image_URL,
                   UpdatedOn = u.UpdatedOn
               })
               .ToListAsync();
        }
    }
}