using System.ComponentModel.DataAnnotations;

namespace TimeToWork.Models
{
	public class Client
	{
		public int ID { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Display(Name = "Full Name")]
		public string FullName
		{
			get
			{
				return LastName + " " + FirstName;
			}
		}

        [Required]
        [StringLength(15, ErrorMessage = "No more than 15 characters.")]
        [Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		public ICollection<Appointment> Appointments { get; set; }
	}
}
