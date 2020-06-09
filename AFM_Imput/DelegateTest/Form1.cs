using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelegateTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int[] a = new int[10];
        public delegate bool Compare(int x, int y);
        public bool Sx(int x, int y)
        {
            return x > y;
        }
        public bool Jx(int x, int y)
        {
            return x < y;
        }
        public void SortArray(Compare compare)
        {
            for (int i = 0; i < a.Length; i++)
            {
                for(int j=i+1;j<a.Length;j++)
                {
                    if (compare(a[i], a[j]))
                    {
                        int t = a[i];
                        a[i] = a[j];
                        a[j] = t;
                    }
                   
                }
            }
        }
        public void display()
        {
            foreach (var num in a)
                textBox2.Text += num + "\r\n";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            Random rm = new Random();
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = rm.Next(0, 100);

                textBox1.Text += a[i] + "\r\n";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            Compare c = Sx;
            SortArray(c);
            display();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            Compare c = Jx;
            SortArray(c);
            display();
        }
    }
}
