namespace Orion.Models
{
    public class UserModel
    {
        public int Id { get; }
        public string ExternalId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
