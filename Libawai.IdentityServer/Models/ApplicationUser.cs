using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Libawai.IdentityServer.Models
{
    public class ApplicationUser:IdentityUser<int>
    {
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public DateTime Birth { get; set; }
        public string Address { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
