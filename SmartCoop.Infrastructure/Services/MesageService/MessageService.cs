using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Services;

namespace SmartCoop.Infrastructure.Services.MesageService
{
    public class MessageService : IMessageService
    {
        private readonly IMessageServiceConfig _config;
        private readonly ICoop _coop;
        private readonly ILogger _logger;
        private readonly string _clientId = Guid.NewGuid().ToString();
        private IManagedMqttClient _client;

        public MessageService(IMessageServiceConfig config, ICoop coop, ILogger<MessageService> logger)
        {
            _config = config;
            _coop = coop;
            _logger = logger;
        }

        public async Task Connect()
        {
            if (string.IsNullOrEmpty(_config.MqttUri))
            {
                return;
            }

            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(_clientId)
                .WithCredentials(_config.MqttUser, _config.MqttPassword)
                .WithTcpServer(_config.MqttUri, _config.MqttPort)
                .WithCleanSession();
            if (!string.IsNullOrEmpty(_config.MqttUser) && !string.IsNullOrEmpty(_config.MqttPassword))
            {
                messageBuilder.WithCredentials(_config.MqttUser, _config.MqttPassword);
            }
            if (_config.MqttSecure)
            {
                messageBuilder.WithTls();
            }
            var options = messageBuilder.Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(options)
                .Build();

            _client = new MqttFactory().CreateManagedMqttClient();

            await _client.StartAsync(managedOptions);

            _client.UseConnectedHandler(async e =>
            {
                _logger.LogInformation("Connected successfully with MQTT Brokers.");
                await _client.SubscribeAsync(new MqttTopicFilterBuilder()
                    .WithTopic($"{_config.MqttBaseTopic}/#")
                    .WithAtLeastOnceQoS()
                    .Build());
                _client.UseApplicationMessageReceivedHandler(Handler);
            });

            _client.UseDisconnectedHandler(e =>
            {
                _logger.LogInformation("Disconnected from MQTT Brokers.");
            });

            
            
        }

        private Task Handler(MqttApplicationMessageReceivedEventArgs e)
        {
            var regEx = $"{_config.MqttBaseTopic}/([^/]*)/(.*)";
            var match = Regex.Match(e.ApplicationMessage.Topic, regEx);
            if (match.Groups.Count >= 3)
            {
                var deviceName = match.Groups[1].Value;
                var message = match.Groups[2].Value;
                var device = _coop.Devices.FirstOrDefault(d => d.Name == deviceName);
                device?.HandleMessage(message);
            }
            return Task.CompletedTask;
        }

        public void SendMessage(string topic, string payload)
        {
            _client.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithAtLeastOnceQoS()
                .Build());
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}