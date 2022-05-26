using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SongsService.Dtos;

namespace SongsService.Services
{
    public interface IMessageBusPublisher
    {
        void PublishChangeSong(SongPublishDto songPublishDto);
    }

    public class MessageBusPublisher : IMessageBusPublisher
    {
        private readonly ILogger<MessageBusPublisher> _logger;
        private readonly IConnection _connection = null!;
        private readonly IModel _channel = null!;

        public MessageBusPublisher(IConfiguration configuration, ILogger<MessageBusPublisher> logger)
        {
            _logger = logger;

            // Connection config object
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(configuration.GetValue<string>("RabbitMq:Uri"))
            };

            // Create connection
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += OnConnectionShutdown;

                _logger.LogInformation("Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create connection to RabbitMQ");
            }
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("Disconnected from RabbitMQ");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        public void PublishChangeSong(SongPublishDto songPublishDto)
        {
            var message = JsonSerializer.Serialize(songPublishDto);

            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                body: Encoding.UTF8.GetBytes(message));

            _logger.LogInformation("Published message to RabbitMQ");
        }
    }
}