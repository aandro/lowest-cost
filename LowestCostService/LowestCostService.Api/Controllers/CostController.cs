using LowestCostService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Swashbuckle.AspNetCore.Annotations;

namespace LowestCostService.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/cost")]
    public class CostController : ControllerBase
    {
        private readonly ILogger<CostController> _logger;

        public CostController(ILogger<CostController> logger)
        {
            _logger = logger;
        }

        [HttpGet("lowest")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IApiResponse<CostResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLowestCost(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}