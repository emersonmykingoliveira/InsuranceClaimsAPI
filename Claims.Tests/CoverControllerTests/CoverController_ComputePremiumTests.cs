using Claims.Controllers;
using Claims.Models;
using Claims.Models.ComputePremium;
using Claims.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Claims.Tests.CoverControllerTests;

public class CoversController_ComputePremiumTests
{
    private readonly CoversController _controller;
    private readonly Mock<ICoverService> _mockCoverService = new();

    private const string _startDateInThePast = "StartDate cannot be in the past";
    private const string _totalPeriodExceedYear = "Total insurance period cannot exceed 1 year";
    private const string _endDateBeforeStartDate = "End date must be later than the start date";

    public CoversController_ComputePremiumTests()
    {
        _controller = new CoversController(_mockCoverService.Object);
    }

    [Fact]
    public void Test_ComputePremium_WhenValidInput()
    {
        //Arrange
        var startDate = DateTime.Now.Date.AddDays(1);
        var endDate = DateTime.Now.Date.AddDays(30);
        var coverType = CoverType.Yacht;

        var computeResponse = new ComputePremiumResponse
        {
            StartDate = startDate,
            EndDate = endDate,
            Total = 50000m
        };

        var result = new ComputePremiumResult { ComputePremium = computeResponse };

        _mockCoverService.Setup(s => s.ComputePremium(startDate, endDate, coverType))
                         .Returns(result);

        //Act
        var response = _controller.ComputePremium(startDate, endDate, coverType);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        var returned = Assert.IsType<ComputePremiumResponse>(okResult.Value);
        Assert.Equal(50000m, returned.Total);
        _mockCoverService.Verify(s => s.ComputePremium(startDate, endDate, coverType), Times.Once);
    }

    [Fact]
    public void Test_ComputePremium_WhenInvalidDates()
    {
        //Arrange
        var startDate = DateTime.Now.Date.AddDays(-30);//StartDate cannot be in the past
        var endDate = DateTime.Now.Date.AddYears(2);//Total insurance period cannot exceed 1 year
        var coverType = CoverType.Tanker;

        var result = new ComputePremiumResult
        {
            Errors = new List<string> { _startDateInThePast, _totalPeriodExceedYear, _endDateBeforeStartDate }
        };

        _mockCoverService.Setup(s => s.ComputePremium(startDate, endDate, coverType))
                         .Returns(result);

        //Act
        var response = _controller.ComputePremium(startDate, endDate, coverType);

        //Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(response);
        var errors = Assert.IsType<List<string>>(badRequest.Value);
        Assert.Contains(_startDateInThePast, errors);
        Assert.Contains(_totalPeriodExceedYear, errors);
        Assert.Contains(_endDateBeforeStartDate, errors);
        _mockCoverService.Verify(s => s.ComputePremium(startDate, endDate, coverType), Times.Once);
    }
}
