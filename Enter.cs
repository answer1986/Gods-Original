using _0lymp.us.Gods;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace _0lymp.us
{
    public partial class Enter : Form
    {
        public delegate void CCHandlrer(JObject response);

        private Iris iris;

        private const string ENTER = "/cerberus/enter.php";

        public int Id
        {
            get;
            private set;
        }

        public string License
        {
            get;
            private set;
        }

        public int Credits
        {
            get;
            private set;
        }

        public Enter()
        {
            InitializeComponent();
            LbVersion.Text = Program.VERSION;
            PbEnter.Hide();
            iris = new Iris();
        }

        //private void Enter_Load(object sender, EventArgs e)
        //{
        //    base.AcceptButton = BtnEnter;
        //}

        private void CommunicateCerberus()
        {
            iris.CommunicateComplete += CommunicateCerberusHandler;
            iris.url = Program.API + Program.VERSION + ENTER;
            iris.data = new NameValueCollection
            {
                ["license"] = TxtLicense.Text
            };
            iris.ExecuteThread();
        }

        protected void CommunicateCerberusHandler(JObject response)
        {
            BeginInvoke(new CCHandlrer(ComCerberusHandler), response);
            iris.KillThread();
        }

        public void ComCerberusHandler(JObject response)
        {
            PbEnter.Hide();
            string a = (string)response.SelectToken("status");
            string text = (string)response.SelectToken("message");
            if (a == "True")
            {
                LbInfoEnter.ForeColor = Color.FromArgb(192, 255, 192);
                LbInfoEnter.Text = "¡Tu licencia es correcta!";
                Id = (int)response.SelectToken("id");
                License = (string)response.SelectToken("license");
                Credits = (int)response.SelectToken("credits");
                Welcome welcome = new Welcome(this)
                {
                    License = License,
                    Credits = Credits
                };
                Hide();
                welcome.ShowDialog();
                Close();
            }
            else
            {
                LbInfoEnter.Text = "¡Tu licencia es incorrecta!,\n O No tiene suficientes créditos.";
            }
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            PbEnter.Show();
            LbInfoEnter.Text = "Conectando con el servidor...";
            CommunicateCerberus();
        }

        private void TxtLicense_KeyPress(object sender, KeyEventArgs e)
        {

        }


    }
}
