using BusinessLayer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelLayer.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FunDooNotes.RabbitMQ
{
    public class EmailConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection _connection;
        private IModel _channel;

        public EmailConsumerService(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "fundoo.email.queue",
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
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();

                    var message =
                        Encoding.UTF8.GetString(body);

                    var emailMessage =
                        JsonSerializer.Deserialize<EmailMessageDTO>(
                            message);

                    if (emailMessage != null)
                    {
                        using var scope =
                            _scopeFactory.CreateScope();

                        var emailService =
                            scope.ServiceProvider
                            .GetRequiredService<IEmailService>();

                        emailService.SendEmail(
                            new EmailDTO
                            {
                                ToEmail = emailMessage.Email,

                                Subject = "Welcome to Fundoo Notes",

                                Body =
                                $@"
                                <div style='font-family:Arial,sans-serif;padding:20px'>
                                    <h2 style='color:#4CAF50'>
                                        Welcome {emailMessage.FirstName}
                                    </h2>

                                    <p>
                                        Your Fundoo Notes account has been
                                        created successfully.
                                    </p>

                                    <p>
                                        Happy Note Taking 📝
                                    </p>

                                    <br/>

                                    <p>
                                        Regards,<br/>
                                        <strong>Fundoo Notes Team</strong>
                                    </p>
                                </div>"
                            });

                        Console.WriteLine(
                            $"Welcome Email Sent To : {emailMessage.Email}");
                    }

                    _channel.BasicAck(
                        eventArgs.DeliveryTag,
                        false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"RabbitMQ Consumer Error : {ex.Message}");
                }
            };

            _channel.BasicConsume(
                queue: "fundoo.email.queue",
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