using ExternalServiceA.Client;
using ExternalServiceB.Client;
using LowestCostService.Data;
using LowestCostService.Domain.Models;
using Refit;
using System.Collections.Concurrent;

namespace LowestCostService.Api.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IExternalServiceA _externalServiceA;
        private readonly IExternalServiceB _externalServiceB;
        private readonly IPackageRepository _packageRepository;

        private readonly ConcurrentDictionary<decimal, TotalCost> _results = new ConcurrentDictionary<decimal, TotalCost>();

        public WorkerService(
            IExternalServiceA externalServiceA,
            IExternalServiceB externalServiceB,
            IPackageRepository packageRepository)
        {
            ArgumentNullException.ThrowIfNull(externalServiceA);
            ArgumentNullException.ThrowIfNull(externalServiceB);
            ArgumentNullException.ThrowIfNull(packageRepository);

            _externalServiceA = externalServiceA;
            _externalServiceB = externalServiceB;
            _packageRepository = packageRepository;
        }

        public async Task<TotalCost> GetLowestCostAsync(CancellationToken ct)
        {
            IReadOnlyCollection<Package> packages = await _packageRepository.GetAllAsync(ct);

            await GetTotalPrice(packages, GetTotalPriceA);
            await GetTotalPrice(packages, GetTotalPriceB);

            var lowestCost = _results[_results.Keys.Min()];
            return lowestCost;
        }

        private async Task GetTotalPrice(
            IReadOnlyCollection<Package> packages,
            Func<IReadOnlyCollection<Package>, Task<TotalCost>> apiCall)
        {
            var result = await apiCall(packages);
            _results[result.TotalPrice] = result;
        }

        private async Task<TotalCost> GetTotalPriceA(IReadOnlyCollection<Package> packages)
        {
            var tasks = packages
               .Select((p) => _externalServiceA.GetTotalAsync(
                   new TotalRequest()
                   {
                       WarehouseAddress = p.SourceAddress,
                       ContactAddress = p.DestinationAddress,
                       PackageDimensions = new[] { p.Width, p.Height, p.Length }
                   }));

            ApiResponse<decimal>[] result = await Task.WhenAll(tasks);

            return new TotalCost()
            {
                CompanyName = nameof(IExternalServiceA),
                TotalPrice = result.Sum(e => e.Content)
            };
        }

        private async Task<TotalCost> GetTotalPriceB(IReadOnlyCollection<Package> packages)
        {
            var tasks = packages
                .Select((p) => _externalServiceB.GetTotalAmountAsync(
                    new TotalAmountRequest()
                    {
                        Consignor = p.SourceAddress,
                        Consignee = p.DestinationAddress,
                        Cartons = new[] { p.Width, p.Height, p.Length } 
                    }));

            ApiResponse<decimal>[] result = await Task.WhenAll(tasks);

            return new TotalCost()
            {
                CompanyName = nameof(IExternalServiceB),
                TotalPrice = result.Sum(e => e.Content)
            };
        }
    }
}
