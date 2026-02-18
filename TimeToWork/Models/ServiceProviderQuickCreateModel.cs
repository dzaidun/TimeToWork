namespace TimeToWork.Models
{
    public class ServiceProviderQuickCreateModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public int PlaceOfWorkID { get; set; }
        public int[] SelectedServices { get; set; }
    }
}
