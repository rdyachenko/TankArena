using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TankBots_Serg
{
    public partial class frmLog: Form
    {
        private bool _Pause = false;
        private delegate void AddToLogCallback(string text);
        
        public frmLog()
        {
            InitializeComponent();
        }

        public void AddToLog(string text)
        {
            if (listBox1.InvokeRequired)
            {
                AddToLogCallback d = new AddToLogCallback(AddToLog);
                listBox1.Invoke(d, text);
            }   
            else
            {
                listBox1.Items.Insert(0, text);
                if (_Pause)
                {
                    MessageBox.Show("Pause");
                    _Pause = false;
                }
            }
        }
        
        public void Pause()
        {
            _Pause = true;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            _Pause = true;
        }
    }
}
