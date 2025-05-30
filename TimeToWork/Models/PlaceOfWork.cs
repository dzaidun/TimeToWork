using System.ComponentModel.DataAnnotations;

namespace TimeToWork.Models
{
    public class PlaceOfWork
    {
        public int PlaceOfWorkID { get; set; }

        [Required]
        [Display(Name = "Місце роботи")]
        [StringLength(250)]
        public string Location { get; set; }

        public ICollection<ServiceProvider> ServiceProvider { get; set; }
    }
}
