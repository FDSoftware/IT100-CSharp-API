using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace IT100_API
{
    class IO
    {
        SerialPort USB = new SerialPort();
        public List<string> Txt { get; set; }

        public bool Connect(string puerto2){
            try {
                USB.PortName = puerto2;
                USB.BaudRate = 9600; //la veloidad siempre queda fija
                USB.DataReceived += new SerialDataReceivedEventHandler(Data);
                USB.Open();
                return true;
            }catch {
                Logger.Log("### Error al conectarse al IT100 ###");
                Environment.Exit(-1);
                return false;
            }
        }

        public void Send(string p){
            //envio la wea que me mandan, nada mas
            Console.WriteLine("### Enviando comando al IT100 ###");
            USB.Write(GetPayLoad(p , "")+ "\r\n");
            Console.WriteLine(" ### Listo ###");
        }

        public void SendCommand(string command, string data) {
            //Segun el manual, es algo asi:
            //Commando + Data + CheckSumm + CL / RF ()
            byte[] array = GetPayLoad(command, data);
            USB.Write(array, 0, array.Length);
        }

        public List<string> Ports(){
            List<string> obs = new List<string>();
            foreach (string s in SerialPort.GetPortNames()){
                obs.Add(s); //Agrego un obstaculo a la lista
            }
            return obs;
        }

        public void PortSelect() {
            bool cont = false;
            do{
                Console.Clear();
                Console.WriteLine("Lista de Puertos:");
                IO serie = new IO();
                int portnum = 0;
                List<string> puertos = new List<string>();
                puertos = Ports();
                foreach (string d in puertos){
                    Console.Write("{0}) ", portnum);
                    portnum++;
                    Console.WriteLine(d);
                }
                Console.Write("{0})", (portnum + 1));
                Console.WriteLine("Actualizar");
                string key = Console.ReadLine();
                try {
                    if ( Convert.ToInt32(key) != ( portnum + 1 ) ) {
                        Connect(Convert.ToString(puertos[Convert.ToInt32(key)]));
                        cont = false;
                        break;
                    }
                    else
                        cont = true;
                }
                catch {
                    Console.WriteLine("Puerto ingresado no valido");
                    Console.ReadKey();
                    cont = true;
                }
                
            } while (cont != false);
        }

        public void Clear() { Txt.Clear(); } //tambien se puede usar (objeto).Txt.Clear(); capaz que lo borre

        public void Data(object sender, SerialDataReceivedEventArgs e) {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.Write("Data Received:");
            //Console.WriteLine(indata);
            Txt.Add(indata);
            IO_aux.Update(indata);
        }

        public static string CHKS(string s){
            /* Teoricamente:
            sumo todos los byte en hexa, y el resultado lo mando como ascii:
            0x36 + 0x35 + 0x34 + 0x33 = 0xD2 */
            byte[] array = Encoding.ASCII.GetBytes(s);
            int chks = 0;
            foreach (byte d in array){
                //ahora que tengo cada byte en decimal, lo sumo
                chks += d;
            }
            string outputHex = Convert.ToString(int.Parse(chks.ToString()), 16);
            Console.WriteLine("### Checksum : {0} (hexadecimal) ###" , outputHex);
            return outputHex;
        }

        public static byte[] GetPayLoad(string msg, string data){
            /* esta toda la magia ?), con esta funcion creo el array 
            gigante de bytes a enviar para el IT100 */
            return Helper.Combine(Encoding.ASCII.GetBytes(msg + data), Encoding.ASCII.GetBytes(CHKS(msg + data)), Encoding.ASCII.GetBytes("\r\n"));
        }
    }
}
