using Claims.Controllers;
using Claims.Models;
using Claims.Models.Claims;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.ClaimsControllerTests;

public class ClaimsController_CreateTests
{
    private readonly Mock<IClaimService> _mockClaimService;
    private readonly ClaimsController _controller;

    public const string _relatedCoverNotExist = "Related cover does not exist";
    public const string _damageCostExceedsLimit = "DamageCost cannot exceed 100.000";
    public const string _createdDateOutOfCoverPeriod = "Created date must be within the period of the related Cover";

    public ClaimsController_CreateTests()
    {
        _mockClaimService = new Mock<IClaimService>();
        _controller = new ClaimsController(_mockClaimService.Object);
    }

    [Fact]
    public async Task Test_CreateAsync_WhenCreationSucceeds()
    {
        //Arrange
        var claimRequest = new ClaimRequest
        {
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.Now.Date,
            Name = "John Doe",
            Type = ClaimType.Fire,
            DamageCost = 5000m
        };

        var newClaim = new ClaimResponse
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = claimRequest.CoverId,
            Created = claimRequest.Created,
            Name = claimRequest.Name,
            Type = claimRequest.Type,
            DamageCost = claimRequest.DamageCost
        };

        var claimResult = new ClaimResult
        {
            NewClaim = newClaim,
            Errors = new List<string>()
        };

        _mockClaimService
            .Setup(s => s.CreateClaimAsync(It.IsAny<ClaimRequest>()))
            .ReturnsAsync(claimResult);

        //Act
        var actionResult = await _controller.CreateAsync(claimRequest);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returnedClaim = Assert.IsType<ClaimResponse>(okResult.Value);
        Assert.Equal(newClaim.Id, returnedClaim.Id);
        Assert.Equal(claimRequest.Name, returnedClaim.Name);
        Assert.True(claimResult.IsSuccess);
        _mockClaimService.Verify(s => s.CreateClaimAsync(It.IsAny<ClaimRequest>()), Times.Once);
    }

    [Fact]
    public async Task Test_CreateAsync_WhenCreationFails()
    {
        //Arrange
        var claimRequest = new ClaimRequest
        {
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.Now.Date.AddYears(-2),//Date out of period
            Name = "John Doe",
            Type = ClaimType.Fire,
            DamageCost = 150000m // exceeding limit
        };

        var errors = new List<string>
        {
            _relatedCoverNotExist,
            _damageCostExceedsLimit,
            _createdDateOutOfCoverPeriod
        };

        var claimResult = new ClaimResult
        {
            Errors = errors
        };

        _mockClaimService
            .Setup(s => s.CreateClaimAsync(It.IsAny<ClaimRequest>()))
            .ReturnsAsync(claimResult);

        //Act
        var actionResult = await _controller.CreateAsync(claimRequest);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        var returnedErrors = Assert.IsAssignableFrom<IEnumerable<string>>(badRequestResult.Value);
        Assert.Equal(errors.Count, returnedErrors.Count());
        Assert.Contains(_relatedCoverNotExist, returnedErrors);
        Assert.Contains(_damageCostExceedsLimit, returnedErrors);
        Assert.Contains(_createdDateOutOfCoverPeriod, returnedErrors);
        Assert.False(claimResult.IsSuccess);
        _mockClaimService.Verify(s => s.CreateClaimAsync(It.IsAny<ClaimRequest>()), Times.Once);
    }
}
