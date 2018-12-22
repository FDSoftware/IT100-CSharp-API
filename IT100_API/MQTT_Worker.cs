using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace IT100_API{

    class MQTT_Worker{
        public static async Task<int> Worker( string[] data ) {
            //weas para conectarme al mqtt y manejar los mensajes
            var factory = new MqttFactory();
            var cliente = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder().WithClientId(data[0]).WithTcpServer(data[1]).Build();
            //me conecto
            Console.WriteLine("### Conectando ###");
            await cliente.ConnectAsync(options);
            TopicFilterBuilder Filter = new TopicFilterBuilder();
            await cliente.SubscribeAsync( Filter.WithTopic(data[2]).Build() );
            await cliente.SubscribeAsync( Filter.WithTopic(data[3]).Build() );
            await cliente.SubscribeAsync( Filter.WithTopic(data[4]).Build() );
            Console.WriteLine("### Subscrito a los canales ###");

            //evento para los mensajes
            cliente.ApplicationMessageReceived += ( s , e ) => {
                Console.WriteLine("### Mensaje del Broker ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                //el topi                       payload, mensaje y data pegados para enviar al it100
                MQTT_Helper.Txt.Add(e.ApplicationMessage.Topic + "," + Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            };
            cliente.Disconnected += async ( s , e ) => {
                Console.WriteLine("### Desconectado ###");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try {
                    await cliente.ConnectAsync(options);
                }catch {
                    Console.WriteLine("### no se puede reconectar ###");
                    Logger.Log("Error al reconectarse al Broker");
                }
            };

            for (; ; ) {//este for es para sincronizar los mensajes
                foreach ( string item in MQTT_Helper.Pendiente ) {
                    string[] gs = item.Split(',');
                    Console.WriteLine(gs[0]);
                    var message = new MqttApplicationMessageBuilder().WithTopic(gs[0]).WithPayload(gs[1]).Build();
                    await cliente.PublishAsync(message);
                }
                MQTT_Helper.Pendiente.Clear();
                //pausa de 1.2s segundos para no bloquear el cpu, se puede reducir hasta .25s
                System.Threading.Thread.Sleep((int)System.TimeSpan.FromSeconds(1.2).TotalMilliseconds);
            }
            return 1;
        }
    }
}
