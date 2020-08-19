using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string GoTag { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int Ranking { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        public bool isActive { get; set; }
    }
}
