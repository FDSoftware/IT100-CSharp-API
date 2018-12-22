using System.Collections.Generic;
using System;
namespace IT100_API
{

    class MQTT_Helper{

        public static List<string> Txt { get; set; } = new List<string>();
        public static List<string> Pendiente { get; set; } = new List<string>();

        public List<string> Get() {
            return Txt;
        }

        public void Clean(bool op){
            if ( op )
                Txt.Clear();
            if ( !op )
                Pendiente.Clear();
        }
        public async void Init( string cfg ) {//inicio del cliente MQTT
            //cfg es la ruta del archivo con la configuracion(ip, canales, usuario, etc)
            string linea = ""; //(linea del archivo no de merca)
            string[] data = {"Cliente" , "127.0.0.1" , "canal1","canal2","canal3"};
            int aux = 0;
            try {
                System.IO.StreamReader file = new System.IO.StreamReader(@cfg);
                while ( ( linea = file.ReadLine() ) != null ) {
                    string[] gs = linea.Split(',');
                    data[aux] = gs[1];
                    aux++;
                }
            }
            catch {
                Console.WriteLine("No se encuentro el archivo de configuracion");
                Logger.Log("No se encontro el archivo de configuracion");
            }
            MQTT_Worker.Worker(data);
        }

        public void Send(string payload, string canal) {   
            //con esta wea envio los mensajes del serie al mqtt
            Pendiente.Add(canal + "," + payload);
        }
    }
}
