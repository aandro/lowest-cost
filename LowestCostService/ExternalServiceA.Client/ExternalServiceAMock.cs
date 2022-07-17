using Bogus;
using Refit;

namespace ExternalServiceA.Client
{
    public class ExternalServiceAMock : IExternalServiceA
    {
        private const decimal Min = 10;
        private const decimal Max = 100;

        public Task<ApiResponse<decimal>> GetTotalAsync(TotalRequest request)
        {
            var totalCost = new Faker().Random.Decimal(Min, Max);

            return Task.FromResult(new ApiResponse<decimal>(
                new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK },
                totalCost,
                new RefitSettings()));
        }
    }
}
