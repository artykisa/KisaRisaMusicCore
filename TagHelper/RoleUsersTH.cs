using System.Collections.Generic;
using System.Threading.Tasks;
using KisaMusic.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KisaRisaMusicCore.TagHelper
{
    [HtmlTargetElement("td", Attributes = "i-role")]
        public class RoleUsersTH : Microsoft.AspNetCore.Razor.TagHelpers.TagHelper
        {
            private UserManager<ApplicationUser> userManager;
            private RoleManager<IdentityRole> roleManager;
 
            public RoleUsersTH(UserManager<ApplicationUser> usermgr, RoleManager<IdentityRole> rolemgr)
            {
                userManager = usermgr;
                roleManager = rolemgr;
            }
 
            [HtmlAttributeName("i-role")]
            public string Role { get; set; }
 
            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                List<string> names = new List<string>();
                IdentityRole role = await roleManager.FindByIdAsync(Role);
                if (role != null)
                {
                    foreach (var user in userManager.Users)
                    {
                        if (user != null && await userManager.IsInRoleAsync(user.ToString(), role.Name))
                            names.Add(user.UserName);
                    }
                }
                output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
            }
    }
}