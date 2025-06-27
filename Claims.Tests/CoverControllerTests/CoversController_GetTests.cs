using Claims.Controllers;
using Claims.Models;
using Claims.Models.Cover;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.CoverControllerTests;

public class CoversController_GetTests
{
    private readonly CoversController _controller;
    private readonly Mock<ICoverService> _mockCoverService = new();

    public CoversController_GetTests()
    {
        _controller = new CoversController(_mockCoverService.Object);
    }

    [Fact]
    public async Task Test_GetAsync_WhenCoversExist()
    {
        // Arrange
        var expectedId = Guid.NewGuid().ToString();
        var covers = new List<CoverResponse>
        {
            new CoverResponse
            {
                Id = expectedId,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddDays(30),
                Type = CoverType.Yacht,
                Premium = 10000m
            }
        };

        _mockCoverService.Setup(s => s.GetAllCoversAsync()).ReturnsAsync(covers);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var resultList = Assert.IsAssignableFrom<IEnumerable<CoverResponse>>(okResult.Value).ToList();

        Assert.Single(resultList);
        Assert.Equal(expectedId, resultList[0].Id);
        Assert.Equal(CoverType.Yacht, resultList[0].Type);
        Assert.Equal(10000m, resultList[0].Premium);

        _mockCoverService.Verify(s => s.GetAllCoversAsync(), Times.Once);
    }

    [Fact]
    public async Task Test_GetAsync_WhenNoCovers()
    {
        // Arrange
        _mockCoverService.Setup(s => s.GetAllCoversAsync()).ReturnsAsync(new List<CoverResponse>());

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var covers = Assert.IsAssignableFrom<IEnumerable<CoverResponse>>(okResult.Value);
        Assert.Empty(covers);
        _mockCoverService.Verify(s => s.GetAllCoversAsync(), Times.Once);
    }

    [Fact]
    public async Task Test_GetByIdAsync_WhenExists()
    {
        //Arrange
        var coverId = Guid.NewGuid().ToString();
        var cover = new CoverResponse { Id = coverId };

        _mockCoverService.Setup(s => s.GetCoverByIdAsync(coverId)).ReturnsAsync(cover);

        //Act
        var result = await _controller.GetByIdAsync(coverId);

        //Assert
        Assert.Equal(coverId, result?.Value?.Id);
        _mockCoverService.Verify(s => s.GetCoverByIdAsync(coverId), Times.Once);
    }

    [Fact]
    public async Task Test_GetByIdAsync_WhenCoverNotExists()
    {
        //Arrange
        var coverId = Guid.NewGuid().ToString();
        var empty = new CoverResponse();

        _mockCoverService.Setup(s => s.GetCoverByIdAsync(coverId)).ReturnsAsync(empty);

        //Act
        var result = await _controller.GetByIdAsync(coverId);

        //Assert
        Assert.Null(result?.Value?.Id);
        _mockCoverService.Verify(s => s.GetCoverByIdAsync(coverId), Times.Once);
    }
}
