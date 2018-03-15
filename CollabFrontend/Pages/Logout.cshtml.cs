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
    public class LogoutModel : PageModel {

        public void OnGet() {
            // TODO hier nochmal schauen warum es nicht geht..
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("user");
            Response.Redirect("/Login");
        }


    }
}
