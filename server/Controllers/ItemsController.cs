using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            var items = new List<Item>();
            items.Add(new Item(){ Id = 1, Category = "Bebida", Description = "Água", Price = 5 });
            items.Add(new Item(){ Id = 2, Category = "Bebida", Description = "Suco", Price = 7 });
            items.Add(new Item(){ Id = 3, Category = "Bebida", Description = "Refrigerante", Price = 9 });
            items.Add(new Item(){ Id = 4, Category = "Bebida", Description = "Cerveja", Price = 10.50M });

            return Ok(items);
        }
    }
}