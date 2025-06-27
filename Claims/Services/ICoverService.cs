using Claims.Models;
using Claims.Models.ComputePremium;
using Claims.Models.Cover;

namespace Claims.Services
{
    public interface ICoverService
    {
        ComputePremiumResult ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
        Task<IEnumerable<CoverResponse>> GetAllCoversAsync();
        Task<CoverResponse?> GetCoverByIdAsync(string id);
        Task<CoverResult?> CreateCoverAsync(CoverRequest request);
        Task<bool> DeleteCoverAsync(string id);
    }
}
