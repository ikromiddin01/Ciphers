

using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Text;
using ProjectCiphers.Ciphers;
using System.Numerics;
using System.Security.Cryptography;

namespace ProjectCiphers
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// Вертикальный перестановки
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        char[,] createTableWords(string sourceText, string key = "книга")
        {


            key = new string(key.Distinct().ToArray());
            int n;
            int m = key.Length;
            int k = 0;
            if (sourceText.Length % key.Length == 0)
            {
                n = sourceText.Length / key.Length + 1;

            }
            else
            {
                n = sourceText.Length / key.Length + 2;
            }
            char[,] tableWords = new char[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    tableWords[i, j] = ' ';
                }
            }
            for (int i = 0; i < m; i++)
            {
                tableWords[0, i] = key[i];
            }
            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (k == sourceText.Length) { break; }
                    tableWords[i, j] = sourceText[k];
                    k++;

                }


            }
            char[] keys = key.ToCharArray();


            return tableWords;
        }
   string VerticalTranspositionCipher(string sourceText, string key = "книга")
            {
    string uniqueKey = new string(key.Distinct().ToArray());
    char[] keys = uniqueKey.ToCharArray();
    Array.Sort(keys);
    string sortedKey = new string(keys);

    int columns = uniqueKey.Length;
    int rows = (int)Math.Ceiling((double)sourceText.Length / columns);
    char[,] table = new char[rows, columns];

    
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < columns; j++)
        {
            table[i, j] = ' ';
        }
    }

    
    int k = 0;
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < columns; j++)
        {
            if (k < sourceText.Length)
            {
                table[i, j] = sourceText[k++];
            }
        }
    }

    string cipherText = "";
    
    foreach (var ch in sortedKey)
    {
        int index = uniqueKey.IndexOf(ch);
        for (int i = 0; i < rows; i++)
        {
            cipherText += table[i, index];
        }
    }

    return cipherText;
}

        string VerticalTranspositionCipherDecryption(string cipherText, string key = "книга")
        {
            string sourceText = "";
            string uniqueKey = new string(key.Distinct().ToArray());

            char[] keys = uniqueKey.ToCharArray();
            Array.Sort(keys);
            string sortedKey = new string(keys);

            int columns = uniqueKey.Length;
            int rows = (int)Math.Ceiling((double)cipherText.Length / columns);
            char[,] table = new char[rows, columns];

            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    table[i, j] = ' ';
                }
            }

            int k = 0;
            foreach (var ch in sortedKey)
            {
                int index = uniqueKey.IndexOf(ch);
                for (int i = 0; i < rows; i++)
                {
                    if (k < cipherText.Length)
                    {
                        table[i, index] = cipherText[k++];
                    }
                }
            }

            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sourceText += table[i, j];
                }
            }

            return sourceText.Trim(); 
        }


        /// <summary>
        /// Вижинера
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public char getPlaceholdSymbol(string sourceText, char[,] tableUnicode)
        {
            for (int i = 0; i < tableUnicode.GetLength(0); i++)
            {
                for (int j = 0; j < tableUnicode.GetLength(1); j++)
                {
                    if (!sourceText.Contains(tableUnicode[i, j]))
                    {
                        return tableUnicode[i, j];
                    }
                }
            }
            return '\0'; 
        }
        public char[,] createMatrixUnicode(int n, string key)
        {
            char[,] tableUnicode = new char[n, n];
            HashSet<char> usedChars = new HashSet<char>();
            int x = 0, y = 0;

            for (int i = 0; i < key.Length; i++)
            {
                if (!usedChars.Contains(key[i]))
                {
                    tableUnicode[x, y] = key[i];
                    y++;
                    if (y >= 256)
                    {
                        y = 0;
                        x++;
                    }
                    usedChars.Add(key[i]);
                }
            }


            for (int i = 1; i <= n * n; i++)
            {
                if (!usedChars.Contains((char)i))
                {
                    tableUnicode[x, y] = (char)i;
                    y++;
                    if (y >= 256)
                    {
                        y = 0;
                        x++;
                    }
                }
            }
            return tableUnicode;
        }
        public string CipherPleyferEcryptio(string sourceText, string key = "student")
        {
            var ciphersText = "";
            int row_first_bi = 0;
            int col_first_bi = 0;
            int row_second_bi = 0;
            int col_second_bi = 0;
            int p;
            string bi = "";
            List<string> biGramm = new List<string>();
            int n = 256;
            char placeholdSymbol;
            char[,] tableUnicode = new char[n, n];

            tableUnicode = createMatrixUnicode(n, key);



 

            placeholdSymbol = '¡';//getPlaceholdSymbol(sourceText, tableUnicode);

            for (int i = 0; i < sourceText.Length - 1; i += 2)
            {
                bi = "";
                if (sourceText[i].ToString() != sourceText[i + 1].ToString())
                {
                    bi += sourceText[i].ToString() + sourceText[i + 1].ToString();
                    biGramm.Add(bi);
                }
                else
                {
                    bi += sourceText[i] + placeholdSymbol.ToString();
                    biGramm.Add(bi);
                    bi = "";
                    bi += sourceText[i + 1].ToString() + placeholdSymbol.ToString();
                    biGramm.Add(bi);

                }

            }
            if (sourceText.Length % 2 == 1)
            {

                bi = sourceText[sourceText.Length - 1].ToString() + placeholdSymbol.ToString();
                biGramm.Add(bi);
            }
            foreach (var b in biGramm)
            {

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (b[0].ToString() == tableUnicode[i, j].ToString())
                        {
                            row_first_bi = i;
                            col_first_bi = j;
                        }
                        if (b[1].ToString() == tableUnicode[i, j].ToString())
                        {
                            row_second_bi = i;
                            col_second_bi = j;

                        }
                    }

                }
                if (row_first_bi == row_second_bi && col_first_bi != col_second_bi)
                {

                    col_first_bi++;
                    col_second_bi++;
                    if (col_first_bi > n - 1) col_first_bi = 0;
                    if (col_second_bi > n - 1) col_second_bi = 0;

                }
                if (col_first_bi == col_second_bi && row_first_bi != row_second_bi)
                {
                    row_second_bi += 1;
                    row_first_bi += 1;
                    if (row_first_bi > n - 1) row_first_bi = 0;
                    if (row_second_bi > n - 1) row_second_bi = 0;

                }
                if ((row_first_bi != row_second_bi) && (col_first_bi != col_second_bi))
                {

                    (col_first_bi, col_second_bi) = (col_second_bi, col_first_bi);

                }
                string s1 = tableUnicode[row_first_bi, col_first_bi].ToString() + tableUnicode[row_second_bi, col_second_bi].ToString();

                ciphersText += s1;
            }

            return ciphersText;
        }
        public string CipherPleyferDecryptio(string sourceText, string key = "student")
        {
            var ciphersText = "";
            int row_first_bi = 0;
            int col_first_bi = 0;
            int row_second_bi = 0;
            int col_second_bi = 0;
            int p;
            string bi = "";
            List<string> biGramm = new List<string>();
            int n = 256;
            char placeholdSymbol;

            char[,] tableUnicode = new char[n, n];
            tableUnicode = createMatrixUnicode(n, key);

            placeholdSymbol = '¡';// getPlaceholdSymbol(sourceText, tableUnicode);

            for (int i = 0; i < sourceText.Length - 1; i += 2)
            {
                bi = "";
                if (sourceText[i].ToString() != sourceText[i + 1].ToString())
                {
                    bi += sourceText[i].ToString() + sourceText[i + 1].ToString();
                    biGramm.Add(bi);
                }
                else
                {
                    bi += sourceText[i] + placeholdSymbol.ToString();
                    biGramm.Add(bi);
                    bi = "";
                    bi += sourceText[i + 1].ToString() + placeholdSymbol.ToString();
                    biGramm.Add(bi);

                }

            }
            if (sourceText.Length % 2 == 1)
            {

                bi = sourceText[sourceText.Length - 1].ToString() + placeholdSymbol.ToString();
                biGramm.Add(bi);
            }
            foreach (var b in biGramm)
            {

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (b[0].ToString() == tableUnicode[i, j].ToString())
                        {
                            row_first_bi = i;
                            col_first_bi = j;
                        }
                        if (b[1].ToString() == tableUnicode[i, j].ToString())
                        {
                            row_second_bi = i;
                            col_second_bi = j;

                        }
                    }

                }
                if ((row_first_bi == row_second_bi))
                {

                    col_first_bi--;
                    col_second_bi--;
                    if (col_first_bi < 0) col_first_bi = n - 1;
                    if (col_second_bi < 0) col_second_bi = n - 1;
                   

                   
                }
                if ((col_first_bi == col_second_bi))
                {
                    row_second_bi--;
                    row_first_bi--;
                    if (row_first_bi < 0) row_first_bi = n - 1;
                    if (row_second_bi < 0) row_second_bi = n - 1;
                }
                if ((row_first_bi != row_second_bi) && (col_first_bi != col_second_bi))
                {

                    (col_first_bi, col_second_bi) = (col_second_bi, col_first_bi);

                }
                string s1 = tableUnicode[row_first_bi, col_first_bi].ToString() + tableUnicode[row_second_bi, col_second_bi].ToString();


                ciphersText += s1;
            }

            ciphersText = ciphersText.Replace(placeholdSymbol.ToString(), "");

            return ciphersText;
        }
        /// <summary>
        /// Вижинера
        /// </summary>
        /// <param name="sourceText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string CipherVijinerEcryption(string sourceText, string key = "key")
        {
            var ciphersText = new StringBuilder(); 
            int unicodeRange = 0x10000;

           
            while (key.Length < sourceText.Length)
            {
                key += key;
            }
            key = key.Substring(0, sourceText.Length); 

            for (int i = 0; i < sourceText.Length; i++)
            {
               
                int c = (sourceText[i] + key[i]-1) % unicodeRange;

                
                ciphersText.Append((char)c);
            }
            return ciphersText.ToString();

        }
        public string CipherVijinerDecryption(string sourceText, string key = "key")
        {
            var decryptedText = new StringBuilder();
            int unicodeRange = 0x10000; 

           
            while (key.Length < sourceText.Length)
            {
                key += key;
            }
            key = key.Substring(0, sourceText.Length);

            for (int i = 0; i < sourceText.Length; i++)
            {
              
                int sourceCharCode = sourceText[i];
                int keyCharCode = key[i];

                
                int decryptedCharCode = (sourceCharCode - keyCharCode +1) % 65536;

                
                decryptedText.Append((char)decryptedCharCode);
            }
            return decryptedText.ToString();

        }
        /// <summary>
        /// Шифр Цезаря
        /// </summary>

        public string CipherCaesarDecryption(string sourceText, int shift)
        {

            var cipherText = "";
            foreach (var s in sourceText)
            {
                int code = s; 
                int shiftedCode = code - shift;

                if (shiftedCode < 0)
                {
                    shiftedCode += 65536; 
                }

                cipherText += Convert.ToChar(shiftedCode);
            }
            return cipherText;
        }
        public string CipherCaesarEncryption(string sourceText, int shift)
        {
            var cipherText = "";
            foreach (var s in sourceText)
            {
                int code = s;
                int shiftedCode = code + shift;

                if (shiftedCode >= 65536)
                {
                   
                    shiftedCode -= 65536;
                }

                cipherText += Convert.ToChar(shiftedCode);
            }

            return cipherText;
        }
        public string CipherOtbashEncryption(string sourceText)
        {
            return new string (sourceText.Reverse().ToArray());
        }
        public string CipherOtbashDecryption(string encryptedText)
        {

            /* var text = encryptedText;
             text = new string(text.Reverse().ToArray());
             string sourceText = "";
             string txt = "";
             for (int i = 0; i < text.Length; i++)
             {
                 if (text[i] != ' ')
                 {
                     txt += text[i];
                 }
                 else
                 {
                     sourceText += new string(txt.Reverse().ToArray());
                     sourceText += text[i];
                     txt = "";
                 }

             }

             sourceText += new string(txt.Reverse().ToArray());*/

              return new string(encryptedText.Reverse().ToArray()); ;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox2.Text = CipherOtbashEncryption(richTextBox1.Text);
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    {
                        richTextBox2.Text = CipherOtbashEncryption(richTextBox1.Text);
                        //richTextBox3.Text = CipherOtbashDecryption(richTextBox2.Text);
                    }
                    break;
                case 1:
                    {
                        richTextBox2.Text = CipherCaesarEncryption(richTextBox1.Text, Int32.Parse(toolStripTextBox1.Text));
                        //richTextBox3.Text = CipherCaesarDecryption(richTextBox2.Text, Int32.Parse(toolStripTextBox1.Text));
                    }
                    break;
                case 2:
                    {
                        richTextBox2.Text = CipherVijinerEcryption(richTextBox1.Text, toolStripTextBox1.Text);
                        //richTextBox3.Text = CipherVijinerDecryption("YOWOZYNCSRHQN", "GOLANG");
                    }
                    break;
                case 3:
                    {
                        richTextBox2.Text = CipherPleyferEcryptio(richTextBox1.Text, toolStripTextBox1.Text);
                        //richTextBox3.Text = CipherPleyferDecryptio(richTextBox2.Text, toolStripTextBox1.Text);
                    }
                    break;

                case 5: {
                        List<char> text = richTextBox1.Text.ToList();
                        List<char> key = toolStripTextBox1.Text.ToList();
                        AlgebraMatrix algebraMatrix = new AlgebraMatrix();
                        algebraMatrix.Encrypt(key, text);
                        string t = new string(text.ToArray());
                        richTextBox2.Text = t.ToString();
                    }
                    break;
                case 4:
                    {
                        
                        richTextBox2.Text = VerticalTranspositionCipher(richTextBox1.Text, toolStripTextBox1.Text);
                    }
                    break;
                case 6:
                    {
                        
                        List<char> text = richTextBox1.Text.ToList();
                        List<char> key = toolStripTextBox1.Text.ToList();
                        Hill hill = new Hill();
                        var Km = hill.KeyToMatrix(key);
                        var det = hill.Determinant(Km);
                        try
                        {
                            if (det == 0) {
                                throw new Exception("Не подходит такой ключ!!");
                            }
                            hill.Encrypt(key, text);
                            string t = new string(text.ToArray());
                            
                            richTextBox2.Text = t.ToString();
                        }
                        catch (Exception ex) {
                            MessageBox.Show(ex.Message, "Ошибка");
                        }
                         

                        

                     

                    }
                    break;
                case 7:
                    {
                        Rsa rsa = new Rsa();
                        BigInteger p = BigInteger.Parse(toolStripTextBox3.Text);
                        BigInteger q = BigInteger.Parse(toolStripTextBox4.Text);
                        richTextBox2.Text = rsa.Encryption(richTextBox1.Text, p, q);
                    }
                    break;

                case 8:
                    {

                        DifiHelman difiHelman = new DifiHelman();

                        var Q = difiHelman.FindPrimitiveRoot(BigInteger.Parse(toolStripTextBox3.Text));
                        
                        var X1 = BigInteger.Parse(toolStripTextBox4.Text);
                        var X2 = BigInteger.Parse(toolStripTextBox2.Text);
                        var K = difiHelman.calculateK(BigInteger.Parse(toolStripTextBox3.Text), X1, X2);
                        MessageBox.Show(K.ToString());
                        MessageBox.Show(X1.ToString()+" " + X2.ToString());
                        if (X1 > Q || X2 > Q)
                        {
                            MessageBox.Show(X1 + " и " + X2 + " должно быть больше " + Q);
                        }
                        else
                        {

                            richTextBox2.Text = difiHelman.Encryption(
                                richTextBox1.Text,
                                BigInteger.Parse(toolStripTextBox3.Text),
                                BigInteger.Parse(toolStripTextBox4.Text),
                                BigInteger.Parse(toolStripTextBox2.Text)
                                );
                        }
                    }
                    break;
                case 9: {

                        string original = richTextBox1.Text ;
                        string key = toolStripTextBox1.Text;

                        using (DES des = DES.Create())
                        {

                            Des des1 = new Des();
                            des.Key = Encoding.UTF8.GetBytes(key);
                            des.Mode = CipherMode.ECB;
                            des.Padding = PaddingMode.PKCS7;

                            byte[] originalBytes = Encoding.Unicode.GetBytes(original);
                            byte[] encryptedBytes = des1.DesEncrypt(originalBytes, des);

                            
                            string encryptedBase64 = Convert.ToBase64String(encryptedBytes);
                           richTextBox2.Text = encryptedBase64;
                            // Console.WriteLine("Encrypted (Base64): " + encryptedBase64);

                           

                        }
                    }
                    break;
               
                   
            }


        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    {
                        // richTextBox2.Text = CipherOtbashEncryption(richTextBox1.Text);
                        richTextBox2.Text = CipherOtbashDecryption(richTextBox1.Text);
                    }
                    break;
                case 1:
                    {
                        //richTextBox2.Text = CipherCaesarEncryption(richTextBox1.Text, Int32.Parse(toolStripTextBox1.Text));
                        richTextBox2.Text = CipherCaesarDecryption(richTextBox1.Text, Int32.Parse(toolStripTextBox1.Text));
                    }
                    break;
                case 2:
                    {
                        //richTextBox2.Text = CipherVijinerEcryption(richTextBox1.Text, "GOLANG");
                        richTextBox2.Text = CipherVijinerDecryption(richTextBox1.Text, toolStripTextBox1.Text);
                    }
                    break;
                case 3:
                    {
                        //richTextBox2.Text = CipherPleyferEcryptio(richTextBox1.Text, toolStripTextBox1.Text);
                        richTextBox2.Text = CipherPleyferDecryptio(richTextBox1.Text, toolStripTextBox1.Text);
                    }
                    break;
                case 5: {
                        List<char> text = richTextBox1.Text.ToList();
                        MessageBox.Show(richTextBox1.Text);
                        List<char> key = toolStripTextBox1.Text.ToList();
                        AlgebraMatrix algebraMatrix = new AlgebraMatrix();
                        algebraMatrix.Decrypt( key, text);
                        string t = new string(text.ToArray());
                        richTextBox2.Text = t.ToString();
                        
                    } break;
                case 4:
                    {
                        richTextBox2.Text = VerticalTranspositionCipherDecryption(richTextBox1.Text, toolStripTextBox1.Text);
                    }
                    break;
                case 6:
                    {
                        List<char> text = richTextBox1.Text.ToList();
                        List<char> key = toolStripTextBox1.Text.ToList();
                        Hill hill = new Hill();
                        hill.Decrypt(key, text);
                        string t = new string(text.ToArray());
                        richTextBox2.Text = t.ToString();



                    }
                    break;
                case 8:
                    {
                        DifiHelman difiHelman = new DifiHelman();
                        var Q = difiHelman.FindPrimitiveRoot(BigInteger.Parse(toolStripTextBox3.Text));

                        var X1 = BigInteger.Parse(toolStripTextBox4.Text);
                        var X2 = BigInteger.Parse(toolStripTextBox2.Text);

                        if (X1 > Q || X2 > Q)
                        {
                            MessageBox.Show(X1 + " и " + X2 + " должно быть меньше " + Q);
                        }
                        else
                        {

                            richTextBox2.Text = difiHelman.Decryption(
                                richTextBox1.Text,
                                BigInteger.Parse(toolStripTextBox3.Text),
                                BigInteger.Parse(toolStripTextBox4.Text),
                                BigInteger.Parse(toolStripTextBox2.Text)
                                );
                        }
                    }
                    break;
                    case 7: {
                        Rsa rsa = new Rsa();
                        BigInteger p = BigInteger.Parse(toolStripTextBox3.Text);
                        BigInteger q = BigInteger.Parse(toolStripTextBox4.Text);
                        richTextBox2.Text = rsa.Decryption(richTextBox1.Text, p, q);
                    }break;
                case 9: {
                        string original = richTextBox1.Text;
                        string key = toolStripTextBox1.Text;

                        using (DES des = DES.Create())
                        {
                            Des des1 = new Des();
                            des.Key = Encoding.UTF8.GetBytes(key);
                            des.Mode = CipherMode.ECB;
                            des.Padding = PaddingMode.PKCS7;

                            byte[] originalBytes = Encoding.Unicode.GetBytes(original);
                            byte[] encryptedBytes = des1.DesEncrypt(originalBytes, des);

                            
                            string encryptedBase64 = Convert.ToBase64String(encryptedBytes);
                            Console.WriteLine("Encrypted (Base64): " + encryptedBase64);

                            
                            byte[] decryptedBytes =des1.DesDecrypt(encryptedBytes, des);
                            richTextBox2.Text =  Encoding.Unicode.GetString(decryptedBytes);

                        }
                    }
                    break;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox2.Text;
            richTextBox2.Text = " ";
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }
    }
}