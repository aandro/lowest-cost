using Bogus;
using Refit;

namespace ExternalServiceB.Client
{
    public class ExternalServiceBMock : IExternalServiceB
    {
        private const decimal Min = 50;
        private const decimal Max = 150;

        public Task<ApiResponse<decimal>> GetTotalAmountAsync(TotalAmountRequest request)
        {
            var totalCost = new Faker().Random.Decimal(Min, Max);

            return Task.FromResult(new ApiResponse<decimal>(
                new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK },
                totalCost,
                new RefitSettings()));
        }
    }
}
