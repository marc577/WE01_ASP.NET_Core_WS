﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace CollabFrontend.Pages
{
    public class IndexModel : PageModel
    {
        private string API = "http://localhost:62548/api/";

        [BindProperty]
        public TodoItem newListItem { get; set; }

        public List<TodoList> todolistList  { get; set; }

        public List<TodoItem> todoItemList {get;set;}
        public String message  { get; set; }

        public IndexModel() : base(){
            todolistList = new List<TodoList>();
            todoItemList = new List<TodoItem>();
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
                var client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                client.Headers.Add("Authorization", bearer);
                try{
                    var response = client.DownloadString("http://localhost:62548/api/TodoCollab/"+ userID);
                    var releases = JArray.Parse(response);
                    todolistList = releases.ToObject<List<TodoList>>();
                    todolistList.Reverse();
                    todoItemList = releases.ToObject<List<TodoItem>>();
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
            String jsonTest = "{ 'listID': '"+listId+"',";
            if (workerId != "0") {
                jsonTest = jsonTest + "'workerID': '"+workerId+"',";
            }
            if(todoDate != null) {
                jsonTest = jsonTest +"'until': '"+todoDate+"',";
            }
            jsonTest +=  "'name': '"+todoText+"' }";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = cli.UploadString(API + "TodoItem","POST", jsonTest);
            
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostCheck (String todoId, String todoName, String todoDate, String workerId) {
            String jsonTest = "{";
            if (workerId != "0") {
                jsonTest = jsonTest + "'workerID': '"+workerId+"',";
            }
            if(todoDate != null) {
                jsonTest = jsonTest +"'until': '"+todoDate+"',";
            }
            jsonTest +=  "'name': '"+todoName+"','isComplete': true}";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = cli.UploadString(API + "TodoItem/"+todoId,"PUT", jsonTest);

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostUncheck (String todoId, String todoName, String todoDate, String workerId) {
            String jsonTest = "{";
            if (workerId != "0") {
                jsonTest = jsonTest + "'workerID': '"+workerId+"',";
            }
            if(todoDate != null) {
                jsonTest = jsonTest +"'until': '"+todoDate+"',";
            }
            jsonTest +=  "'name': '"+todoName+"','isComplete': false}";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = cli.UploadString(API + "TodoItem/"+todoId,"PUT", jsonTest);

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostDelete (String todoId, String todoName) {
            String jsonTest = "{'name': '"+todoName+"','isComplete': false }";

            var cli = new WebClient();
            string response = cli.UploadString(API + "TodoItem/"+todoId,"DELETE","");

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
            String json = "{ 'name': '"+listName+"', 'ownerID': '5c7ad24f-528c-4e97-bea7-4540ae137b91' }";

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = cli.UploadString(API + "TodoList","POST", json);

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostDeleteList (String listId) {
            var cli = new WebClient();
            string response = cli.UploadString(API + "TodoList/"+listId,"DELETE","");

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostAddCollab (String listId, String email) {
        
            return RedirectToPage("/Index");
        }
    }
}
