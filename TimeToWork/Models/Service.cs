using System.ComponentModel.DataAnnotations;

namespace TimeToWork.Models
{
	public class Service
	{
		public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Назва послуги")]
		[StringLength(50, MinimumLength = 3)]
		public string ServiceName { get; set; }

        [Required]
        [Display(Name = "Короткий опис")]
		public string ShortDescription { get; set; }

        
        [Display(Name = "Опис")]
		public string Description { get; set; }

        [Required]
        [Display(Name = "Ціна")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Час зустрічі годин")]
        [Range(0, 23,
        ErrorMessage = "Максимум 23 години")]
        public int ЕxecutionTimeHours { get; set; }

        [Required]
        [Display(Name = "Час зустрічі хвилин")]
        [Range(0, 59,
        ErrorMessage = "Максимум 59 хвилин")]
        public int ЕxecutionTimeMinutes { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
		public ICollection<ServiceAssignment> ServiceAssignments { get; set; }
	}
}
