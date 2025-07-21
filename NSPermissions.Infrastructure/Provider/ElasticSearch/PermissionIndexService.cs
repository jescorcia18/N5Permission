using Microsoft.Extensions.Logging;
using Nest;
using NSPermissions.Domain.Entities;
using NSPermissions.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSPermissions.Infrastructure.Provider.ElasticSearch;

public class PermissionIndexService (IElasticClient _elasticClient, ILogger<PermissionIndexService> _logger) : IPermissionIndexService
{

    public async Task IndexAsync(Permission permission)
    {
        var response = await _elasticClient.IndexDocumentAsync(permission);
        if (!response.IsValid)
        {
            _logger.LogError("Failed to index permission in Elasticsearch: {Reason}", response.ServerError?.ToString());
        }
        else
        {
            _logger.LogInformation("Permission indexed successfully with ID: {Id}", permission.Id);
        }
    }
}
