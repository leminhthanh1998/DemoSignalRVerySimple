using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoSignalR.Hubs;
using DemoSignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DemoSignalR.Controllers
{
    public class PostController : Controller
    {
        private readonly IHubContext<PostHub> _hubContext;

        public PostController(IHubContext<PostHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PostModel model)
        {
            if (ModelState.IsValid)
            {
                var id = model.ID;
                var name = model.Name;
                var number = model.Number;

                var htmlData = CreateHtml(id, name, number);
                if(model.Method==1)
                await _hubContext.Clients.All.SendAsync("ReceiveNotify", htmlData);
                if (model.Method == 2)
                    await _hubContext.Clients.All.SendAsync("EditNotify",id, htmlData);
                if (model.Method == 3)
                    await _hubContext.Clients.All.SendAsync("DeleteNotify", id);

                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");
        }

        private string CreateHtml(int id, string name, int number)
        {
            string data = $"<tr id=\"r{id}\">\r\n " +
                    $" <th scope=\"row\">{id}</th>\r\n " +
                    $"<td>{name}</td>\r\n" +
                     $"<td>{number}</td>\r\n"+
                    $"</tr>";
            return data;
        }
    }
}