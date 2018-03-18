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

namespace CollabFrontend.Pages
{
    public class ContactModel : PageModel
    {
        
        [BindProperty]
        public User user { get; set; }

        public void OnGet()
        {
            var jwt = HttpContext.Session.GetString("token");
            if(jwt != null){
                getData(jwt);
            }else{
                Response.Redirect("/Login");
            }
        }

        private void getData(String bearer) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null) {
                Request request = new Request();
                
                try{
                    String response = request.RequestDownloadWithAuthorization("User/"+ userID,bearer);
                    var obj = JObject.Parse(response);
                    user = obj.ToObject<User>();
                }catch(WebException e){
                    var response = e.Response as HttpWebResponse;
                    if (response != null)
                    {
                        if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                            Response.Redirect("Login");
                        }
                    }
                }
            }else{
                Response.Redirect("Login");
            }
        }

        public IActionResult OnPostBack() {
            return RedirectToPage("Index");
        }

        public IActionResult OnPostDiscard() {
            return RedirectToPage("Contact");
        }

        public IActionResult OnPostChangePassword(String password) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null) {
                Request request = new Request();
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String json = "{'newPassword':'"+password+"'}";
                    String response = request.RequestUploadWithAuthorization(json,"User/"+userID,"PATCH",jwt);
                }catch(WebException e){
                    var response = e.Response as HttpWebResponse;
                    if (response != null) {
                        if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                            Response.Redirect("Login");
                        }
                    }
                }
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPostSave(String email) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null) {
                Request request = new Request();
                
                
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    user.MailAdress = email;
                    
                    String json = JsonConvert.SerializeObject(user);
                    String response = request.RequestUploadWithAuthorization(json,"User/"+userID,"PUT",jwt);
                }catch(WebException e){
                    var response = e.Response as HttpWebResponse;
                    if (response != null) {
                        if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                            Response.Redirect("Login");
                        }
                    }
                }
            }
            return RedirectToPage("Contact");
        }
    }
}
