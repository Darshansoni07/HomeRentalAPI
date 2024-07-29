using System.ComponentModel.DataAnnotations;

namespace HomeRent.Dto
{
    public class PropertyReqDto
    {
        public int property_id { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string Image_URL { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}