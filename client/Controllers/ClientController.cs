using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using client.Models;

namespace client.Controllers
{
    public class ClientController : Controller
    {
        private readonly string _url;

        public ClientController()
        {
            _url = "http://localhost:5000/api/";
        }

        public IActionResult Index()
        {
            List<Item> items = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                var getTask = client.GetAsync("inventary");
                getTask.Wait();

                if (getTask.Result.IsSuccessStatusCode)
                {
                    var content = getTask.Result.Content.ReadAsStringAsync();
                    content.Wait();

                    var listItems = JsonConvert.DeserializeObject<List<Item>>(content.Result);

                    if (listItems.Count > 0)
                    {
                        items = listItems;
                    }
                }
            }

            ViewData["items"] = items;
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Item item)
        {
            var errorMessage = string.Empty;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                var postTask = client.PostAsJsonAsync<Item>("inventary",item);
                postTask.Wait();

                if (postTask.Result.IsSuccessStatusCode) 
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    errorMessage = postTask.Result.Content.ReadAsStringAsync().Result;
                }
            }

            ViewData["errorMessage"] = errorMessage;
            return View();
        }
        
        public IActionResult Remove(string itemid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                var deleteTask = client.DeleteAsync(String.Concat("inventary/",itemid));
                deleteTask.Wait();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Notifications()
        {
            List<Notifications> notifications = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                var getTask = client.GetAsync("inventary/notifications");
                getTask.Wait();

                if (getTask.Result.IsSuccessStatusCode)
                {
                    var content = getTask.Result.Content.ReadAsStringAsync();
                    content.Wait();

                    var listNotifications = JsonConvert.DeserializeObject<List<Notifications>>(content.Result);

                    if (listNotifications.Count > 0)
                    {
                        notifications = listNotifications;
                    }
                }
            }

            ViewData["notifications"] = notifications;
            return View();
        }

    }
}
