namespace IT100_API
{

    public class Base{
        //con esta clase manejo toda la wea del it100
        IO USB = new IO();

        public void Init()
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
