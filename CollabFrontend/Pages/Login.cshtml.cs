using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace CollabFrontend.Pages {
    public class LoginModel : PageModel {

        public void OnGet() {
        }

        private void writeMessage() {
            
        }

        public IActionResult OnPostLogin() {
            // TODO: Login Webservice aufrufen
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostRegistrate() {
            // TODO: Registrate Webservice aufrufen
            return Page();
        }



    }
}
