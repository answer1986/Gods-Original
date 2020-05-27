using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0lymp.us
{
    public partial class Captcha : Form
    {
        public Captcha()
        {
            InitializeComponent();
        }

        private void Captcha_Load(object sender, EventArgs e)
        {
            BtCaptcha.DialogResult = DialogResult.Yes;
        }
    }
}
