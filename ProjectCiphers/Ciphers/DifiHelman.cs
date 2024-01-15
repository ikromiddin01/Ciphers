using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCiphers.Ciphers
{
    class DifiHelman
    {




        public string Decryption(string sourceText, BigInteger A, BigInteger X1, BigInteger X2)
        {
            BigInteger K = calculateK(A, X1, X2);
            var cipherText = "";
            foreach (var s in sourceText)
            {
                int code = s; 
                int shiftedCode = code - (int)K;

                if (shiftedCode < 0)
                {
                    shiftedCode += 65536; 
                }

                cipherText += Convert.ToChar(shiftedCode);
            }
            return cipherText;
        }
        public string Encryption(string sourceText, BigInteger A, BigInteger X1, BigInteger X2)
        {
            BigInteger K = calculateK(A,X1,X2);
            var cipherText = "";
            foreach (var s in sourceText)
            {
                int code = s; 
                int shiftedCode = code + (int)K;

                if (shiftedCode >= 65536)
                {
                   
                    shiftedCode -= 65536;
                }

                cipherText += Convert.ToChar(shiftedCode);
            }

            return cipherText;
        }
      public  BigInteger calculateK(BigInteger A,BigInteger X1, BigInteger X2) {

            BigInteger Q = FindPrimitiveRoot(A);
            BigInteger Y1 =BigInteger.ModPow(A,X1,Q) ;
            BigInteger Y2 = BigInteger.ModPow(A, X2, Q);
            BigInteger K =  BigInteger.ModPow(Y1,X2,Q);
            return K;
        }
      public  BigInteger FindPrimitiveRoot(BigInteger A)
        {
            for (BigInteger i = A - 1; i >= 2; i--)
            {


                HashSet<BigInteger> generatedValues = new HashSet<BigInteger>();
                bool isPrimitiveRoot = true;
                for (BigInteger j = 1; j < i; j++)
                {
                    BigInteger value = BigInteger.ModPow(A, j, i);
                    if (generatedValues.Contains(value))
                    {
                        isPrimitiveRoot = false;
                        break;

                    }
                    generatedValues.Add(value);
                }
                if (isPrimitiveRoot) return i;
            }
            return 0;
        }

    }
}
