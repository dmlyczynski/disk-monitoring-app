using AutoFixture;
using AutoFixture.AutoMoq;

using DiskMonitoring.Client.Core.DeviceIoControll;
using DiskMonitoring.Client.Core.Volumes;
using DiskMonitoring.Client.Module;

using Moq;

namespace DiskMonitoring.Client.Tests;

public class ReportServiceTests
{
    private readonly IFixture _fixture;

    public ReportServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    [Fact]
    public void CalculateReport_WhenVolumesAreEmptyList_ThenShouldReturnEmptyString()
    {
        // Arrange 
        _fixture.Freeze<Mock<IVolumeService>>()
            .Setup(x => x.EnumerateVolumes())
            .Returns(Array.Empty<string>);

        // Act 
        var result = CreateSut().CalculateReport();

        // Assert 
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CalculateReport_WhenVolumesAreNotEmpty_ThenShouldBeNotEmpty()
    {
        // Arrange 
        _fixture.Freeze<Mock<IVolumeService>>()
            .Setup(x => x.EnumerateVolumes())
            .Returns(new[] {"test", "test2"});

        // Act 
        var result = CreateSut().CalculateReport();

        // Assert 
        Assert.NotEmpty(result);
    }

    [Fact]
    public void CalculateReport_WhenGetVolumeDiskExtentsIsNotEmpty_ThenShouldReturnValue()
    {
        // Arrange 
        _fixture.Freeze<Mock<IVolumeService>>()
            .Setup(x => x.EnumerateVolumes())
            .Returns(new[] { "test" });

        _fixture.Freeze<Mock<INativeDiskService>>()
            .Setup(x => x.GetVolumeDiskExtents(It.IsAny<string>()))
            .Returns([_fixture.Create<DiskExtent>()]);

        // Act 
        var result = CreateSut().CalculateReport();

        // Assert 
        Assert.NotEmpty(result);
    }

    private IReportService CreateSut() => _fixture.Create<ReportService>();
}