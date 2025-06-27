using Claims.Controllers;
using Claims.Models;
using Claims.Models.Claims;
using Claims.Models.Cover;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.ClaimsControllerTests;

public class ClaimsController_GetTests
{
    private readonly Mock<IClaimService> _mockClaimService;
    private readonly ClaimsController _controller;

    public ClaimsController_GetTests()
    {
        _mockClaimService = new Mock<IClaimService>();
        _controller = new ClaimsController(_mockClaimService.Object);
    }

    [Fact]
    public async Task Test_GetAsync_WhenClaimsExist()
    {
        //Arrange
        var expectedId = Guid.NewGuid().ToString();
        var claims = new List<ClaimResponse>
        {
            new ClaimResponse
            {
                Id = expectedId,
                CoverId = Guid.NewGuid().ToString(),
                Created = DateTime.Now.Date.AddDays(5),
                Name = "John Doe",
                Type = ClaimType.Fire,
                DamageCost = 12500.75m
            }
        };

        _mockClaimService.Setup(s => s.GetAllClaimsAsync()).ReturnsAsync(claims);

        //Act
        var result = await _controller.GetAsync();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultList = Assert.IsAssignableFrom<IEnumerable<ClaimResponse>>(okResult.Value).ToList();

        Assert.Single(resultList);
        Assert.Equal(expectedId, resultList[0].Id);
        Assert.Equal("John Doe", resultList[0].Name);
        Assert.Equal(ClaimType.Fire, resultList[0].Type);

        _mockClaimService.Verify(s => s.GetAllClaimsAsync(), Times.Once);
    }


    [Fact]
    public async Task Test_GetAsync_WhenNoClaims()
    {
        //Arrange
        _mockClaimService.Setup(s => s.GetAllClaimsAsync()).ReturnsAsync(new List<ClaimResponse>());

        //Act
        var result = await _controller.GetAsync();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var claims = Assert.IsAssignableFrom<IEnumerable<ClaimResponse>>(okResult.Value);
        Assert.Empty(claims);

        _mockClaimService.Verify(s => s.GetAllClaimsAsync(), Times.Once);
    }

    [Fact]
    public async Task Test_GetByIdAsync_WhenClaimExists()
    {
        //Arrange
        var claimId = Guid.NewGuid().ToString();
        var claim = new ClaimResponse
        {
            Id = claimId,
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.Now.Date,
            Name = "John Doe",
            Type = ClaimType.Fire,
            DamageCost = 7500m
        };

        _mockClaimService.Setup(s => s.GetClaimByIdAsync(claimId)).ReturnsAsync(claim);

        //Act
        var result = await _controller.GetByIdAsync(claimId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(claimId, result?.Value?.Id);
        Assert.Equal("John Doe", result?.Value?.Name);
        _mockClaimService.Verify(s => s.GetClaimByIdAsync(claimId), Times.Once);
    }

    [Fact]
    public async Task Test_GetByIdAsync_WhenClaimDoesNotExist()
    {
        //Arrange
        var claimId = Guid.NewGuid().ToString();
        var empty = new ClaimResponse();
        _mockClaimService.Setup(s => s.GetClaimByIdAsync(claimId)).ReturnsAsync(empty);

        //Act
        var result = await _controller.GetByIdAsync(claimId);

        //Assert
        Assert.Null(result?.Value?.CoverId);
        _mockClaimService.Verify(s => s.GetClaimByIdAsync(claimId), Times.Once);
    }
}
