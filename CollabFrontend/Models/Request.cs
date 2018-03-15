using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace RazorPagesMovie.Models
{
    public class Request {

        private string API = "http://localhost:62548/api/";
        public String RequestWithoutAuthorization(String json, String requestAdress, String requestMethod) {
            var client = new WebClient();
            var cli = new WebClient();
            cli.Headers[HttpRequestHeader.ContentType] = "application/json";

            String response = cli.UploadString(API + requestAdress,requestMethod, json);
            return response;
        }

        public String RequestDownloadWithAuthorization(String requestAdress, String bearer) {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            client.Headers.Add("Authorization", bearer);
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            String response = client.DownloadString(API+requestAdress);
            return response;
        }

        public String RequestUploadWithAuthorization(String json, String requestAdress, String requestMethod, String bearer) {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            client.Headers.Add("Authorization", bearer);
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            String response = client.UploadString(API + requestAdress,requestMethod, json);
            return response;
        }
    }
}