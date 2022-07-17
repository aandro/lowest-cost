using LowestCostService.Domain.Models;

namespace LowestCostService.Data
{
    public class PackageRepositoryMock : IPackageRepository
    {
        // TODO: use Bogus or other random data generator
        private static IReadOnlyCollection<Package> _packages = new List<Package>()
        {
            new Package()
            {
                Width = 10,
                Length = 10,
                Height = 10,
            },
            new Package()
            {
                Width = 100,
                Length = 100,
                Height = 100,
            }
        };

        public Task<IReadOnlyCollection<Package>> GetAllAsync(CancellationToken ct)
        {
            return Task.FromResult(_packages);
        }
    }
}
