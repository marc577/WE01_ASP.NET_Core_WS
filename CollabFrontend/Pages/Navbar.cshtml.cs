using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CollabFrontend.Pages
{
    public class NavbarModel : PageModel
    {
        public string Message { get; set; }

        public NavbarModel():base(){
             Message = "Your application description page.";
        }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
}
