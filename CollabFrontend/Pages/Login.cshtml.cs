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

        [BindProperty]
        public User newCreatedUser  { get; set; }

        public void OnGet() {
            
        }

        public IActionResult OnPostLogin(String email, String password) {

            var userR = new UserRequest(){MailAdress = email, Password = password};
            var json = JsonConvert.SerializeObject(userR);

            Request request = new Request();
            
            try {
                String response = request.RequestWithoutAuthorization(json,"Login","POST");
                var ob = JObject.Parse(response);
                HttpContext.Session.SetString("token", "Bearer "+ ob.GetValue("token").ToString());
                HttpContext.Session.SetString("user",ob.GetValue("userID").ToString());
            } catch (WebException e) {
                var response = e.Response as HttpWebResponse;
                if (response != null) {
                    if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                        Response.Redirect("Login");
                    }
                    if(response.StatusCode.Equals(HttpStatusCode.InternalServerError)) {
                        Response.Redirect("Login");
                    }
                }
            }
            
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostRegistrate() {
            var json = JsonConvert.SerializeObject(newCreatedUser);

            Request request = new Request();

            try {
                String response = request.RequestWithoutAuthorization(json,"User","POST");
                
                return OnPostLogin(newCreatedUser.MailAdress, newCreatedUser.Password);
            } catch (WebException e) {
                var response = e.Response as HttpWebResponse;
                if (response != null) {
                    if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                        Response.Redirect("Login");
                    }
                    if(response.StatusCode.Equals(HttpStatusCode.InternalServerError)) {
                        Response.Redirect("Login");
                    }
                }
            }

            return Page();
        }



    }
}
