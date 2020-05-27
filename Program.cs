using System;
using System.Threading;
using System.Windows.Forms;

namespace _0lymp.us
{
    static class Program
    {
        public const string API = "https://www.0lymp.us/api/";
        public const string VERSION = "1.6.0";
        private static Mutex mutex;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
         //mutex = new Mutex(initiallyOwned: true, "MyApplicationMutex", out bool createdNew);
         //if (createdNew)
         //{
         Application.Run(new Enter());
            //
            //else
            //{
            //MessageBox.Show("0lymp.us " + VERSION + " ya se esta ejecutando.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
    }

}