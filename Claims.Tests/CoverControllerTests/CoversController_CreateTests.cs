using Claims.Controllers;
using Claims.Models;
using Claims.Models.Cover;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.CoverControllerTests;

public class CoversController_CreateTests
{
    private readonly CoversController _controller;
    private readonly Mock<ICoverService> _mockCoverService = new();

    private const string _startDateInThePast = "StartDate cannot be in the past";
    private const string _totalPeriodExceedYear = "Total insurance period cannot exceed 1 year";
    private const string _endDateBeforeStartDate = "End date must be later than the start date";

    public CoversController_CreateTests()
    {
        _controller = new CoversController(_mockCoverService.Object);
    }

    [Fact]
    public async Task Test_CreateAsync_WhenSuccess()
    {
        //Arrange
        var request = new CoverRequest
        {
            StartDate = DateTime.Now.Date.AddDays(1),
            EndDate = DateTime.Now.Date.AddDays(10),
            Type = CoverType.Tanker
        };

        var id = Guid.NewGuid().ToString();
        var response = new CoverResponse { Id = id, Premium = 15000m };
        var result = new CoverResult { NewCover = response };

        _mockCoverService.Setup(s => s.CreateCoverAsync(request)).ReturnsAsync(result);

        //Act
        var actionResult = await _controller.CreateAsync(request);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var returned = Assert.IsType<CoverResponse>(okResult.Value);
        Assert.Equal(id, returned.Id);
        _mockCoverService.Verify(s => s.CreateCoverAsync(request), Times.Once);
    }

    [Fact]
    public async Task Test_CreateAsync_WhenInvalid()
    {
        //Arrange
        var request = new CoverRequest
        {
            StartDate = DateTime.Now.Date.AddDays(-30),//StartDate cannot be in the past,
            EndDate = DateTime.Now.Date.AddYears(2),//Total insurance period cannot exceed 1 year,
            Type = CoverType.Yacht
        };

        var result = new CoverResult
        {
            Errors = new List<string> { _startDateInThePast, _totalPeriodExceedYear, _endDateBeforeStartDate }
        };

        _mockCoverService.Setup(s => s.CreateCoverAsync(request)).ReturnsAsync(result);

        //Act
        var actionResult = await _controller.CreateAsync(request);

        //Assert
        var badResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        var errors = Assert.IsType<List<string>>(badResult.Value);
        Assert.Contains(_startDateInThePast, errors);
        Assert.Contains(_totalPeriodExceedYear, errors);
        Assert.Contains(_endDateBeforeStartDate, errors);
        _mockCoverService.Verify(s => s.CreateCoverAsync(request), Times.Once);
    }
}
