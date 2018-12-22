using System;
using System.Collections.Generic;

namespace IT100_API
{
    public class Base_IoT{

        private IO USB = new IO();
        private MQTT_Helper mqtt = new MQTT_Helper();
        public List<string> IT100_Data { get; set; } = new List<string>();

        //esta clase "extiende" la clase base agregando las funciones IoT
        public void Init(string cfg) {
            USB.PortSelect();
            mqtt.Init(cfg);
            Logger.Init();
        }

        public void Init(string cfg, string port) {
            USB.Connect(port);
            mqtt.Init(cfg);
            Logger.Init();
        }
        public void Send(string command , string data){
            USB.SendCommand(command,data);
        }

        public List<string> Data(){
            if ( USB.Txt == null) {
                //String vacio, agrego linea en blanco para no tirar error
                IT100_Data.Add("-1 Sin Info");
            }else {
                IT100_Data = USB.Txt;
            }
            return IT100_Data;
        }

        public void Loop(){
            //primero sincronizo el IT100
            if ( !( USB.Txt == null ) ) {

                foreach ( string item in USB.Txt ) {
                    //aca clasifico los mensajes entrantes del IT100 y los dejo en los canales correspondientes
                    //por ahora hay 3 nomas, Particiones, Zonas y personalizado
                    // en el ultimo entran y salen los mensajes que no coincidan con comandos de particion o zonas
                    bool c1 = item.StartsWith("651") || item.StartsWith("650") ||
                              item.StartsWith("654") || item.StartsWith("655");

                    bool c2 = item.StartsWith("609") || item.StartsWith("610");

                    if ( c1 )
                        //Envio la wea
                        Console.WriteLine("Mensaje entrante del IT100: comando de particion");
                        mqtt.Send(item , "Particion"); //hacer que luego saque el canal del txt
                    if ( c2 )
                        //comandos de zonas:
                        Console.WriteLine("Mensaje entrante del IT100: comando de zonas");
                        mqtt.Send(item , "Zonas");
                    if ( !c1 && !c2 )
                        //va en personalizado:
                        Console.WriteLine("Mensaje entrante del IT100: comando personalizado");
                        mqtt.Send(item , "Personalizado");
                }
                USB.Clear();
            }
                //y ahora el MQTT
                foreach ( string item in mqtt.Get() ) {
                    //envio todo a lo nazi al IT100
                    if ( item != "" ) {
                        string[] gs = item.Split(',');
                        USB.Send(gs[1]);
                    }
                }
                mqtt.Clean(true);
                //listo, todo sicronizado y no exploto nada =)
            
        }
       
    }
}
