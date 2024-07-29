using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class PropertyDto
    {
        public int property_id { get; set; }
        public string description { get; set; }       
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string? Image_URL { get; set; }       
        public int rent_amount { get; set; }
        public int UsersId { get; set; }
        public string Flat { get; set; }
        public string Area { get; set; }
        public string independent { get; set; }
        public int bath { get; set; }
        public string[]? fileData { get; set; }
        //after add 29/12/23
        public DateTime UpdatedOn { get; set; }
        public string avilablefor { get; set; }
        public int badroom { get; set; }
        public int kitchen { get; set; }
        public int fan { get; set; }
        public int exhaustfan { get; set; }
        public int stove { get; set; }
        public int light { get; set; }
        public int fridge { get; set; }
        public int washingMachine { get; set; }
        public int wardrobe { get; set; }
        public int sofa { get; set; }
        public int bed { get; set; }
        public int dining { get; set; }
        public int ac { get; set; }
        public int waterpurifier { get; set; }
        public int tv { get; set; }
        public int floor { get; set; }
        public int parking { get; set; }
        public int agreement { get; set; }
        public int balcony { get; set; }
        //to
    }

    public class fileUpload
    {
        public string File_Name { get;set; }
        public int File_Size { get; set; }
        public Byte[] bytes { get; set; }
        public string File_Type { get; set; }
    }
}
