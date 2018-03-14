using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RazorPagesMovie.Models;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Http;

namespace RazorPagesMovie.Pages {
    public class LoginModel : PageModel {

        public string Message { get; set; }

        [BindProperty]
        public User user { get; set; }

        public void OnGet() {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            var response = client.UploadString("http://localhost:62548/api/login", "POST","{'MailAdress': 'sys@admin.de', 'Passsword': '123456'}");
            Console.WriteLine("Response"+response);
        }

        private void writeMessage() {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            var response = client.DownloadString("http://localhost:62548/api/user");
            var releases = JArray.Parse(response);
            var userList = releases.ToObject<List<User>>();
            foreach (User u in userList){
                System.Console.Write("{0} ", u.Id);
                Message += u.Id + " ";
            }
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
