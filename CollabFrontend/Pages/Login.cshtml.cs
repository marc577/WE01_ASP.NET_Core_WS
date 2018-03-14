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

        private string API = "http://localhost:62548/api/";

        public User newCreatedUser  { get; set; }

        public void OnGet() {
        }

        private void writeMessage() {
            
        }

        public IActionResult OnPostLogin(String email, String password) {
            // TODO: Login Webservice aufrufen
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostRegistrate(String email, String firstname, String lastname, String password) {
            User user = new User();
            user.LastName = "lastname";
            user.FirstName = "firstname";
            user.Password = "neu";
            user.MailAdress = "email@email.com";
            
            String json = JsonConvert.SerializeObject(user);

            String jsonTest = "{ 'lastName': '"+lastname+"', 'firstName': '"+firstname+"', 'mailAdress' : '"+email+"','password' : '"+password+"'}";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            String response = cli.UploadString(API + "User","POST", jsonTest);

            return Page();
        }



    }
}
