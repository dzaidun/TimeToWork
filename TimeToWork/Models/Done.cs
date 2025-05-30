using System.ComponentModel.DataAnnotations;

namespace TimeToWork.Models
{
    public class Done
    {
        public int DoneId { get; set; }
        public int ServiceId { get; set; }
        public int ClientId { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        public int ServiceProviderID { get; set; }

        public Service Service { get; set; }
        public Client Client { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
    }
}
