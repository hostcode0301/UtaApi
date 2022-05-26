using System.Text;
using PlaylistsService.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlaylistsService.Services
{
    public class MessageBusSubcriber : BackgroundService
    {
        private readonly ILogger<MessageBusSubcriber> _logger;
        private readonly IEventProcessor _eventProcessor;
        private readonly IConnection _connection = null!;
        private readonly IModel _channel = null!;
        private readonly string _queueName = null!;

        public MessageBusSubcriber(
            IConfiguration configuration,
            ILogger<MessageBusSubcriber> logger,
            IEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventProcessor = eventProcessor;

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

                // Declare waiting
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

                _connection.ConnectionShutdown += OnConnectionShutdown;

                _logger.LogInformation("Connected to RabbitMQ, Waiting for message ...");
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.ToArray());
                _logger.LogInformation($"Message received: {message}");

                // Handle message
                _eventProcessor.ProcessEvent(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}