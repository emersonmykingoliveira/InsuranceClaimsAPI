using Claims.DbContexts;
using Claims.Models;
using Claims.Models.ComputePremium;
using Claims.Models.Cover;
using Microsoft.EntityFrameworkCore;

namespace Claims.Services
{
    public class CoverService : ICoverService
    {
        private readonly ClaimsContext _claimsContext;
        private readonly IAuditerService _auditerService;
        private readonly CoverValidatorService _validator;

        public CoverService(ClaimsContext claimsContext, IAuditerService auditerService, CoverValidatorService validator)
        {
            _claimsContext = claimsContext;
            _auditerService = auditerService;
            _validator = validator;
        }

        public ComputePremiumResult ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            List<string> errors = _validator.ValidateRequestedDates(startDate, endDate);

            if (errors?.Count > 0)
                return new ComputePremiumResult { Errors = errors };

            return ComputePremiumHandler.ComputePremiumByDate(startDate, endDate, coverType);
        }

        public async Task<IEnumerable<CoverResponse>> GetAllCoversAsync()
        {
            return await _claimsContext.Covers.ToListAsync();
        }

        public async Task<CoverResponse?> GetCoverByIdAsync(string id)
        {
            return await _claimsContext.Covers.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CoverResult?> CreateCoverAsync(CoverRequest request)
        {
            CoverResult result = BuildCoverIfValid(request);

            if (result.IsSuccess && result.NewCover is not null)
            {
                    _claimsContext.Covers.Add(result.NewCover);
                    await _claimsContext.SaveChangesAsync();
                    _auditerService.AuditCover(result.NewCover.Id, "POST");
            }

            return result;
        }

        public async Task<bool> DeleteCoverAsync(string id)
        {
            int affectedRows = 0;
            var cover = await _claimsContext.Covers.Where(cover => cover.Id == id).SingleOrDefaultAsync();

            if (cover is not null)
            {
                _claimsContext.Covers.Remove(cover);
                affectedRows = await _claimsContext.SaveChangesAsync();

                if(affectedRows > 0)
                {
                    _auditerService.AuditCover(id, "DELETE");
                }
            }

            return affectedRows > 0;
        }

        private CoverResult BuildCoverIfValid(CoverRequest coverRequest)
        {
            CoverResult coverValidateResult = new CoverResult();
            var errors = _validator.ValidateRequest(coverRequest);

            if (errors?.Count > 0)
                coverValidateResult.Errors = errors;
            else
                coverValidateResult.NewCover = MapToCoverResponse(coverRequest);

            return coverValidateResult;
        }

        private CoverResponse? MapToCoverResponse(CoverRequest coverRequest)
        {
            var premium = ComputePremiumHandler.ComputePremiumByDate(coverRequest.StartDate, coverRequest.EndDate, coverRequest.Type);

            return new CoverResponse
            {
                Id = Guid.NewGuid().ToString(),
                Type = coverRequest.Type,
                EndDate = coverRequest.EndDate.Date,
                StartDate = coverRequest.StartDate.Date,
                Premium = premium?.ComputePremium?.Total ?? 0m
            };
        }
    }
}
