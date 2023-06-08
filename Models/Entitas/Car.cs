using System.ComponentModel.DataAnnotations;

namespace ShowRoomAPI.Models.Entitas
{
    public class Car
    {
        [Required, Key]
        public string SerialNo { get; set; }
        public string Brand { get; set; }
        public string ModelNo { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
