using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using RabbitMQ.Client.Events;

using Common.Queue.Consumer.Delegates;
using Common.Queue.Message;

namespace Common.Queue.Consumer
{
    public class QueueConsumer : IQueueConsumer
    {
        private readonly AsyncEventingBasicConsumer _eventConsumer;
        private readonly ConcurrentDictionary<string, AsyncMessageEventHadler> _eventDictionary;
        private readonly JsonSerializerOptions _jsonSerializerOption;
        private readonly ILogger _logger;

        private event AsyncMessageEventHadler? _messageEventHadler;

        public QueueConsumer(AsyncEventingBasicConsumer eventConsumer, JsonSerializerOptions jsonSerializerOption, ILogger logger) 
        {
            if (eventConsumer == null)
                throw new ArgumentNullException(nameof(eventConsumer));

            if (jsonSerializerOption == null)
                throw new ArgumentNullException(nameof(jsonSerializerOption));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _eventConsumer = eventConsumer;
            _eventDictionary = new ConcurrentDictionary<string, AsyncMessageEventHadler>();
            _jsonSerializerOption = jsonSerializerOption;
            _logger = logger;
        }
    
        public void StartConsume()
        {
            _eventConsumer.ReceivedAsync += RecieveMessage;
        }

        private async Task RecieveMessage(object sender, BasicDeliverEventArgs e)
        {
            string? queueMessage = null;

            try
            {
                byte[] body = e.Body.ToArray();
                queueMessage = Encoding.UTF8.GetString(body);

                if (e.Body.Length == 0 && string.IsNullOrEmpty(queueMessage!))
                {
                    string info = $"Body or text message is empty. ConsumerTag: {e.ConsumerTag}, DeliveryTag: {e.DeliveryTag}, RoutingKey: {e.RoutingKey}";
                    _logger.LogInformation(info);
                    return;
                }

                MessageContext? messageContext = JsonSerializer.Deserialize<MessageContext>(queueMessage!, _jsonSerializerOption);

                if (messageContext != null)
                {
                    if (_messageEventHadler != null)
                    {
                        await _messageEventHadler(messageContext);
                    }
                    else
                    {
                        string key = messageContext!.PublisherId!;

                        if (_eventDictionary.TryGetValue(key, out AsyncMessageEventHadler? eventHandler) && eventHandler != null)
                        {
                            await eventHandler(messageContext);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ExceptionMessage: {exceptionMessage}\n QueueMessage:{queueMessage}", ex.Message, queueMessage);
            }
        }

        public void AddConsumer(AsyncMessageEventHadler eventHandler)
        {
            _messageEventHadler += eventHandler;
        }

        public bool TryAddConsumer(string key, AsyncMessageEventHadler eventHandler)
        {
            return _eventDictionary.TryAdd(key, eventHandler);
        }

        public bool TryRemoveConsumer(string key)
        {
            return _eventDictionary.TryRemove(key, out AsyncMessageEventHadler? eventHandler);
        }
    }
}
