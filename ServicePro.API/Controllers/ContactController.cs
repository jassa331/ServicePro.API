using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicePro.Core.DTOs;
using ServicePro.Core.Interfaces;

namespace ServicePro.API.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactDto dto)
        {
            await _service.CreateContactAsync(dto);
            return Ok(new { message = "Contact submitted successfully" });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllContactsAsync();
            return Ok(data);
        }

    }
}
