using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace CollabFrontend.Pages {
    public class LoginModel : PageModel {

        private string API = "http://localhost:62548/api/";

        public User newCreatedUser  { get; set; }

        public void OnGet() {
        }

        private void writeMessage() {
            
        }

        public IActionResult OnPostLogin(String email, String password) {

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            var userR = new UserRequest(){MailAdress = email, Password = password};
            var json = JsonConvert.SerializeObject(userR);

            String response = cli.UploadString(API + "Login","POST", json);
            var ob = JObject.Parse(response);

            HttpContext.Session.SetString("token", "Bearer "+ ob.GetValue("token").ToString());
            HttpContext.Session.SetString("user",ob.GetValue("userID").ToString());

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostRegistrate(String email, String firstname, String lastname, String password) {
            User user = new User();
            user.LastName = "lastname";
            user.FirstName = "firstname";
            user.Password = "neu";
            user.MailAdress = "email@email.com";
            String json = JsonConvert.SerializeObject(user);

            Console.WriteLine(json);

            //String jsonTest = "{ 'lastName': '"+lastname+"', 'firstName': '"+firstname+"', 'mailAdress' : '"+email+"','password' : '"+password+"'}";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            String response = cli.UploadString(API + "User","POST", json);

            return Page();
        }



    }
}
