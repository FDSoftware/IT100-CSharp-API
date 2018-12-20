using System;
using System.Linq;

namespace IT100_API
{
    class Helper
    {
        //para no hacer cagada dejo aca algunas cosas generales
        public static int DecToHex( int num )
        {
            //funcion de mierda para pasar de decimal a hexadecimal
            int quotient = num;
            int i = 1, j, temp = 0;
            char[] hexadecimalNumber = new char[100];
            char temp1;
            while ( quotient != 0 ) {
                temp = quotient % 16;

                if ( temp < 10 )
                    temp = temp + 48;
                else
                    temp = temp + 55;

                temp1 = Convert.ToChar(temp);
                hexadecimalNumber[i++] = temp1;
                quotient = quotient / 16;
            }
            string fin = "";
            for ( j = i - 1; j > 0; j-- ) {
                fin += hexadecimalNumber[j];
            }
            return Convert.ToInt32(fin);
        }

        public static byte[] Combine( params byte[][] arrays )
        {

            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;

            foreach ( byte[] array in arrays ) {
                System.Buffer.BlockCopy(array , 0 , rv , offset , array.Length);
                offset += array.Length;
            }

            return rv;
        }
    }
}
