using Refit;

namespace ExternalServiceB.Client
{
    public interface IExternalServiceB
    {
        [Get("/api/v1/cartons/amount")]
        Task<ApiResponse<decimal>> GetTotalAmountAsync(TotalAmountRequest request);
    }
}