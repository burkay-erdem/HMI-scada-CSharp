using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ConsoleApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OFF.BringToFront();
            button5.BringToFront();
            button7.BringToFront();

        }


        private void OFF_Click(object sender, EventArgs e)
        {
            ON.BringToFront();
        }

        private void AUTO_Click(object sender, EventArgs e)
        {
            OFF.BringToFront();
        }

        private void ON_Click(object sender, EventArgs e)
        {
            AUTO.BringToFront();
        }
        int sayac = 0;
        private void Button1_Click(object sender, EventArgs e)
        {
            if (sayac > 0)
                sayac--;
            if (sayac < 10)
                maskedTextBox1.Text = "0" + sayac.ToString() + "0";
            else if (sayac <= 50 && sayac >= 10)
                maskedTextBox1.Text = sayac.ToString() + "0";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (sayac < 50)
                sayac++;
            if (sayac < 10)
                maskedTextBox1.Text = "0" + sayac.ToString() + "0";
            else if (sayac <= 50 && sayac >= 10)
                maskedTextBox1.Text = sayac.ToString() + "0";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            button8.BringToFront();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            button5.BringToFront();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            button7.BringToFront();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            button9.BringToFront();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(30);
                this.Invoke((MethodInvoker)delegate
              {
                  maskedTextBox1.Text = FBoxClientParameters.makinahız;
                  maskedTextBox2.Text = FBoxClientParameters.makinadevir;
                  maskedTextBox4.Text = FBoxClientParameters.setdeg;
                  maskedTextBox3.Text = FBoxClientParameters.anlıkdeg;
                  maskedTextBox6.Text = FBoxClientParameters.bekdk;
                  maskedTextBox5.Text = FBoxClientParameters.beksn;
                  maskedTextBox7.Text = FBoxClientParameters.bekdk;
                  maskedTextBox8.Text = FBoxClientParameters.beksn;
                  if (FBoxClientParameters.altlamb == "0")
                      button5.BringToFront();
                  else if (FBoxClientParameters.altlamb == "1")
                      button8.BringToFront();
                  if (FBoxClientParameters.üstlamb == "0")
                      button7.BringToFront();
                  else if (FBoxClientParameters.üstlamb == "1")
                      button9.BringToFront();
                  if (FBoxClientParameters.fan == "0")
                      OFF.BringToFront();
                  else if (FBoxClientParameters.fan == "1")
                      ON.BringToFront();
                  else if (FBoxClientParameters.fan == "2")
                      AUTO.BringToFront();
                  if (FBoxClientParameters.ipmud == "0")
                      button6.BringToFront();
                  else if (FBoxClientParameters.ipmud == "1")
                      button10.BringToFront();
               
              });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            
            
            Thread t = new Thread(new ThreadStart(this.TypewriteText));
            t.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }


        
        private void TypewriteText()
        {
            while (true)
            {
                Thread.Sleep(50);
                this.Invoke((MethodInvoker)delegate {
                    int LOC21 = label21.Location.X;
                    int LOC20 = label17.Location.X;
                    LOC21++;
                    LOC20++;
                    this.label21.Location = new System.Drawing.Point(LOC21, 0);
                    this.label20.Location = new System.Drawing.Point(LOC20, 0);


                });
                
            }
        }
    }
}
