using Claims.Controllers;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.CoverControllerTests;

public class CoversController_DeleteTests
{
    private readonly CoversController _controller;
    private readonly Mock<ICoverService> _mockCoverService = new();

    public CoversController_DeleteTests()
    {
        _controller = new CoversController(_mockCoverService.Object);
    }

    [Fact]
    public async Task Test_DeleteAsync_WhenSuccess()
    {
        //Arrange
        var coverId = Guid.NewGuid().ToString();
        _mockCoverService.Setup(s => s.DeleteCoverAsync(coverId)).ReturnsAsync(true);

        //Act
        var result = await _controller.DeleteAsync(coverId);

        //Assert
        Assert.IsType<NoContentResult>(result);
        _mockCoverService.Verify(s => s.DeleteCoverAsync(coverId), Times.Once);
    }

    [Fact]
    public async Task Test_DeleteAsync_WhenFails()
    {
        //Arrange
        var coverId = Guid.NewGuid().ToString();
        _mockCoverService.Setup(s => s.DeleteCoverAsync(coverId)).ReturnsAsync(false);

        //Act
        var result = await _controller.DeleteAsync(coverId);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        _mockCoverService.Verify(s => s.DeleteCoverAsync(coverId), Times.Once);
    }
}
