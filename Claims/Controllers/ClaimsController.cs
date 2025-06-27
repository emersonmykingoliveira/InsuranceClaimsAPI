using Claims.Models.Claims;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    /// <summary>
    /// Controller to manage claim operations such as retrieving, creating, and deleting claims.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsController"/> class.
        /// </summary>
        /// <param name="claimService">Service for handling claim business logic.</param>
        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        /// <summary>
        /// Retrieves all claims asynchronously.
        /// </summary>
        /// <returns>
        /// A list of <see cref="ClaimResponse"/> representing all claims.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimResponse>>> GetAsync()
        {
            var claims = await _claimService.GetAllClaimsAsync();
            return Ok(claims);
        }

        /// <summary>
        /// Creates a new claim with the provided request data.
        /// </summary>
        /// <param name="claimRequest">The claim request data.</param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing the newly created <see cref="ClaimResponse"/> on success,
        /// or a BadRequest with errors on failure.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> CreateAsync(ClaimRequest claimRequest)
        {
            var result = await _claimService.CreateClaimAsync(claimRequest);
            return result?.IsSuccess ?? false ? Ok(result?.NewClaim) : BadRequest(result?.Errors);
        }

        /// <summary>
        /// Deletes a claim by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the claim to delete.</param>
        /// <returns>
        /// <see cref="NoContentResult"/> on successful deletion,
        /// or <see cref="NotFoundResult"/> if the claim does not exist.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            bool deleted = await _claimService.DeleteClaimAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Retrieves a claim by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the claim to retrieve.</param>
        /// <returns>
        /// The <see cref="ClaimResponse"/> representing the requested claim if found;
        /// otherwise, <c>null</c>.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimResponse?>> GetByIdAsync(string id)
        {
            return await _claimService.GetClaimByIdAsync(id);
        }
    }
}
