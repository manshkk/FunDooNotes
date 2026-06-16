using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interfaces;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly IRabbitMQPublisher _publisher;

        public RabbitMQController(IRabbitMQPublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost("send")]
        public IActionResult SendMessage()
        {
            _publisher.Publish(
                "fundoo.email.queue",
                "Hello RabbitMQ");

            return Ok("Message Published Successfully");
        }
    }
}