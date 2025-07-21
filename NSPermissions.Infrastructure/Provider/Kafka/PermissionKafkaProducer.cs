using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSPermissions.Domain.Interfaces;

using System.Text.Json;


namespace NSPermissions.Infrastructure.Provider.Kafka;

public class PermissionKafkaProducer : IPermissionKafkaProducer
{
    private readonly ILogger<PermissionKafkaProducer> _logger;
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic = "permission-events";

    public PermissionKafkaProducer(ILogger<PermissionKafkaProducer> logger, IConfiguration configuration)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync(string operation)
    {
        var message = new
        {
            Id = Guid.NewGuid(),
            Name = operation,
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(message);

        var deliveryResult = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });

        if (deliveryResult.Status == PersistenceStatus.Persisted)
            _logger.LogInformation("Kafka event published: {Operation}", operation);
        else
            _logger.LogWarning("Kafka event not persisted: {Operation}", operation);
    }
}
