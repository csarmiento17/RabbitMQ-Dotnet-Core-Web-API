using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication32.Models;
using RabbitMQ.Client;
using System.Text;

namespace WebApplication32.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody]CustomMessage customMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "customQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete:false,
                        arguments:null);

                    string message = "Id: " + customMessage.Id + ", Message: " + customMessage.Message + ", Date: " + customMessage.Date;
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "customQueue",
                        basicProperties: null,
                        body: body);
                }
            }
        }
    }
}
