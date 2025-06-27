using Claims.Models.Claims;

namespace Claims.Services
{
    public interface IClaimService
    {
        Task<ClaimResult?> CreateClaimAsync(ClaimRequest claimRequest);
        Task<IEnumerable<ClaimResponse>> GetAllClaimsAsync();
        Task<ClaimResponse?> GetClaimByIdAsync(string id);
        Task<bool> DeleteClaimAsync(string id);
    }
}
