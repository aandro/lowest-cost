using Refit;

namespace ExternalServiceA.Client
{
    public interface IExternalServiceA
    {
        [Get("/api/v1/packages/total")]
        Task<ApiResponse<decimal>> GetTotalAsync(TotalRequest request);
    }
}