using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT100_API
{
    class IT100
    {
        //con esta clase manejo toda la wea del it100
        IO USB = new IO();

        public void init()
        {
            //void para iniciar la wea
            USB.PortSelect();
        }

        public void GetEstado()
        {
            //este void manda comando de estado, recuperar el mismo con la funcion getdata()
            USB.SendCommand("000" , "");
        }

        public void Send( string t1 , string t2 )
        {
            USB.SendCommand(t1 , t2);
        }

    }
}
