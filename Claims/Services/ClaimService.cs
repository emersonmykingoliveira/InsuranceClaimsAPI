using Claims.DbContexts;
using Claims.Models.Claims;

namespace Claims.Services
{
    public class ClaimService : IClaimService
    {
        private readonly ClaimsContext _claimsContext;
        private readonly IAuditerService _auditerService;
        private readonly ClaimValidatorService _validator;

        public ClaimService(ClaimsContext claimsContext, IAuditerService auditerService, ClaimValidatorService validator)
        {
            _claimsContext = claimsContext;
            _auditerService = auditerService;
            _validator = validator;
        }

        public async Task<ClaimResult?> CreateClaimAsync(ClaimRequest claimRequest)
        {
            var result = await BuildClaimIfValid(claimRequest);

            if (result.IsSuccess && result.NewClaim is not null)
            {
                    await _claimsContext.AddItemAsync(result.NewClaim);
                    _auditerService.AuditClaim(result.NewClaim.Id, "POST");
            }

            return result;
        }

        public async Task<IEnumerable<ClaimResponse>> GetAllClaimsAsync()
        {
            return await _claimsContext.GetClaimsAsync();
        }

        public async Task<ClaimResponse?> GetClaimByIdAsync(string id)
        {
            return await _claimsContext.GetClaimAsync(id);
        }

        public async Task<bool> DeleteClaimAsync(string id)
        {
            var deleted = await _claimsContext.DeleteItemAsync(id);
            if (deleted)
            {
                _auditerService.AuditClaim(id, "DELETE");
            }
            return deleted;
        }

        private async Task<ClaimResult> BuildClaimIfValid(ClaimRequest claimRequest)
        {
            ClaimResult claimValidateResult = new ClaimResult();
            var errors = await _validator.ValidateClaimRequestAsync(claimRequest);

            if (errors?.Count > 0)
                claimValidateResult.Errors.AddRange(errors);
            else
                claimValidateResult.NewClaim = MapToClaimResponse(claimRequest);

            return claimValidateResult;
        }

        private ClaimResponse MapToClaimResponse(ClaimRequest claimRequest)
        {
            return new ClaimResponse
            {
                Id = Guid.NewGuid().ToString(),
                CoverId = claimRequest.CoverId,
                Created = claimRequest.Created.Date,
                DamageCost = claimRequest.DamageCost,
                Name = claimRequest.Name,
                Type = claimRequest.Type
            };
        }
    }
}
