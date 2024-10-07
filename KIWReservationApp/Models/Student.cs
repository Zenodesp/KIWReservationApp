using System.ComponentModel.DataAnnotations;

namespace KIWReservationApp.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
