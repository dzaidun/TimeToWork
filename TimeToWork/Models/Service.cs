using System.ComponentModel.DataAnnotations;

namespace TimeToWork.Models
{
	public class Service
	{
		public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Service Name")]
		[StringLength(50, MinimumLength = 3)]
		public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Short Description")]
		public string ShortDescription { get; set; }

        
        [Display(Name = "Description")]
		public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Execution Time Hours")]
        [Range(0, 23,
        ErrorMessage = "Maximum 23 hours")]
        public int ЕxecutionTimeHours { get; set; }

        [Required]
        [Display(Name = "Execution Time Minutes")]
        [Range(0, 59,
        ErrorMessage = "Maximum 59 minutes")]
        public int ЕxecutionTimeMinutes { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
		public ICollection<ServiceAssignment> ServiceAssignments { get; set; }
	}
}
