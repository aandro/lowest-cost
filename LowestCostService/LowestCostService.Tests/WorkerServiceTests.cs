using Bogus;
using ExternalServiceA.Client;
using ExternalServiceB.Client;
using FluentAssertions;
using LowestCostService.Api.Services;
using LowestCostService.Data;
using LowestCostService.Domain.Models;
using Moq;
using Refit;

namespace LowestCostService.Tests
{
    public class WorkerServiceTests
    {
        private const int TotalPackagesCount = 3;

        private readonly Mock<IExternalServiceA> _serviceA;
        private readonly Mock<IExternalServiceB> _serviceB;
        private readonly Mock<IPackageRepository> _repository;

        public WorkerServiceTests()
        {
            _serviceA = new Mock<IExternalServiceA>();
            _serviceB = new Mock<IExternalServiceB>();
            _repository = new Mock<IPackageRepository>();

            _repository
                .Setup(m => m.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Faker().Make(TotalPackagesCount, () => new Faker<Package>().Generate()).ToList());
        }

        [Fact]
        public async Task GetLowestCostAsync_ShouldReturnLowestCostCompanyAndValue()
        {
            // Arrange
            var lowestPricePerUnit = 10;

            _serviceA
               .Setup(m => m.GetTotalAsync(It.IsAny<TotalRequest>()))
               .ReturnsAsync(new ApiResponse<decimal>(new HttpResponseMessage(), lowestPricePerUnit, new RefitSettings()));
            _serviceB
                .Setup(m => m.GetTotalAmountAsync(It.IsAny<TotalAmountRequest>()))
                .ReturnsAsync(new ApiResponse<decimal>(new HttpResponseMessage(), 100, new RefitSettings()));

            var service = new WorkerService(_serviceA.Object, _serviceB.Object, _repository.Object);

            // Act 
            var result = await service.GetLowestCostAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CompanyName.Should().Be(nameof(IExternalServiceA));
            result.TotalPrice.Should().Be(lowestPricePerUnit * TotalPackagesCount);
        }
    }
}