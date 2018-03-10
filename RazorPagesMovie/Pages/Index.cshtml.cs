using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages{
    public class IndexModel : PageModel {

        private string API = "http://localhost:62548/api/";

        public List<TodoList> todolistList  { get; set; }

        public List<TodoItem> todoItemList {get;set;}
        public String message  { get; set; }
        public void OnGet() {
            writeMessage();
        }

        private void writeMessage() {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            var response = client.DownloadString("http://localhost:62548/api/TodoCollab/5c7ad24f-528c-4e97-bea7-4540ae137b91");
            //var response = client.DownloadString("http://localhost:62548/api/TodoList");
            var releases = JArray.Parse(response);
            todolistList = releases.ToObject<List<TodoList>>();
            Console.Write(todolistList);
            //response = client.DownloadString("http://localhost:62548/api/TodoItem");
            //releases = JArray.Parse(response);
            //todoItemList = releases.ToObject<List<TodoItem>>();
    
        }

        public IActionResult OnPostAddItem(String listID, String todoText, String todoDate) {
            TodoItem item = new TodoItem();
            item.ListID = new Guid(listID);
            item.Name = todoText;
            String json = new JObject(item).ToString();

            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";
            string response = cli.UploadString(API + "TodoItem", json);
            
            //var client = new WebClient();
            //client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            //client.Headers.Add("content-type","application/json");
            //client.UploadString(API + "TodoItem", json);
            // TODO: Login Webservice aufrufen
            return RedirectToPage("/Index");
        }
    }
}
