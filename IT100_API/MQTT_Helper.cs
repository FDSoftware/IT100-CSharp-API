using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace IT100_API
{
    class MQTT_Helper
    {
        public List<string> Txt { get; set; } = new List<string>();
        public List<string> pendiente { get; set; } = new List<string>();

        public List<string> Get()
        {
            return Txt;
        }

        public void Clean(bool op)
        {
            if ( op )
                Txt.Clear();
            if ( !op )
                pendiente.Clear();
        }
        public async void Init( string cfg )//inicio el cliente mqtt
        {
            //cfg es la ruta del archivo con la configuracion(ip, canales, usuario, etc)
            //primero leo la cfg (no me diga :v)
            string linea = ""; //(linea del archivo no de merca)
            string[] data = {"Cliente" , "127.0.0.1" , "canal1","canal2","canal3"};
            int aux = 0;
            List<string> obs = new List<string>(); //Creo lista con los obstaculos
            System.IO.StreamReader file = new System.IO.StreamReader(@cfg);

            while ( ( linea = file.ReadLine() ) != null ) {
                string[] gs = linea.Split(',');
                data[aux] = gs[1];
                aux++;
            }
            MQTT_Worker(data);
            //worker.Start();
            //int x = await MQTT_Worker(data);
        }

        private async Task<int> MQTT_Worker(string[] data)
        {
            //weas para conectarme al mqtt y manejar los mensajes
            var factory = new MqttFactory();
            var cliente = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(data[0])
                .WithTcpServer(data[1])
                .Build();

            //me conecto
            Console.WriteLine("Conectando");
            await cliente.ConnectAsync(options);

            //me suscribo a los canales:
            Console.WriteLine("### Conectado al server ###");
            await cliente.SubscribeAsync(new TopicFilterBuilder().WithTopic(data[2]).Build());
            await cliente.SubscribeAsync(new TopicFilterBuilder().WithTopic(data[3]).Build());
            await cliente.SubscribeAsync(new TopicFilterBuilder().WithTopic(data[4]).Build());
            Console.WriteLine("### Subscrito a los canales ###");

            //evento para los mensajes
            cliente.ApplicationMessageReceived += ( s , e ) =>{
                Console.WriteLine("### Mensaje del Broker ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
              //Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
              //Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                                //el topi                       payload, mensaje y data pegados para enviar al it100
                Txt.Add(e.ApplicationMessage.Topic + "," + Encoding.UTF8.GetString(e.ApplicationMessage.Payload) );
            };

            cliente.Disconnected += async ( s , e ) =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try {
                    await cliente.ConnectAsync(options);
                }
                catch {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                    Console.WriteLine(":(");
                }
            };

            for ( ; ; ) {//este for es para sincronizar los mensajes
                    foreach ( string item in pendiente ) {
                            string[] gs = item.Split(',');
                            Console.WriteLine(gs[0]);
                            var message = new MqttApplicationMessageBuilder()
                                .WithTopic(gs[0])
                                //.WithTopic("canal1")
                                .WithPayload(gs[1])
                                .Build();
                            await cliente.PublishAsync(message);
                    }
                    pendiente.Clear();
                //pausa de 1.2s segundos para no bloquear el cpu, se puede reducir hasta .25s
                System.Threading.Thread.Sleep((int)System.TimeSpan.FromSeconds(1.2).TotalMilliseconds);
            }
            return 1;
        }
        public void Send(string payload, string canal)
        {   //con esta wea envio los mensajes del serie al mqtt
            pendiente.Add(canal + "," + payload);
        }
    }
}
