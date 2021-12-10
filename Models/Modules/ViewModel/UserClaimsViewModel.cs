using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.ViewModel
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }

        public class UserClaim
        {
            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
