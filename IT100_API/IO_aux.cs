using System;

namespace IT100_API
{
    class IO_aux{
        //clase auxiliar para manejar los mensajes entrantes del IT100
        public static void Update(string input){
            //con este void separo todoos los codigos del IT100
            string data = input.Remove(0, 3);
             if (input.StartsWith("908")){
                Console.WriteLine("Version Alarma DSC: {0}", data);
            }
            if (input.StartsWith("650")){
                Console.WriteLine("Particion Lista: {0}", data);
            }
            if (input.StartsWith("651")) {
                Console.WriteLine("Particion no disponible: {0}", data);
            }
            if (input.StartsWith("654")){
                Console.WriteLine("Particion en alarma: {0}", data);
            }
            if (input.StartsWith("655")){
                Console.WriteLine("Particion desarmada: {0}", data);
            }
            if (input.StartsWith("609")) {
                Console.WriteLine("Zona abierta como tu vieja: {0}", data);
            }
            if (input.StartsWith("610")){
                Console.WriteLine("Zona cerrada: {0}", data);
            }
        }
    }
}
