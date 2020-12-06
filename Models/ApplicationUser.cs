using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace KisaRisaMusicCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}
