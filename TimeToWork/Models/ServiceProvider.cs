using System.ComponentModel.DataAnnotations;

namespace TimeToWork.Models
{
	public class ServiceProvider
	{
		public int ID { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		[StringLength(50)]
		public string LastName { get; set; }

		[Required]
		[Display(Name = "First Name")]
		[StringLength(50)]
		public string FirstName { get; set; }

		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Display(Name = "Hire Date")]
		public DateTime HireDate { get; set; }

		[Display(Name = "Full Name")]
		public string FullName
		{
			get { return LastName + " " + FirstName; }
		}

        public int PlaceOfWorkID { get; set; }

        public PlaceOfWork PlaceOfWork { get; set; }

		public ICollection<ServiceAssignment> ServiceAssignments { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
