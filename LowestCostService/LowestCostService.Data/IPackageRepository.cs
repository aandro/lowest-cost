using LowestCostService.Domain.Models;

namespace LowestCostService.Data
{
    public interface IPackageRepository
    {
        Task<IReadOnlyCollection<Package>> GetAllAsync(CancellationToken ct);
    }
}
