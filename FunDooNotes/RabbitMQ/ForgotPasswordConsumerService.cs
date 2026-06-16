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
    public class ForgotPasswordConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection _connection;
        private IModel _channel;

        public ForgotPasswordConsumerService(
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
                queue: "fundoo.forgotpassword.queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var consumer =
                new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                Console.WriteLine("Forgot Password Consumer Triggered");
                try
                {
                    var body =
                        eventArgs.Body.ToArray();

                    var message =
                        Encoding.UTF8.GetString(body);

                    var forgotPasswordMessage =
                        JsonSerializer.Deserialize<ForgotPasswordMessageDTO>(
                            message);

                    if (forgotPasswordMessage != null)
                    {
                        using var scope =
                            _scopeFactory.CreateScope();

                        var emailService =
                            scope.ServiceProvider
                            .GetRequiredService<IEmailService>();

                        string resetLink =
                            $"https://localhost:7221/reset-password?email={forgotPasswordMessage.Email}&token={forgotPasswordMessage.ResetToken}";

                        emailService.SendEmail(
                            new EmailDTO
                            {
                                ToEmail =
                                    forgotPasswordMessage.Email,

                                Subject =
                                    "Fundoo Notes Password Reset",

                                Body =
                                $@"
                                <h2>Password Reset Request</h2>

                                <p>
                                    Click the link below to reset your password:
                                </p>

                                <a href='{resetLink}'>
                                    Reset Password
                                </a>"
                            });
                    }

                    _channel.BasicAck(
                        eventArgs.DeliveryTag,
                        false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };

            _channel.BasicConsume(
                queue: "fundoo.forgotpassword.queue",
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