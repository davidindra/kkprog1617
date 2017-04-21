using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace enigma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string[] file = System.IO.File.ReadAllLines(openFileDialog1.FileName);
                string[] tables = splitByCommas(file[0]);
                string[] inputStrings = file.Skip(1).ToArray();

                textBox1.Text = file[0];

                foreach (string inputString in inputStrings)
                {
                    if (String.IsNullOrWhiteSpace(inputString)) continue;

                    string inputString2 = inputString;
                    string outputString = "";
                    
                    foreach (string table in tables)
                    {
                        Dictionary<char, char> tableCharmap = readCharmap(System.IO.File.ReadAllLines(Environment.CurrentDirectory + "/tables/" + table)[0]);
                        
                        for(int i = 0; i < inputString2.Length; i++)
                        {
                            char translatedChar = ' ';
                            if (!tableCharmap.TryGetValue(inputString2[i], out translatedChar))
                            {
                                outputString += inputString2[i];
                            }
                            else
                            {
                                outputString += translatedChar;
                            }
                        }

                        //textBox1.Text += "\r\ndebug:" + outputString;

                        inputString2 = outputString;
                        outputString = "";
                    }

                    textBox1.Text += "\r\nVSTUP=" + inputString + "\r\nVYSTUP=" + inputString2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Při zpracování došlo k chybě. Chybová hláška: " + ex.Message);
            }
        }

        private string[] splitByCommas(string input)
        {
            string[] tmp = input.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            foreach (string item in tmp)
            {
                result.Add(item.Trim());
            }
            return result.ToArray();
        }

        private Dictionary<char, char> readCharmap(string input)
        {
            string[] charPairs = splitByCommas(input);

            Dictionary<char, char> result = new Dictionary<char, char>();

            foreach (string charPair in charPairs)
            {
                result.Add(charPair.First(), charPair.Last());
            }

            return result;
        }
    }
}
