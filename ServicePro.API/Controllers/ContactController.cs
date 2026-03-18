using Microsoft.AspNetCore.Mvc;
using ServicePro.Core.DTOs;
using ServicePro.Core.Interfaces;
using ServicePro.Services;

namespace ServicePro.API.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _service;
        private readonly IPdfService _pdfService;

        public ContactController(IContactService service,
                                 IPdfService pdfService)
        {
            _service = service;
            _pdfService = pdfService;
        }
        //jassadhammi
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

        // ✅ CORRECT PDF ENDPOINT
        [HttpGet("download-pdf")]
        public async Task<IActionResult> DownloadPdf()
        {
            var contacts = await _service.GetAllContactsAsync();

            var pdfBytes = _pdfService.GenerateContactPdf(contacts);

            return File(pdfBytes, "application/pdf", "ContactRecords.pdf");
        }
    }
}