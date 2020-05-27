using _0lymp.us.Gods;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace _0lymp.us
{
    public partial class Welcome : Form
    {
        public delegate void CTGYLHandler(JObject response);

        private Iris iris;
        private const string LIVES = "/tyche/get_your_lives.php";
        private Enter oEnter;
        private Checker checker;
        public int Id { get; set; }
        public string License { get; set; }
        public int Credits { get; set; }

        public Welcome(Enter enter)
        {
            oEnter = enter;
            InitializeComponent();
            LbVersion.Text = Program.VERSION;
            iris = new Iris();
            PbLive.Hide();
            Id = enter.Id;
            License = enter.License;
            Credits = enter.Credits;
            LblLincense.Text = "****" + oEnter.License.Substring(4, oEnter.License.Length - 4);
            LblCredits.Text = Credits.ToString();
        }

        public Welcome(Checker checker)
        {
            this.checker = checker;
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            PbLive.Show();
            CommunicateTycheGetYourLives();
        }

        private void CommunicateTycheGetYourLives()
        {
            iris.CommunicateComplete += CommunicateTycheGetYourLivesHandler;
            iris.url = Program.API + Program.VERSION + LIVES;
            iris.data = new NameValueCollection
            {
                ["license"] = License
            };
            iris.ExecuteThread();
        }

        protected void CommunicateTycheGetYourLivesHandler(JObject response)
        {
            BeginInvoke(new CTGYLHandler(ComTycheGetYourLivesHandler), response);
            iris.KillThread();
        }

        public void ComTycheGetYourLivesHandler(JObject response)
        {
            string text = (string)response.SelectToken("status");
            string text2 = (string)response.SelectToken("exception");
            dynamic val = response.SelectTokens("lives");
            string text3 = "";
            foreach (object item in val)
            {
                JToken jToken = (JToken)(dynamic)item;
                LbCounterLive.Text = jToken.Count().ToString();
                for (int i = 0; i < jToken.Count(); i++)
                {
                    text3 += jToken.SelectToken(i.ToString() + ".cc");
                    text3 += " - ";
                    text3 += jToken.SelectToken(i.ToString() + ".date");
                    text3 += " - ";
                    text3 += jToken.SelectToken(i.ToString() + ".name").ToString().ToUpper();
                    text3 += "\r\n";
                }
            }
            TbxLive.Text = text3;
            PbLive.Hide();
        }

        private void BtnGoToChecker_Click_1(object sender, EventArgs e)
        {
            Checker checker = new Checker(this)
            {
                Id = Id,
                License = License,
                Credits = Credits
            };
            Hide();
            checker.ShowDialog();
            Close();
        }
    }
}
