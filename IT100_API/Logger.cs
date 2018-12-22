using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IT100_API {

    class Logger {
        public static string nombre = ""; //nombre del archivo de log
        //logger para debug /control de errores
        public static void Init(){
            //aca creo el archivo para el log
            DateTime dateTime = new DateTime();
            dateTime = DateTime.Now;
            nombre = "log " + Convert.ToDateTime(dateTime).ToString("MM-dd") + ".txt";
            if ( !System.IO.File.Exists(nombre) )
                File.Create(nombre);
        }

        public static void Log(string txt){
            try {
                DateTime dateTime = new DateTime();
                dateTime = DateTime.Now;
                StreamWriter sw = new StreamWriter(nombre);
                sw.WriteLine(txt + "  " + Convert.ToDateTime(dateTime).ToString("hh:mm:ss"));
                sw.Close();
            }
            catch ( Exception e ) {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
