using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeRent.Models
{
    public class Properties
    {
        [Key]
        public int property_id { get; set; }             
        [Required]
        public string description { get; set; }
        [Required]
        public string address { get; set; }
        public string city { get; set;}
        public string state { get; set;}
        public string Image_URL { get; set;}

        public string Flat { get; set; }
        public string Area { get; set; }
        public string independent { get; set; }
        public int bath {  get; set; }
        //add new data 28/12/23
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
        public bool RequestPropertyApprove { get; set; }

        [Required]
        public int rent_amount { get; set;}                
        public int UsersId { get; set;}
        public Users Users { get; set;}

        //public ICollection<HomeBook> Booking { get; set; }

    }
}
