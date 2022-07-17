using ExternalServiceA.Client;
using ExternalServiceB.Client;
using LowestCostService.Data;
using LowestCostService.Domain.Models;
using System.Collections.Concurrent;

namespace LowestCostService.Api.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly object _lock = new object();

        private readonly IExternalServiceA _externalServiceA;
        private readonly IExternalServiceB _externalServiceB;
        private readonly IPackageRepository _packageRepository;

        private readonly ConcurrentDictionary<string, decimal> _results = new ();
        private TotalCost _lowestCost;

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

            Func<Package, Task> func = async (p) =>
            {
                decimal lowestCost = int.MaxValue;
                ConcurrentDictionary<decimal, TotalCost> resultsInverseMap = new();

                lowestCost = await GetTotalForPackage(p, lowestCost, resultsInverseMap, GetTotalPriceA);
                lowestCost = await GetTotalForPackage(p, lowestCost, resultsInverseMap, GetTotalPriceB);

                _lowestCost = resultsInverseMap[lowestCost];
            };

            var tasks = packages.Select(p => func(p));
            await Task.WhenAll(tasks);

            return _lowestCost;
        }

        private async Task<decimal> GetTotalForPackage(
            Package p,
            decimal currentLowestCost,
            IDictionary<decimal, TotalCost> resultsInverseMap,
            Func<Package, Task<TotalCost>> f)
        {
            var result = await f(p);

            lock (_lock)
            {
                _results.AddOrUpdate(result.CompanyName, result.TotalPrice, (k, v) => v += result.TotalPrice);
            }

            var currentValue = _results[result.CompanyName];
            resultsInverseMap.Add(currentValue, new TotalCost() { CompanyName = result.CompanyName, TotalPrice = currentValue });

            return Math.Min(currentLowestCost, currentValue);
        }

        private async Task<TotalCost> GetTotalPriceA(Package p)
        {
            var result = await _externalServiceA.GetTotalAsync(
                new TotalRequest()
                {
                    WarehouseAddress = p.SourceAddress,
                    ContactAddress = p.DestinationAddress,
                    PackageDimensions = new[] { p.Width, p.Height, p.Length }
                });

            return new TotalCost()
            {
                CompanyName = nameof(IExternalServiceA),
                TotalPrice = result.Content
            };
        }

        private async Task<TotalCost> GetTotalPriceB(Package p)
        {
            var result = await _externalServiceB.GetTotalAmountAsync(
                    new TotalAmountRequest()
                    {
                        Consignor = p.SourceAddress,
                        Consignee = p.DestinationAddress,
                        Cartons = new[] { p.Width, p.Height, p.Length } 
                    });

            return new TotalCost()
            {
                CompanyName = nameof(IExternalServiceB),
                TotalPrice = result.Content
            };
        }
    }
}
