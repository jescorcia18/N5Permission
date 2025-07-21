using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSPermissions.Domain.Dtos;
using System.Net.Http.Json;
using System.Net;
using NSPermissions.Api;


namespace NSPermissions.Test.Unit.Controllers;

public class PermissionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PermissionsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPermissions_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/permissions");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostPermission_ReturnsCreated()
    {
        var newPermission = new PermissionDto
        {
            EmployeeName = "Juan",
            EmployeeLastName = "Escorcia",
            PermissionDate = DateTime.UtcNow,
            PermissionTypeId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/permissions/request", newPermission);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ModifyPermission_NotFound_WhenInvalidId()
    {
        var updateDto = new PermissionDto
        {
            EmployeeName = "Nuevo",
            EmployeeLastName = "Apellido",
            PermissionDate = DateTime.UtcNow,
            PermissionTypeId = 1
        };

        var response = await _client.PutAsJsonAsync("/api/permissions/modify/9999", updateDto);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePermission_ReturnsNotFound_WhenInvalidId()
    {
        var response = await _client.DeleteAsync("/api/permissions/delete/9999");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
