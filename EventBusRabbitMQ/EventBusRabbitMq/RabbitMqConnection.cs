using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMQ.Properties
{
    public class RabbitMqConnection :IRabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMqConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            if (!IsConnected) {
                TryConnect();
            }
        }

        public bool IsConnected { get { return _connection != null && _connection.IsOpen && !_disposed; } }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMq connection"); 
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) { return; }
            try
            {
                _connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            return IsConnected;
        }
    }
}
