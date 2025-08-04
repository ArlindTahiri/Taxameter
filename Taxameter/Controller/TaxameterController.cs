using Microsoft.AspNetCore.Mvc;
using Taxameter.Servies;

namespace Taxameter.Controller
{
    [ApiController]
    [Route("api/taxameter")]
    public class TaxameterController : ControllerBase
    {
        private readonly TaxameterService _service;

        public TaxameterController(TaxameterService service)
        {
            _service = service;
        }

        [HttpPost("start")]
        public IActionResult Start()
        {
            _service.Start();
            return Ok();
        }

        [HttpPost("stop")]
        public IActionResult Stop()
        {
            _service.Stop();
            return Ok();
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _service.Reset();
            return Ok();
        }
    }
}
