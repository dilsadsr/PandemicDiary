using System;
using System.Text;
using EventBusRabbitMq.Events;
using EventBusRabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EventBusRabbitMq.Producer
{
    public class EventBusRabbitMqProducer
    {
        private readonly IRabbitMqConnection _connection;

        public EventBusRabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void PublishSaveDiaryNoteEvent(string queueName, SaveDiaryNoteEvent publishModel)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(publishModel);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties prop = channel.CreateBasicProperties();
                prop.Persistent = true;
                prop.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "",
                                    routingKey: queueName,
                                    basicProperties: null,
                                    body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) => {
                    Console.WriteLine("Sent RabbitMq");
                };

                channel.ConfirmSelect();
            }
        }
    }
}
