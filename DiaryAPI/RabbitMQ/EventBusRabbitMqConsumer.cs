using System;
using MediatR;
using AutoMapper;
using EventBusRabbitMQ;
using System.Text;
using EventBusRabbitMq.Common;
using RabbitMQ.Client.Events;
using DiaryAPI.Repositories.Interfaces;
using Newtonsoft.Json;
using EventBusRabbitMq.Events;
using DiaryAPI.Entities;
using RabbitMQ.Client;

namespace DiaryAPI.RabbitMQ
{
    public class EventBusRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IDiaryNoteRepository _repository;

        public EventBusRabbitMqConsumer(IRabbitMqConnection connection, IDiaryNoteRepository repository)
        {
            _connection = connection;
            _repository = repository;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConsts.DiaryNoteQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;
            channel.BasicConsume(queue: EventBusConsts.DiaryNoteQueue, autoAck: true, consumer: consumer);
        }


        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConsts.DiaryNoteQueue)
            {
                var msg = Encoding.UTF8.GetString(e.Body.Span);
                var data = JsonConvert.DeserializeObject<SaveDiaryNoteEvent>(msg);

                var result = _repository.Create(new DiaryNote() { PersonName = data.PersonName, Note = data.Note });

            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
