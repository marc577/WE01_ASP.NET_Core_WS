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
    public class IndexModel : PageModel
    {
        private string API = "http://localhost:62548/api/";

        [BindProperty]
        public TodoItem newListItem { get; set; }

        public List<TodoList> todolistList  { get; set; }
        public String message  { get; set; }

        public IndexModel() : base(){
            todolistList = new List<TodoList>();
        }
        public void OnGet() {
            var jwt = HttpContext.Session.GetString("token");
            if(jwt != null){
                writeMessage(jwt);
            }else{
                Response.Redirect("/Login");
            }
        }

        private void writeMessage(string bearer) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                Request request = new Request();
                
                try{
                    String response = request.RequestDownloadWithAuthorization("TodoCollab/"+ userID,bearer);
                    var releases = JArray.Parse(response);
                    todolistList = releases.ToObject<List<TodoList>>();
                    todolistList.Reverse();
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

        public IActionResult OnPostAddItem(String listId, String todoText, String todoDate, String workerId) {
            
            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                Request request = new Request();
                DateTime parsed;
                try {
                    parsed = DateTime.ParseExact(todoDate, "yyyy-MM-ddTHH:mm", System.Globalization.CultureInfo.InvariantCulture);
                } catch(System.FormatException) {
                    parsed = new DateTime();
                } 
                TodoItem todoItem = new TodoItem{ListID=new Guid(listId),Name=todoText,Until=parsed,WorkerID=new Guid(workerId)};
                String json = JsonConvert.SerializeObject(todoItem);
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String response = request.RequestUploadWithAuthorization(json,"TodoItem","POST",jwt);
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
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostCheck (String todoId, String todoName, String todoDate, String workerId) {
            
            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                DateTime parsed;
                Request request = new Request();
                try {
                    parsed = DateTime.ParseExact(todoDate, "yyyy-MM-ddTHH:mm", System.Globalization.CultureInfo.InvariantCulture);
                } catch(Exception) {
                    parsed = new DateTime();
                } 
                TodoItem todoItem = new TodoItem(){Name=todoName,Id=new Guid(todoId),Until=parsed,WorkerID=new Guid(workerId),IsComplete=true};
            
                String json = JsonConvert.SerializeObject(todoItem);
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String response = request.RequestUploadWithAuthorization(json,"TodoItem/"+todoId,"PUT",jwt);
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

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostUncheck (String todoId, String todoName, String todoDate, String workerId) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                DateTime parsed;
                Request request = new Request();
                try {
                    parsed = DateTime.ParseExact(todoDate, "yyyy-MM-ddTHH:mm", System.Globalization.CultureInfo.InvariantCulture);
                } catch(Exception) {
                    parsed = new DateTime();
                } 
                if (workerId == null) {
                    workerId = "00000000-0000-0000-0000-000000000000";
                }
                TodoItem todoItem = new TodoItem(){Name=todoName,Id=new Guid(todoId),Until=parsed,WorkerID=new Guid(workerId),IsComplete=false};
            
                String json = JsonConvert.SerializeObject(todoItem);
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String response = request.RequestUploadWithAuthorization(json,"TodoItem/"+todoId,"PUT",jwt);
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

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostDelete (String todoId, String todoName) {

            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                Request request = new Request();
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String response = request.RequestUploadWithAuthorization("","TodoItem/"+todoId,"DELETE",jwt);
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

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostEditItem (String todoId, String todoName, String todoDate, String workerId) {
            String jsonTest = "{ ";
            jsonTest = jsonTest + "'workerID': '"+workerId+"',";
            if(todoDate != null) {
                jsonTest = jsonTest +"'until': '"+todoDate+"',";
            }
            jsonTest +=  "'name': '"+todoName+"' }";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = cli.UploadString(API + "TodoItem/"+todoId,"PUT", jsonTest);

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostCreateList (String listName) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                TodoList todoList = new TodoList{Name=listName, OwnerID=new Guid(userID)};
                String json = JsonConvert.SerializeObject(todoList);
                Request request = new Request();
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String response = request.RequestUploadWithAuthorization(json,"TodoList","POST",jwt);
                    
                } catch(WebException e) {
                    var response = e.Response as HttpWebResponse;
                    if (response != null) {
                        if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                            Response.Redirect("Login");
                        }
                    }
                }
            } else {
                RedirectToPage("/Login");
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostDeleteList (String listId) {
            string userID = HttpContext.Session.GetString("user");
            if(userID != null){
                Request request = new Request();
                try{
                    var jwt =  HttpContext.Session.GetString("token");
                    String response = request.RequestUploadWithAuthorization("","TodoList/"+listId,"Delete",jwt);
                    
                } catch(WebException e) {
                    var response = e.Response as HttpWebResponse;
                    if (response != null) {
                        if(response.StatusCode.Equals(HttpStatusCode.Unauthorized)){
                            Response.Redirect("Login");
                        }
                    }
                }
            } else {
                RedirectToPage("/Login");
            }


            return RedirectToPage("/Index");
        }

        public IActionResult OnPostAddCollab (String listId, String email) {
        
            return RedirectToPage("/Index");
        }
    }
}
