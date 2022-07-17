using LowestCostService.Domain.Models;
using Bogus;

namespace LowestCostService.Data
{
    public class PackageRepositoryMock : IPackageRepository
    {
        private const int PackagesCount = 10;

        private static IReadOnlyCollection<Package> _packages =
            new Faker().Make(PackagesCount, () => new Faker<Package>().Generate()).ToList();

        public Task<IReadOnlyCollection<Package>> GetAllAsync(CancellationToken ct)
        {
            return Task.FromResult(_packages);
        }
    }
}
