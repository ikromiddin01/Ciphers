using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCiphers.Ciphers
{
    class Rsa
    {
      private  BigInteger GenerateRandomCoPrime(BigInteger p, BigInteger q)
        {
            var n = (p - 1) * (q - 1);
            BigInteger e = 3;

            for (var i = n - 1; i >= 3; i--)
            {
                if (BigInteger.GreatestCommonDivisor(i, n) == 1)
                {
                    if (ModInverse(i, n) != i)
                    {
                        return i;
                    }
                }
            }

            return e;
        }
       public string Encryption(string text, BigInteger p, BigInteger q)
        {
            var n = p * q;
            var phi = (p - 1) * (q - 1);
            BigInteger e = GenerateRandomCoPrime(p, q);

            string encryptedText = "";
            foreach (var i in text)
            {
                int code = i;
                BigInteger c = BigInteger.ModPow(code, e, n);
                encryptedText += Convert.ToChar((int)c);
            }

            return encryptedText.Trim();
        }

       public string Decryption(string encryptedText, BigInteger p, BigInteger q)
        {
            var n = p * q;
            var phi = (p - 1) * (q - 1);
            BigInteger e = GenerateRandomCoPrime(p, q);
            var d = ModInverse(e, phi);

            string decryptedText = "";
            foreach (var block in encryptedText)
            {
                int c = block;
                BigInteger m = BigInteger.ModPow(c, d, n);
                decryptedText += Convert.ToChar((int)m);
            }

            return decryptedText;
        }

       public BigInteger ModInverse(BigInteger e, BigInteger phi)
        {
            BigInteger a = e, b = phi;
            BigInteger x = 0, y = 1, lastx = 1, lasty = 0, temp;
            while (b != 0)
            {
                BigInteger q = a / b;
                BigInteger r = a % b;

                a = b;
                b = r;

                temp = x;
                x = lastx - q * x;
                lastx = temp;

                temp = y;
                y = lasty - q * y;
                lasty = temp;
            }

            if (lastx < 0)
            {
                lastx += phi;
            }

            return lastx;
        }



    }
}
