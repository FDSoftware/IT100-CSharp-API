using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT100_API
{
    public class Base_IoT
    {
        IO USB = new IO();
        MQTT_Helper mqtt = new MQTT_Helper();
        //esta clase "extiende" la clase base agregando las funciones IoT
        public void Init() {
            USB.PortSelect();
            mqtt.Init("cfg.txt");
        }
        public void aux()
        {
            foreach ( string item in mqtt.Get() ) {
                if (item != "") { Console.WriteLine(item); }
            }
            mqtt.Clean(true);
        }
       public void Loop(){
            //primero sincronizo el IT100
            foreach (string item in USB.Get()){
                //aca clasifico los mensajes entrantes del IT100 y los dejo en los canales correspondientes
                //por ahora hay 3 nomas, Particiones, Zonas y personalizado
                // en el ultimo entran y salen los mensajes que no coincidan con comandos de particion o zonas
                bool c1 = item.StartsWith("651") || item.StartsWith("650") || item.StartsWith("654") || item.StartsWith("655");
                bool c2 = item.StartsWith("609") || item.StartsWith("610");
                 if (c1) {
                    //Envio la wea
                    Console.WriteLine("Mensaje entrante del IT100: comando de particion");
                    mqtt.Send(item, "Particion");
                }
                if (c2) {
                    //comandos de zonas:
                    Console.WriteLine("Mensaje entrante del IT100: comando de zonas");
                    mqtt.Send(item, "Zonas");
                }
                if (!c1 && !c2) {
                    //va en personalizado:
                    Console.WriteLine("Mensaje entrante del IT100: comando personalizado");
                    mqtt.Send(item, "Personalizado");
                }
                
            }
            USB.Clear();
            //y ahora el MQTT
            foreach ( string item in mqtt.Get() ) {
                //envio todo a lo nazi al IT100
                if (item != "") {
                    string[] gs = item.Split(',');
                    USB.Send(gs[1]);
                }
            }
            mqtt.Clean(true);
            //listo, todo sicronizado y no exploto nada =)
        }
       
    }
}
