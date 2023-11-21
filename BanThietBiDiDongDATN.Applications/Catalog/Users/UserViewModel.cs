namespace BanThietBiDiDongDATN.Application.Catalog.Users
{
    public class UserViewModel
    {
        public string id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string NumberPhone { get; set; }
        public string? Avatar { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
    }
}
