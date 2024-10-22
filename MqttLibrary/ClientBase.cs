using MQTTnet.Client;
using MQTTnet;
namespace MqttLibrary
{
    public class ClientBase
    {
        protected readonly IMqttClient _mqttClient;
        protected readonly MqttConfig _mqttConfig;
        public IMqttClient MqttClient => _mqttClient;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="config"></param>
        public ClientBase(IMqttClient client, MqttConfig config)
        {
            _mqttClient = client;
            _mqttConfig = config;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config"></param>
        public ClientBase(MqttConfig config)
        {
            _mqttConfig = config;
            _mqttClient = new MqttFactory().CreateMqttClient();
        }
        /// <summary>
        /// Connect to the MQTT broker
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            var options = new MqttClientOptionsBuilder()
                    //.WithClientId(clientId)
                    .WithTcpServer(_mqttConfig.URL, _mqttConfig.PORT)
                    .WithCleanSession()
                    .Build();
            await _mqttClient.ConnectAsync(options);
        }
        /// <summary>
        /// Disconnect from the MQTT broker
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectAsync()
        {
            await _mqttClient.DisconnectAsync();
        }
    }
}