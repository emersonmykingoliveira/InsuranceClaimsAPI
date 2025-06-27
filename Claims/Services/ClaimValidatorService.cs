using Claims.DbContexts;
using Claims.Models.Claims;
using Claims.Models.Cover;
using Microsoft.EntityFrameworkCore;

namespace Claims.Services
{
    public class ClaimValidatorService
    {
        private readonly ClaimsContext _claimsContext;
        private const decimal _damageCostLimit = 100_000;

        public ClaimValidatorService(ClaimsContext claimsContext)
        {
            _claimsContext = claimsContext;
        }
        public async Task<List<string>> ValidateClaimRequestAsync(ClaimRequest claimRequest)
        {
            List<string> errors = new List<string>();

            var cover = await _claimsContext.Covers.FirstOrDefaultAsync(s => s.Id == claimRequest.CoverId);

            if (cover is null)
            {
                errors.Add("Related cover does not exist");
            }
            else
            {
                if (DamageCostExceedLimit(claimRequest.DamageCost))
                    errors.Add($"DamageCost cannot exceed {_damageCostLimit}");

                if (IsCreatedDateOutsideCover(claimRequest.Created, cover))
                    errors.Add("Created date must be within the period of the related Cover");
            }

            return errors;
        }

        private static bool IsCreatedDateOutsideCover(DateTime created, CoverResponse cover)
        {
            return (created.Date < cover?.StartDate.Date || created.Date > cover?.EndDate.Date);
        }

        private static bool DamageCostExceedLimit(decimal damageCost)
        {
            return damageCost > _damageCostLimit;
        }
    }
}
