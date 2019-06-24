using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Numerics;

namespace protect_inf_LR1
{
    public partial class Form1 : Form
    {
        char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                        'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 
                                                        'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                        'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                        '8', '9', '0' ,'.', 'A','B','C','D','E','F','G',
                                                        'H','I','J', 'K', 'L', 'M', 'N','O','P','Q','R',
                                                        'S', 'T','U','V', 'W','X','Y','Z',
        'а','б','в','г','д','е','ж','з','и','Й','к','л','н','о','р','с','т','у','ф','х','ч','ш','щ',
        'ь','ы','ъ','э','ю','я','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p'
            ,'q','r','s','t','u','v','w','x','y','z','\n','\r','-'};


        public Form1()
        {
            InitializeComponent();
        }

        private static long Global_e;
        private static long Global_d;

        private string FileName;

        //зашифровать
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            if ((textBox_p.Text.Length > 0) && (textBox_q.Text.Length > 0) && (fileTxtBox.Text.Length > 0))
            {
                try
                {
                    long p = Convert.ToInt64(textBox_p.Text);
                    long q = Convert.ToInt64(textBox_q.Text);
                    //long d = Convert.ToInt64(textBox_d.Text);
                    long e_ = Convert.ToInt64(textBox_n.Text);

                    if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                    {
                        string s = "";

                        //StreamReader sr = new StreamReader("in.txt");
                        StreamReader sr = new StreamReader(FileName);

                        while (!sr.EndOfStream)
                        {
                            s += sr.ReadLine();
                        }
                        string coding = sr.CurrentEncoding.ToString();
                        sr.Close();
                        
                        s = s.ToUpper();
                        //for(int i = 0; i < characters.Count(); i++)
                        //{
                        //    if(s == characters[i].ToString())
                        //    {

                        //    }
                        //}
                        //s = ReadFile().ToUpper();

                        long n = p * q;
                        long m = (p - 1) * (q - 1);
                        long d = Calculate_d(m);
                        //long e_ = Calculate_e(d, m);
                        Global_e = n; //Calculate_e(d, m);
                        //d = Calculate_d(m);
                        Global_d = Calculate_d(m);
                        e_ = Calculate_e(d, m);

                        List<string> result = RSA_Endoce(s, e_, n);

                        StreamWriter sw = new StreamWriter("out1.txt");
                        foreach (string item in result)
                            sw.WriteLine(item);
                        sw.Close();

                        textBox_d.Text = d.ToString();
                        //textBox_n.Text = n.ToString();
                        //textBox_n.Text = e_.ToString();

                        //Process.Start("out1.txt");
                        ReadLineToText();
                        OpenKeyEler(p, q);
                    }
                    else
                        MessageBox.Show("p или q - не простые числа!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Введите p, q, и имя файла!");
        }


        private string ReadFile()
        {
            string res = string.Empty;
            string encoding = string.Empty;
            string outStr = string.Empty;
            //чтение файла с ASCII
            Stream fs = new FileStream(FileName, FileMode.Open);
            using (StreamReader sr = new StreamReader(fs, true))
            {
                encoding = sr.CurrentEncoding.ToString();
            }
            
            if (encoding != System.Text.Encoding.UTF8.ToString())
            {
                byte[] B = System.IO.File.ReadAllBytes(FileName);
                outStr = System.Text.Encoding.Default.GetString(B);

                byte[] b = System.Text.Encoding.Default.GetBytes(outStr);
                outStr = System.Text.Encoding.Default.GetString(b);
            }


            return outStr;
        }

        private void OpenKeyEler(long p, long q)
        {
            long result = p * q;

            BigInteger a = default(BigInteger);
            BigInteger b = default(BigInteger);
            EuclidEx(p, q);
            // openKeyTxtbox.Text = result.ToString();
            label11.Text = result.ToString();
        }

        //расшифровать
        private void buttonDecipher_Click(object sender, EventArgs e)
        {
            if ((textBox_d.Text.Length > 0) && (textBox_n.Text.Length > 0))
            {
                string _n = Global_d.ToString();
                string _e = Global_e.ToString();
                long d =   Convert.ToInt64(_n); //Convert.ToInt64(textBox_d.Text); //
                long n =   Convert.ToInt64(_e);  //Convert.ToInt64(textBox_n.Text); //

                List<string> input = new List<string>();

                StreamReader sr = new StreamReader("out1.txt");
                //StreamReader sr = new StreamReader(FileName);

                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }

                sr.Close();

                string result = RSA_Dedoce(input, d, n);

                StreamWriter sw = new StreamWriter("out2.txt");
                sw.WriteLine(result);
                sw.Close();

                Process.Start("out2.txt");
                //ReadLineToText();
            }
            else
                MessageBox.Show("Введите секретный ключ!");
        }

        private void ReadLineToText()
        {
            listBox1.Items.Clear();
            string file = "out1.txt";
            //string file = FileName;
            StreamReader sr = new StreamReader(file);
            string line = "";
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            while (line != null)
            {
                line = sr.ReadLine();
                if (line != null)
                {
                    list.Add(line);
                    listBox1.Items.Add(line);
                }
            }
            sr.Close();
        }

        //проверка: простое ли число?
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        //зашифровать
        private List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)e);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }

        //расшифровать
        private string RSA_Dedoce(List<string> input, long d, long n)
        {

            string result = "";

            BigInteger bi;
            try
            {
                foreach (string item in input)
                {
                    bi = new BigInteger(Convert.ToDouble(item));
                    bi = BigInteger.Pow(bi, (int)d);

                    BigInteger n_ = new BigInteger((int)n);

                    bi = bi % n_;

                    int index = Convert.ToInt32(bi.ToString());

                    result += characters[index].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не верно задан ключ расшифровки");
                return result = null;
            }
            return result;
        }

        //вычисление параметра d. d должно быть взаимно простым с m
        private long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        //вычисление параметра e
        private long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            if(dial.ShowDialog() == DialogResult.OK)
            {
                string fileName = dial.FileName;
                fileTxtBox.Text = fileName;
                FileName = fileName;
                ReadFile();
            }
        }

        public BigInteger[] EuclidEx(BigInteger a, BigInteger b)
        {
            BigInteger d0 = a;
            BigInteger d1 = b;
            BigInteger x0 = BigInteger.Parse("1");
            BigInteger x1 = BigInteger.Parse("0");
            BigInteger y0 = BigInteger.Parse("0");
            BigInteger y1 = BigInteger.Parse("1");

            while (d1.CompareTo(BigInteger.Parse("1")) == 1)
            {
                BigInteger q = BigInteger.Divide(d0, d1);
                BigInteger d2 = d0 % d1;
                BigInteger x2 = BigInteger.Subtract(x0, BigInteger.Multiply(q, x1));
                BigInteger y2 = BigInteger.Subtract(y0, BigInteger.Multiply(q, y1));
                d0 = d1;
                d1 = d2;
                x0 = x1;
                x1 = x2;
                y0 = y1;
                y1 = y2;
            }
            BigInteger[] result = { x1, y1, d1 };
            if (x1 > 0)
            {
                //KOtxtBox.Text = x1.ToString();
                label10.Text = x1.ToString();
            }
            if (y1 > 0)
            {
                //KOtxtBox.Text = y1.ToString();
                label10.Text = y1.ToString();
            }
            return result;
        }

    }
}
