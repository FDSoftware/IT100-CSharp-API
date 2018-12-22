using System.Collections.Generic;

namespace IT100_API
{

    public class Base{
        //con esta clase manejo toda la wea del it100
        private IO USB = new IO();
        public List<string> Data { get; set; }

        public void Init(){
            //void para iniciar la wea
            USB.PortSelect();
        }

        public bool Init(string port ) {
            return USB.Connect(port);
        }
 
        public void GetEstado() {
            //este void manda comando de estado, recuperar el mismo con la funcion getdata()
            USB.SendCommand("000" , "");
        }

        public void Send( string t1 , string t2 ){
            USB.SendCommand(t1 , t2);
        }

        public void Act(){
            //actualizo la lista con mensajes del IT100
            Data = USB.Txt;
        }
    }
}
