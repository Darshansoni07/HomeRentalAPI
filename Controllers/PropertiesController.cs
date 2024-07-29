using AutoMapper;
using HomeRent.Dto;
using HomeRent.IRepository;
using HomeRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeRent.Controllers
{
    //[Authorize]
    public class PropertiesController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PropertiesController(IWebHostEnvironment webHostEnvironment,IUnitOfWork uow,IMapper mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            this._uow = uow;
            this._mapper = mapper;
        }

        [HttpGet("gettAllProperty")]
        public async Task<IActionResult> GetProperties()
        {
            var properties = await _uow.PropertyRepository.GetProperties();
            var propertiesDto = _mapper.Map<IEnumerable<PropertyDto>>(properties);
            return Ok(propertiesDto);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetProperty(int id)
        {
            var property = await _uow.PropertyRepository.GetPropertyById(id);

            if (property == null)
            {
                return NotFound();
            }

            var propertyDto = _mapper.Map<PropertyDto>(property);
            return Ok(propertyDto);
        }

        [HttpPost("addlist")]
        public async Task<IActionResult> CreateProperty(PropertyDto propertyDto)
        {
            string baseURL = @"assets/images/category/hotel/";
            string[] dataArray  = propertyDto.fileData;
            string imageUrl = "";          
            List<string> cleanedDataList = new List<string>();
            foreach (var base64ImageData in dataArray)
            {
                string cleanedBase64Image = base64ImageData
                    .Replace("{[", "")  // Remove leading characters
                    .Replace("]}", "")  // Remove trailing characters
                    .Replace("\\r\\n", "") 
                    .Trim(); // Example trim
                cleanedDataList.Add(cleanedBase64Image);
            }                        
            for (int i = 0; i < cleanedDataList.Count; i++)
            {
                string base64String = cleanedDataList[i].Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64String);
                string directoryPath = @"D:\CISProject\Assignment Project\HomeRentAngular\src\assets\images\category\hotel";
                //string directoryPath = @"D:\CISProject\HomeRental\wwwroot\assets\images\category\hotel";
                string fileName = $"image_{DateTime.Now.Ticks}_{i}.png"; // Customize file name

                string filePath = Path.Combine(directoryPath, fileName);

                System.IO.File.WriteAllBytes(filePath, bytes);
                imageUrl += (imageUrl != "" ? "," : "") + baseURL + fileName;
            }
            propertyDto.Image_URL = imageUrl;

            //fileData
            if (propertyDto.Image_URL!=null) { 
                    var property = _mapper.Map<Properties>(propertyDto);
                    var createdProperty = await _uow.PropertyRepository.CreateProperty(property);
                    var createdPropertyDto = _mapper.Map<PropertyDto>(createdProperty);
                    await _uow.SaveAsync();
                    return Ok("Success");
            }
            else
            {
               return BadRequest("Invalid source path or file does not exist.");
            }
           
        }

        //Request send to admin 
        /*[HttpPost("request-property-access/{propertyId}")]
        public async Task<IActionResult> RequestPropertyAccess(int propertyId)
        {
            var result = await _uow.PropertyRepository.RequestPropertyAccess(propertyId);
            if (result)
            {
                return Ok("Property access requested successfully.");
            }
            return NotFound("User not found or request failed.");
        }*/

        //Request approved  ok test
        [HttpPost("approve-property-access/{propertyId}")]
        public async Task<IActionResult> ApprovePropertyAccess(int propertyId)
        {
            var result = await _uow.PropertyRepository.ApprovePropertyAccess(propertyId);
            if (result)
            {  
                return Ok("Property access requested successfully.");
            }
            return NotFound("User not found or request failed.");
        }

        //admin will get All Property   ok test
        [HttpGet("property-with-request")]
        public async Task<IActionResult> GetUsersWithAgentRequest()
        {
            var result = await _uow.PropertyRepository.GetPropertyWithRequest();
            return Ok(result);
        }


        //edit 
        /*[HttpPut("updateProperty/{id}")]
        public async Task<IActionResult> UpdateProperty(int id, PropertyDto propertyDto)
        {
            string baseURL = @"assets/images/category/hotel/";
            string[] dataArray = propertyDto.fileData;
            string imageUrl = "";
            List<string> cleanedDataList = new List<string>();
            foreach (var base64ImageData in dataArray)
            {
                string cleanedBase64Image = base64ImageData
                    .Replace("{[", "")  // Remove leading characters
                    .Replace("]}", "")  // Remove trailing characters
                    .Replace("\\r\\n", "")
                    .Trim(); // Example trim
                cleanedDataList.Add(cleanedBase64Image);
            }
            for (int i = 0; i < cleanedDataList.Count; i++)
            {
                string base64String = cleanedDataList[i].Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64String);
                string directoryPath = @"D:\CISProject\Assignment Project\HomeRentAngular\src\assets\images\category\hotel";
                //string directoryPath = @"D:\CISProject\HomeRental\wwwroot\assets\images\category\hotel";
                string fileName = $"image_{DateTime.Now.Ticks}_{i}.png"; // Customize file name

                string filePath = Path.Combine(directoryPath, fileName);

                System.IO.File.WriteAllBytes(filePath, bytes);
                imageUrl += (imageUrl != "" ? "," : "") + baseURL + fileName;
            }
            propertyDto.Image_URL = imageUrl;

            //fileData
            if (propertyDto.Image_URL != null)
            {
                var property = _mapper.Map<Properties>(propertyDto);
                var createdProperty = await _uow.PropertyRepository.CreateProperty(property);
                var createdPropertyDto = _mapper.Map<PropertyDto>(createdProperty);
                await _uow.SaveAsync();
                return Ok("Success");
            }
            else
            {
                return BadRequest("Invalid source path or file does not exist.");
            }            
        }*/


        [HttpPut("updateProperty/{id}")]
        public async Task<IActionResult> UpdateProperty(int id, PropertyDto propertyDto)
        {
            try
            {
                var existingProperty = await _uow.PropertyRepository.GetPropertyById(id);

                if (existingProperty == null)
                {
                    return NotFound("Property not found");
                }

                // Update the existing property fields based on the received propertyDto
                existingProperty.description = propertyDto.description;
                existingProperty.address = propertyDto.address;
                existingProperty.city = propertyDto.city;
                existingProperty.state = propertyDto.state;
                existingProperty.Image_URL = existingProperty.Image_URL;
                existingProperty.rent_amount = propertyDto.rent_amount;
                existingProperty.Flat = propertyDto.Flat;
                existingProperty.Area = propertyDto.Area;
                existingProperty.independent = propertyDto.independent;
                existingProperty.bath = propertyDto.bath;
                existingProperty.UpdatedOn = propertyDto.UpdatedOn;
                existingProperty.avilablefor = propertyDto.avilablefor;
                existingProperty.badroom = propertyDto.badroom;
                existingProperty.kitchen = propertyDto.kitchen;
                existingProperty.fan = propertyDto.fan;
                existingProperty.exhaustfan = propertyDto.exhaustfan;
                existingProperty.stove = propertyDto.stove;
                existingProperty.light = propertyDto.light;
                existingProperty.fridge = propertyDto.fridge;
                existingProperty.washingMachine = propertyDto.washingMachine;
                existingProperty.wardrobe = propertyDto.wardrobe;
                existingProperty.sofa = propertyDto.sofa;
                existingProperty.bed = propertyDto.bed;
                existingProperty.dining = propertyDto.dining;
                existingProperty.ac = propertyDto.ac;
                existingProperty.waterpurifier = propertyDto.waterpurifier;
                existingProperty.tv = propertyDto.tv;
                existingProperty.floor = propertyDto.floor;
                existingProperty.parking = propertyDto.parking;
                existingProperty.agreement = propertyDto.agreement;
                existingProperty.balcony = propertyDto.balcony;

                if (propertyDto.fileData != null)
                {
                    string baseURL = @"assets/images/category/hotel/";
                    string[] dataArray = propertyDto.fileData;
                    string imageUrl = "";
                    List<string> cleanedDataList = new List<string>();

                    foreach (var base64ImageData in dataArray)
                    {
                        string cleanedBase64Image = base64ImageData
                            .Replace("{[", "")  // Remove leading characters
                            .Replace("]}", "")  // Remove trailing characters
                            .Replace("\\r\\n", "")
                            .Trim(); // Example trim
                        cleanedDataList.Add(cleanedBase64Image);
                    }

                    for (int i = 0; i < cleanedDataList.Count; i++)
                    {
                        string base64String = cleanedDataList[i].Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64String);
                        string directoryPath = @"D:\CISProject\Assignment Project\HomeRentAngular\src\assets\images\category\hotel";
                        string fileName = $"image_{DateTime.Now.Ticks}_{i}.png"; // Customize file name
                        string filePath = Path.Combine(directoryPath, fileName);

                        System.IO.File.WriteAllBytes(filePath, bytes);
                        imageUrl += (imageUrl != "" ? "," : "") + baseURL + fileName;
                    }

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        existingProperty.Image_URL += (existingProperty.Image_URL != "" ? "," : "") + imageUrl;
                    }
                }
                _uow.PropertyRepository.UpdateProperty(id,existingProperty);
                await _uow.SaveAsync();

                return Ok("Property updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update property: {ex.Message}");
            }
        }






        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            await _uow.PropertyRepository.DeleteProperty(id);
            return Ok("success");
        }

        [HttpDelete("deleteImage")]
        public async Task<IActionResult> DeletePropertyImage(int id, string image)
        {
            await _uow.PropertyRepository.DeletePropertyImage(id, image);
            return Ok("success");
        }
    }
}

/*for (int i = 0; i < dataArray.Length; i++)
            {
                string base64String = dataArray[i].Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64String);
                string directoryPath = @"D:\CISProject\Assignment Project\HomeRentAngular\src\assets\images\category\hotel";
                string fileName = $"image_{DateTime.Now.Ticks}_{i}.png";
                string filePath = Path.Combine(directoryPath, fileName);
                System.IO.File.WriteAllBytes(filePath, bytes);
                imageUrl += (imageUrl != "" ? "," : "") + baseURL + fileName;
                Console.WriteLine($"Image {i + 1} saved at: {imageUrl}");
            }*/

/*[HttpPost("addlist")]
public async Task<IActionResult> CreateProperty(PropertyDto propertyDto)
{
    propertyDto.Image_URL = "assets/images/category/hotel/01.jpg";
    var property = _mapper.Map<Properties>(propertyDto);
    var createdProperty = await _uow.PropertyRepository.CreateProperty(property);
    var createdPropertyDto = _mapper.Map<PropertyDto>(createdProperty);
    await _uow.SaveAsync();
    return Ok("Success");
}*/