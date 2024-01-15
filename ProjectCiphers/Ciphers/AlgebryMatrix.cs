using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace ProjectCiphers.Ciphers
{
     class AlgebraMatrix

    {
        public List<List<int>> KeyToMatrix(List<char> key)
        {
            int x = (int)Math.Sqrt(key.Count);
            if ((x * x) != key.Count)
            {
                x++;
                for (int i = key.Count, y = 0; i < x * x; i++)
                    key.Add(key[y++]);
            }
            List<List<int>> keyMatrix = new List<List<int>>();
            for (int i = 0; i < x; i++)
            {
                List<int> vec = new List<int>();
                for (int j = 0; j < x; j++)
                    vec.Add((int)key[i * x + j]);
                keyMatrix.Add(vec);
            }
            return keyMatrix;
        }
        public void Encrypt(List<char> key, List<char> text)
        {
            char filler = '¡';
            List<List<int>> keyMatrix = KeyToMatrix(key);
            if (text.Contains(filler))
            {
                for (int i = 48; i < 65536; i++)
                {
                    if (!text.Contains((char)i))
                    {
                        filler = (char)i;
                        break;
                    }
                }
            }
            while ((text.Count % keyMatrix.Count) != 0)
            {
                text.Add(filler);
            }
            StringBuilder encrypted = new StringBuilder();
            for (int i = 0; i < text.Count; i += keyMatrix.Count)
            {
                for (int j = 0; j < keyMatrix.Count; ++j)
                {
                    int encryptedChar = 0;
                    for (int k = 0; k < keyMatrix.Count; ++k)
                    {
                        encryptedChar += keyMatrix[j][k] * text[i + k];
                    }
                    encrypted.Append((char)(encryptedChar));
                }
            }
            text.Clear();
            foreach (char c in encrypted.ToString().ToCharArray())
            {
                text.Add(c);
            }
        }

        public void Decrypt(List<char> key, List<char> text)
        {
            List<List<int>> keyMatrix = KeyToMatrix(key);
            Console.WriteLine(keyMatrix.ToString());
            List<List<double>> inverseKeyMatrix = InvertMatrix(keyMatrix);
            Console.WriteLine(inverseKeyMatrix.ToString());
            StringBuilder decrypted = new StringBuilder();
            for (int i = 0; i < text.Count; i += keyMatrix.Count)
            {
                for (int j = 0; j < keyMatrix.Count; ++j)
                {
                    double decryptedChar = 0;
                    for (int k = 0; k < keyMatrix.Count; ++k)
                        decryptedChar +=inverseKeyMatrix[j][k] * text[i + k];
                    decrypted.Append((char)(Math.Round(decryptedChar)));
                }
            }
            text.Clear();
            foreach (char c in decrypted.ToString().ToCharArray())
            {
                if (c != '¡')
                    text.Add(c);
            }

        }
        private List<List<double>> InvertMatrix(List<List<int>> matrix)
        {
            List<List<double>> result = new List<List<double>>();
            List<List<int>> algebraicComplementsMatrix = AlgebraicComplements(matrix);
           
            int det = (int)Determinant(matrix);
            for (int i = 0; i < matrix.Count; i++)
            {
                result.Add(new List<double>());
                for (int j = 0; j < matrix.Count; j++)
                    result[i].Add(algebraicComplementsMatrix[j][i]*(1.0/det));
            }
                        return result;
        }
        

       
        public double Determinant(List<List<int>> matrix)
        {
            double result = 0;
            if (matrix.Count == 1)
            {
                result = matrix[0][0];
                return result;
            }
            for (int i = 0; i < matrix[0].Count; i++)
            {
                List<List<int>> temp = new List<List<int>>();
                for (int j = 1; j < matrix.Count; j++)
                {
                    temp.Add(new List<int>());
                    for (int k = 0; k < matrix[0].Count; k++)
                    {
                        temp[j - 1].Add(0);
                        if (k < i)
                            temp[j - 1][k] = matrix[j][k];
                        else if (k > i)
                            temp[j - 1][k - 1] = matrix[j][k];
                    }
                }
                result += matrix[0][i] * Math.Pow(-1, i) * Determinant(temp);
            }
            return result;
        }
        public List<List<int>> AlgebraicComplements(List<List<int>> matrix)
        {
            List<List<int>> algebraicComplements = new List<List<int>>();
            for (int i = 0; i < matrix.Count; i++)
            {
                algebraicComplements.Add(new List<int>());
                for (int t = 0; t < matrix.Count; t++)
                    algebraicComplements[i].Add(0);
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    List<List<int>> temp = new List<List<int>>();
                    for (int k = 0; k < matrix.Count - 1; k++)
                    {
                        temp.Add(new List<int>());
                        for (int l = 0; l < matrix.Count - 1; l++)
                            temp[k].Add(0);
                    }
                    for (int k = 0; k < matrix.Count; k++)
                    {
                        for (int l = 0; l < matrix[k].Count; l++)
                        {
                            if (k != i && l != j)
                            {
                                int rowIndex = k < i ? k : k - 1;
                                int colIndex = l < j ? l : l - 1;
                                temp[rowIndex][colIndex] = matrix[k][l];
                            }
                        }
                    }
                    algebraicComplements[i][j] = (int)(Math.Pow(-1, i + j) * Determinant(temp));
                }
            }
            return algebraicComplements;
        }

    }

}
