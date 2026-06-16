using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FunDooNotes.RabbitMQ
{
    public class NoteConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;

        private IConnection _connection;

        private IModel _channel;

        public NoteConsumerService(
            IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "fundoo.note.queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);
        }

        protected override Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var consumer =
                new EventingBasicConsumer(_channel);

            consumer.Received +=
                (sender, eventArgs) =>
                {
                    try
                    {
                        var body =
                            eventArgs.Body.ToArray();

                        var message =
                            Encoding.UTF8.GetString(body);

                        Console.WriteLine(
                            $"Note Event Received : {message}");

                        _channel.BasicAck(
                            eventArgs.DeliveryTag,
                            false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            ex.Message);
                    }
                };

            _channel.BasicConsume(
                queue: "fundoo.note.queue",
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();

            base.Dispose();
        }
    }
}