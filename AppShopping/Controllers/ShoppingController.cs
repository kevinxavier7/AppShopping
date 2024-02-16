using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppShopping.Services;
using System.Collections;
using AppShopping.Models;

namespace AppShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ShoppingService _service;
        public ShoppingController(ShoppingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Getshopping()
        {
            return await _service.GetAll();
          
        }

        [HttpPost]
        public async Task<ActionResult<Shopping>> PostShopping(Shopping shopping)
        {
            return await _service.CreateShopping(shopping);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopping(int id, Shopping shopping)
        {
            return await _service.UpdateShopping(id, shopping);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopping(int id)
        {
            return await _service.DeleteShopping(id);
        }
    }
}
