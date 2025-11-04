using ILOS.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ILOS.Application.DTOs;

namespace ILOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly IRateService _rateService;

        public RatesController(IRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRates()
        {

            var rates = await _rateService.GetRateConfigurationsAsync();


            return Ok(rates);
        }

        [HttpPost("simulate")]
        public async Task<ActionResult> SimulateCost([FromBody] SimulationRequestDto request)
        {
            var cost = await _rateService.CalculateCostAsync(request);
            if (cost == -1)
            {

                return NotFound(new { error = $"Rate name '{request.RateName}' not found." });
            }


            return Ok(new { totalCost = cost });
        }
    }
}