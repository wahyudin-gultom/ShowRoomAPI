using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShowRoomAPI.Models.Entitas
{
    public abstract class GeneralColumn {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsRemoved { get; set; }
    }

    public class Car: GeneralColumn
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

    public class VMCar
    {
        public string Brand { get; set; }
        public string ModelNo { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
