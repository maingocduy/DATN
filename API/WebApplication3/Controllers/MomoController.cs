using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTOs.Momo;
using WebApplication3.Service.MomoService;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        private readonly IMomoService _momoService;

        public MomoController(IMomoService momoService)
        {
            _momoService = momoService;
        }
        [HttpPost("{nameProject}")]
        public async Task<IActionResult> CreatePaymentUrl(string nameProject, OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(nameProject,model);
            return Ok(response);
        }

        [HttpGet("test")]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return Ok(response);
        }
    }
}
