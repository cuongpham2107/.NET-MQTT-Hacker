using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
namespace MqttLibrary
{
    public class Publisher : ClientBase
    {
        public Publisher(IMqttClient client, MqttConfig config) : base(client, config) { }
        public Publisher(MqttConfig config) : base(config) { }
        /// <summary>
        /// Publish a message with no payload
        /// </summary>
        /// <param name="_topic"></param>
        /// <param name="_retainFlag"></param>
        /// <param name="_qos"></param>
        /// <returns></returns>
        public async Task PublishNoPayloadAsync(
            string _topic, 
            bool _retainFlag = false, 
            MqttQualityOfServiceLevel _qos = MqttQualityOfServiceLevel.AtMostOnce
        )
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(_topic)
                .WithPayload(new byte[] { 0 })
                .WithContentType("0")
                .WithQualityOfServiceLevel(_qos)
                .WithRetainFlag(_retainFlag)
                .Build();
            await _mqttClient.PublishAsync(message);
        }
        /// <summary>
        /// Publish a string message
        /// </summary>
        /// <param name="_topic"></param>
        /// <param name="_payload"></param>
        /// <param name="_qos"></param>
        /// <param name="_retailFlag"></param>
        /// <returns></returns>
        public async Task PublishStringAsync(
            string _topic, 
            string _payload, 
            MqttQualityOfServiceLevel _qos = MqttQualityOfServiceLevel.AtMostOnce, 
            bool _retailFlag = false)
        {
            await _mqttClient.PublishStringAsync(_topic, _payload, _qos, _retailFlag);
        }
        /// <summary>
        /// Publish a binary message
        /// </summary>
        /// <param name="_topic"></param>
        /// <param name="_payload"></param>
        /// <param name="_qos"></param>
        /// <param name="_retailFlag"></param>
        /// <returns></returns>
        public async Task PublishBinaryAsync(
            string _topic, 
            byte[] _payload, 
            MqttQualityOfServiceLevel _qos = MqttQualityOfServiceLevel.AtMostOnce, 
            bool _retailFlag = false)
        {
            await _mqttClient.PublishBinaryAsync(_topic, _payload, _qos, _retailFlag);
        }
    }
}