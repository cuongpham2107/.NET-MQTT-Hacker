using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System;
namespace MqttLibrary
{
    public class Subscriber : ClientBase
    {
        public Subscriber(IMqttClient client, MqttConfig config) : base(client, config) { }
        public Subscriber(MqttConfig config) : base(config) { }
        public Dictionary<string, Func<MqttApplicationMessage, Task>> Handlers { get; set; } = new();
        public Func<string, Task>? ErrorHandler { get; set; }
        public Func<string, Task>? LogHandler { get; set; }
        /// <summary>
        /// Start the subscriber
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _mqttClient.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;
            await ConnectAsync();
            await SubscribeAsync(Handlers.Keys.ToArray());
        }
        /// <summary>
        /// Subscribe to a topic
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="qos"></param>
        /// <returns></returns>
        public async Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(qos)
                .Build();
            await _mqttClient.SubscribeAsync(topicFilter);
        }
        /// <summary>
        /// Subscribe to multiple topics
        /// </summary>
        /// <param name="topics"></param>
        /// <param name="qos"></param>
        /// <returns></returns>
        public async Task SubscribeAsync(string[] topics, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            foreach (var topic in topics)
            {
                await _mqttClient.SubscribeAsync(topic, qos);
            }
        }
        /// <summary>
        /// Unsubscribe from a topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task UnsubscribeAsync(string topic)
        {
            await _mqttClient.UnsubscribeAsync(topic);
        }
        /// <summary>
        /// Unsubscribe from multiple topics
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            string topic = arg.ApplicationMessage.Topic;
            // if this is a rpc call - abort
            if (topic.StartsWith("MQTTnet.RPC/")) { return; }
            try
            {
                await Handlers[topic].Invoke(arg.ApplicationMessage);
            }
            catch (Exception e)
            {
                if (ErrorHandler != null)
                    await ErrorHandler.Invoke(e.Message);
            }
        }
    }
}
