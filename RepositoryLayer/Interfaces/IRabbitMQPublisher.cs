namespace RepositoryLayer.Interfaces
{
    public interface IRabbitMQPublisher
    {
        void Publish(string queueName, string message);
    }
}