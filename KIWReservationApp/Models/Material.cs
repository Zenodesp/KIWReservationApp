
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIWReservationApp.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Display(Name="Volgnummer")]
        public string? SerialNumber { get; set; }
        [Display(Name = "Start Reservatie")]
        [DataType(DataType.DateTime)]
        public DateTime? PickupTime { get; set; }
        [Display(Name = "Gereserveerd?")]
        public bool IsReserved { get; set; }
        [Display(Name = "Afgehaald?")]
        public bool IsPickedUp { get; set; }
        [Display(Name = "Gereserveerd door")]
        public string? UserReserved { get; set; }
        [Display(Name = "Einde Reservatie")]
        [DataType(DataType.DateTime)]
        public DateTime? ReturnTime { get; set; }
        [Display(Name = "Teruggegeven?")]
        public bool IsReturned { get; set; }

        


    }

    
}
