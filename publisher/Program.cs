using MqttLibrary;
// publisher
public class Program {    
    private static async Task Main(string[] args) {
        MqttConfig mqttConfig = new MqttConfig();
        Publisher publisher = new Publisher(mqttConfig);
        await publisher.ConnectAsync();
        while(true) {
            Console.Write("Message >>> ");
            string? payload = Console.ReadLine();
            await publisher.PublishStringAsync("channel-21071999/send/message", payload!);
        }        
    }
}