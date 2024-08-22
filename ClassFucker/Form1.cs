using System;
using System.Diagnostics;

namespace ClassFucker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Scan();
            
            InitializeComponent();
        }
        public void Scan()
        {
            Process[] processes = Process.GetProcessesByName("hscagent");
        }
    }
}
