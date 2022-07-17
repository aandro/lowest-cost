using LowestCostService.Domain.Models;

namespace LowestCostService.Api.Services
{
    public interface IWorkerService
    {
        Task<TotalCost> GetLowestCostAsync(CancellationToken ct);
    }
}
