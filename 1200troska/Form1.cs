using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1200troska
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                char[,] city = loadCity(System.IO.File.ReadAllLines(Environment.CurrentDirectory + "/mesto.txt")); // read input file

                // clean output box
                textBox1.Text = "";
                string output = "";

                for (int b = 0; b < 100; b++) // show parsed city map
                {
                    for (int a = 0; a < 100; a++)
                    {
                        output += city[a, b]; // show that square
                    }

                    output += "\r\n"; // next row
                }

                textBox1.Text = output; // write it down
            }
            catch (Exception ex) // safety reasons
            {
                MessageBox.Show("Při zpracování nastala chyba. Chybová hláška: " + ex.Message);
            }
        }

        private char[,] loadCity(string[] inputFile) // parse input file into 2D array
        {
            char[,] result = new char[100, 100];
            for(int y = 0; y < 100; y++) // loop through the lines
            { 
                for(int x = 0; x < 100; x++) // and characters
                {
                    result[x, y] = (inputFile[y])[x]; // write into array
                }
            }

            return result; // return the result array
        }
    }
}
