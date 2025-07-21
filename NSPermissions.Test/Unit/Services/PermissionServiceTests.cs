using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSPermissions.Application.Services.Permissions.Imp;
using NSPermissions.Domain.Dtos;
using NSPermissions.Domain.Entities;
using NSPermissions.Domain.Interfaces;
using NSPermissions.Domain.Interfaces.UnitOfWork;

namespace NSPermissions.Test.Unit.Services;

public class PermissionServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUow;
    private readonly Mock<IPermissionIndexService> _mockIndex;
    private readonly Mock<IPermissionKafkaProducer> _mockKafka;
    private readonly PermissionService _service;

    public PermissionServiceTests()
    {
        _mockUow = new Mock<IUnitOfWork>();
        _mockIndex = new Mock<IPermissionIndexService>();
        _mockKafka = new Mock<IPermissionKafkaProducer>();

        _service = new PermissionService(
            _mockUow.Object,
            _mockIndex.Object,
            _mockKafka.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnPermissionDtoWithId()
    {
        // Arrange
        var dto = new PermissionDto
        {
            EmployeeName = "Juan",
            EmployeeLastName = "Escorcia",
            PermissionDate = DateTime.UtcNow,
            PermissionTypeId = 1
        };

        _mockUow.Setup(x => x.Permissions.AddAsync(It.IsAny<Permission>()));
        _mockUow.Setup(x => x.CommitAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Juan", result.EmployeeName.ToString());
        _mockKafka.Verify(x => x.PublishAsync("request"), Times.Once);
        _mockIndex.Verify(x => x.IndexAsync(It.IsAny<Permission>()), Times.Once);
    }
}
