using GoReal.Models.Entities;

namespace GoReal.Models.Api
{
    public class User
    {
        public int UserId { get; set; }
        public string GoTag { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public bool isActive { get; set; }
        public bool isBan { get; set; }
        public Role Roles { get; set; }
    }
}
