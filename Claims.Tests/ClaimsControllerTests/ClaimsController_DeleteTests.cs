using Claims.Controllers;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.ClaimsControllerTests;

public class ClaimsController_DeleteTests
{
    private readonly Mock<IClaimService> _mockClaimService;
    private readonly ClaimsController _controller;

    public ClaimsController_DeleteTests()
    {
        _mockClaimService = new Mock<IClaimService>();
        _controller = new ClaimsController(_mockClaimService.Object);
    }

    [Fact]
    public async Task Test_DeleteAsync_WhenDeletionSucceeds()
    {
        //Arrange
        var claimId = Guid.NewGuid().ToString();
        _mockClaimService.Setup(s => s.DeleteClaimAsync(claimId)).ReturnsAsync(true);

        //Act
        var actionResult = await _controller.DeleteAsync(claimId);

        //Assert
        Assert.IsType<NoContentResult>(actionResult);
        _mockClaimService.Verify(s => s.DeleteClaimAsync(claimId), Times.Once);
    }

    [Fact]
    public async Task Test_DeleteAsync_WhenClaimDoesNotExist()
    {
        //Arrange
        var claimId = Guid.NewGuid().ToString();
        _mockClaimService.Setup(s => s.DeleteClaimAsync(claimId)).ReturnsAsync(false);

        //Act
        var actionResult = await _controller.DeleteAsync(claimId);

        //Assert
        Assert.IsType<NotFoundResult>(actionResult);
        _mockClaimService.Verify(s => s.DeleteClaimAsync(claimId), Times.Once);
    }
}
