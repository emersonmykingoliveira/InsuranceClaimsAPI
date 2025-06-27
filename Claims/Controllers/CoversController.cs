using Claims.Models;
using Claims.Models.ComputePremium;
using Claims.Models.Cover;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

/// <summary>
/// Controller to manage cover-related operations such as computing premiums,
/// retrieving, creating, and deleting covers.
/// </summary>
[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ICoverService _coverService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoversController"/> class.
    /// </summary>
    /// <param name="coverService">Service for handling cover-related business logic.</param>
    public CoversController(ICoverService coverService)
    {
        _coverService = coverService;
    }

    /// <summary>
    /// Computes the premium for a given cover based on the start date, end date, and cover type.
    /// </summary>
    /// <param name="startDate">The start date of the cover period.</param>
    /// <param name="endDate">The end date of the cover period.</param>
    /// <param name="coverType">The type of the cover.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing the computed premium on success;
    /// otherwise, a BadRequest with validation or business rule errors.
    /// </returns>
    [HttpPost("compute")]
    public ActionResult ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        ComputePremiumResult result = _coverService.ComputePremium(startDate, endDate, coverType);
        return result.IsSuccess ? Ok(result.ComputePremium) : BadRequest(result.Errors);
    }

    /// <summary>
    /// Retrieves all covers asynchronously.
    /// </summary>
    /// <returns>
    /// An <see cref="ActionResult"/> containing a list of <see cref="CoverResponse"/> representing all covers.
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoverResponse>>> GetAsync()
    {
        var covers = await _coverService.GetAllCoversAsync();
        return Ok(covers);
    }

    /// <summary>
    /// Retrieves a cover by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the cover.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing the <see cref="CoverResponse"/> if found;
    /// otherwise, <c>null</c>.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CoverResponse?>> GetByIdAsync(string id)
    {
        return await _coverService.GetCoverByIdAsync(id);
    }

    /// <summary>
    /// Creates a new cover with the provided request data.
    /// </summary>
    /// <param name="coverRequest">The data for the cover to create.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> containing the newly created <see cref="CoverResponse"/> on success;
    /// otherwise, a BadRequest with validation or business rule errors.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult> CreateAsync(CoverRequest coverRequest)
    {
        var result = await _coverService.CreateCoverAsync(coverRequest);
        return result?.IsSuccess ?? false ? Ok(result?.NewCover) : BadRequest(result?.Errors);
    }

    /// <summary>
    /// Deletes a cover by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the cover to delete.</param>
    /// <returns>
    /// <see cref="NoContentResult"/> on successful deletion;
    /// otherwise, <see cref="NotFoundResult"/> if the cover does not exist.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        bool deleted = await _coverService.DeleteCoverAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
