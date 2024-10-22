using MQTTnet;
using MqttLibrary;
using System.Text;
// subscriber
public class Program {
    [Obsolete]
    private static async Task Main(string[] args) {
        MqttConfig mqttConfig = new();
        Subscriber subscriber = new(mqttConfig);
        subscriber.Handlers["channel-21071999/send/message"] = async (MqttApplicationMessage e) => {
            Console.WriteLine($"#Message: {Encoding.UTF8.GetString(e.Payload)}");
            await Task.CompletedTask;
        };
        await subscriber.StartAsync();        
        Thread.Sleep(-1);
    }
}