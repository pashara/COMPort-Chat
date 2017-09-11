using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SPort
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            chatTextBox.ReadOnly = true;
            this.openChat(false);
        }
        
        

        private void openChat(bool isOpen)
        {
            if(isOpen == true)
            {
                usernameTextBox.ReadOnly = true;
                messageTextBox.ReadOnly = false;
                sendButton.Enabled = true;
                button1.Enabled = false;
                
            }
            else
            {
                usernameTextBox.ReadOnly = false;
                messageTextBox.ReadOnly = true;
                sendButton.Enabled = false;
                button1.Enabled = true;
            }
        }
        
        private string generateStringMessabeFromString(string PORT, string message)
        {
            string time = DateTime.Now.ToShortTimeString();
            return PORT + ":" + time+":"+message;
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[serialPort1.BytesToRead];
            serialPort1.Read(data, 0, data.Length);
            chatTextBox.Text += System.Text.Encoding.UTF8.GetString(data);
            chatTextBox.Text += Environment.NewLine;
        }

        void serialPort_SendData(byte[] data)
        {
            serialPort1.RtsEnable = true;
            serialPort1.Write(data, 0, data.Length);
            Thread.Sleep(100);
            // пауза для корректного завершения работы передатчика  
            serialPort1.RtsEnable = false;
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            
            //serialPort1 = new System.IO.Ports.SerialPort(usernameTextBox.Text, 9600, Parity.None, 8, StopBits.One);

            serialPort1.PortName = usernameTextBox.Text; //Указываем наш порт - в данном случае COM1.
            serialPort1.BaudRate = Int32.Parse(speedList.Text); //указываем скорость.
            serialPort1.DataBits = 8;

            serialPort1.ReadTimeout = 500;
            serialPort1.WriteTimeout = 500;
            serialPort1.Open(); //Открываем порт.   

            //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort1.DataReceived += serialPort_DataReceived;


            if (serialPort1.IsOpen)
            {
                 this.openChat(true);
                 this.Text = programName + ":" + usernameTextBox.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close(); //Закрываем порт.

            this.Text = programName;
            this.openChat(false);
        }
        

        private void sendButton_Click(object sender, EventArgs e)
        {
            serialPort_SendData(Encoding.UTF8.GetBytes(generateStringMessabeFromString(usernameTextBox.Text, messageTextBox.Text)));

            chatTextBox.Text += generateStringMessabeFromString(usernameTextBox.Text, messageTextBox.Text);
            chatTextBox.Text += Environment.NewLine;
            messageTextBox.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

