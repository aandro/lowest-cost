using LowestCostService.Api.Models;
using LowestCostService.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Swashbuckle.AspNetCore.Annotations;

namespace LowestCostService.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cost")]
    public class CostController : ControllerBase
    {
        private readonly ILogger<CostController> _logger;
        private readonly IWorkerService _worker;

        public CostController(ILogger<CostController> logger, IWorkerService workerService)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(workerService);

            _logger = logger;
            _worker = workerService;
        }

        [HttpGet("lowest")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IApiResponse<CostResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLowestCost(CancellationToken cancellationToken)
        {
            // In production design here:
            // - command is being sent to message broker
            // - separate worker service is performing requests to external APIs and calculations in consumer
            // - result is persisted to db
            // For the brevity, DI "service" is used in technical assignment 

            var result = await _worker.GetLowestCostAsync(cancellationToken);

            return Ok(new CostResponse()
            {
                CompanyName = result.CompanyName,
                TotalPrice = result.TotalPrice
            });
        }
    }
}