using _0lymp.us.Gods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace _0lymp.us
{
    public partial class Checker : Form
    {
        private Zeus zeus;
        private Poseidon poseidon;
        private Hades hades;
        private Hera hera;
        private Aphrodite aphrodite;
        private Ares ares;
        private Hermes hermes;
        private Athena athena;
        private Apollo apollo;
        private Artemis artemis;

        private Perses perses;
        private Iris iris;

        private const string REDUCTO = "/cerberus/reducto.php";
        private const string SAVE = "/tyche/save.php";
        private const string GATE = "/tartarus/get_gate.php";

        private int CounterLive;
        private int CounterDie;
        //private const Keys CutKeys = (Keys)131160;
        //private const Keys PasteKeys = (Keys)131158;
        //private Welcome oWelcome;

        public int Id { get; set; }
        public string License { get; set; }
        public string State { get; private set; }
        public int Credits { get; set; }
        public int CostLive { get; private set; }
        private int Gate { get; set; }
        private string Url { get; set; }

        public delegate void CCRHandler(JObject response);
        public delegate void CTSHandler(JObject response);
        public delegate void CTGHandler(JObject response);

        public delegate void GCHandler(string creditCards);

        public delegate void UCHandler(string cards);
        public delegate void LCHandler(string cards);
        public delegate void DCHandler(string cards);
        public delegate void VCHandler();

        private delegate void RPHandler(int percent, string message, string typeMessage);

        public Checker(Welcome welcome)
        {
            InitializeComponent();
            LbVersion.Text = Program.VERSION;
            PbLoadGate.Hide();
            PbLoadGenerateCards.Hide();
            PbLoadStart.Hide();
            PbLive.Hide();
            PbDie.Hide();

            zeus = new Zeus();
            poseidon = new Poseidon();
            hades = new Hades();
            hera = new Hera();
            aphrodite = new Aphrodite();
            ares = new Ares();
            hermes = new Hermes();
            athena = new Athena();
            apollo = new Apollo();
            artemis = new Artemis();

            iris = new Iris();
            perses = new Perses();

            License = welcome.License;
            Credits = welcome.Credits;
            CostLive = int.Parse("999");
            Gate = 9;
            if (Credits < 1)
            {
                BtnGenerateCards.Enabled = false;
                BtnStart.Enabled = false;
                BtnClearAll.Enabled = false;
                BtnStop.Enabled = false;
                MessageBox.Show("¡Gracias por apoyarnos!,\nTus créditos son insuficientes,\nPor favor, vuelva a recargar su licencia.", "¡Gracias!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            LblLincense.Text = "****" + welcome.License.Substring(4, welcome.License.Length - 4);
            LblCredits.Text = Credits.ToString();
            CounterLive = 0;
            CounterDie = 0;
        }

        private void Checker_Load(object sender, EventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();
            TbxLive.ContextMenu = contextMenu;
            TbxDie.ContextMenu = contextMenu;
            this.Activated += CheckerAfterLoad;

        }
        private void CheckerAfterLoad(object sender, EventArgs e)
        {
            this.Activated -= CheckerAfterLoad;

            if (DateTime.Parse("26/01/2020").Day > DateTime.Now.Day && DateTime.Parse("26/01/2020").Month == DateTime.Now.Month)
            {
                MessageBox.Show("Hasta el dia 25 de Enero, el costo por Live de todos los Gates sera de 2 a 1 Credito(s).\nOferta para recargas y ventas hasta con 50% Bonus.\nLas ofertas solo aplican para montos desde 13USD", "¡EVENTO RElAMPAGO!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            RbApollo.Checked = true;
            CommunicateTartarusGate();
        }

        private void BtnInfoBin_Click(object sender, EventArgs e)
        {
            GetBinInfo();
        }

        private void GetBinInfo()
        {
            CommunicateAuraGetBinInfo();
        }

        private void BtnGenerateCards_Click(object sender, EventArgs e)
        {
            GenerateCards();
        }

        private void GenerateCards()
        {
            perses.GenerateCardsComplete += GenerateCardsHandler;
            perses.bin = TbxBin.Text;
            perses.month = CbxMonth.Text;
            perses.year = CbxYear.Text;
            perses.cvv = TbxCvv.Text;
            perses.quantity = TbxQuantity.Text;
            BtnGenerateCards.Enabled = false;
            BtnStart.Enabled = false;
            BtnClearAll.Enabled = false;
            PbLoadGenerateCards.Show();
            perses.ExecuteThread();
        }

        protected void GenerateCardsHandler(string creditCards)
        {
            BeginInvoke(new GCHandler(GenCardsHandler), creditCards);
            perses.KillThread();
        }

        public void GenCardsHandler(string creditCards)
        {
            TbxResult.Text = creditCards;
            BtnGenerateCards.Enabled = true;
            BtnStart.Enabled = true;
            PbLoadGenerateCards.Hide();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            AThread();
            BtnGenerateCards.Enabled = false;
            BtnStart.Enabled = false;
            BtnClearAll.Enabled = false;
            PbLoadStart.Show();
            PbLive.Show();
            PbDie.Show();
        }

        protected void ReduceCardsHandler(string cards)
        {
            BeginInvoke(new UCHandler(RCardsHandler), cards);
            AThread(4);
        }

        protected void LiveCardHandler(string card)
        {
            BeginInvoke(new LCHandler(LCardHandler), card);
            AThread(5);
        }

        protected void DieCardHandler(string card)
        {
            BeginInvoke(new DCHandler(DCardHandler), card);
            AThread(6);
        }

        protected void VerifyCardHandler()
        {
            BeginInvoke(new VCHandler(VCardHandler), new object[0]);
            AThread(2);
        }

        protected void RefreshProgressHandler(int percent, string message, string typeMessage)
        {
            BeginInvoke(new RPHandler(RProgressHandler), percent, message, typeMessage);
            AThread(7);
        }

        private void RCardsHandler(string cards)
        {
            TbxResult.Text = cards;
        }

        private void LCardHandler(string card)
        {
            CommunicateCerberusReducto();
            TbxResult.Text = TbxResult.Text.Replace(card, "");
            TbxLive.Text += card;
            CounterLive++;
            LbCounterLive.Text = CounterLive.ToString();
            PlaySound();
            CommunicateTycheSave(card);
        }

        private void DCardHandler(string card)
        {
            TbxResult.Text = TbxResult.Text.Replace(card, "");
            TbxDie.Text += card;
            CounterDie++;
            LbCounterDie.Text = CounterDie.ToString();
            PlaySound("sound2");
        }

        private void VCardHandler()
        {
            AThread(3);
            BtnGenerateCards.Enabled = true;
            BtnStart.Enabled = true;
            BtnClearAll.Enabled = true;
            PbLoadStart.Hide();
            PbLive.Hide();
            PbDie.Hide();
        }

        private void RProgressHandler(int percent, string message, string typeMessage)
        {
            PbRefreshProcess.Value = percent;
            if (typeMessage == "success")
            {
                LbInfoProcess.ForeColor = Color.FromArgb(192, 255, 192);
            }
            else
            {
                //LbInfoProcess.ForeColor = Color.FromName();
            }
            LbInfoProcess.Text = message;
        }

        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            TbxResult.Text = "";
            TbxLive.Text = "";
            TbxDie.Text = "";
            LbCounterCC.Text = "0";
            LbCounterLive.Text = "0";
            LbCounterDie.Text = "0";
            CounterLive = 0;
            CounterDie = 0;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            AThread(3);
            BtnGenerateCards.Enabled = true;
            BtnStart.Enabled = true;
            BtnClearAll.Enabled = true;
            PbLoadGenerateCards.Hide();
            PbLoadStart.Hide();
            PbLive.Hide();
            PbDie.Hide();
            perses.KillThread();
            EndProcessTree("chromedriver.exe");
            EndProcessTree("conhost.exe");
        }

        private static void EndProcessTree(string process)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/im {process} /f /t",
                CreateNoWindow = true,
                UseShellExecute = false
            }).WaitForExit();
        }

        private void PlaySound(string sound="sound")
        {
            SoundPlayer soundPlayer = new SoundPlayer(sound+".wav");
            soundPlayer.Play();
        }

        private void CommunicateCerberusReducto()
        {
            iris.CommunicateComplete += CommunicateCerberusReductoHandler;
            iris.url = Program.API + Program.VERSION + REDUCTO;
            iris.data = new NameValueCollection
            {
                ["license"] = License,
                ["costLive"] = CostLive.ToString()
            };
            iris.ExecuteThread();
        }

        protected void CommunicateCerberusReductoHandler(JObject response)
        {
            BeginInvoke(new CCRHandler(ComCerberusReductoHandler), response);
            iris.KillThread();
        }

        public void ComCerberusReductoHandler(JObject response)
        {
            string a = (string)response.SelectToken("status");
            string text = (string)response.SelectToken("message");
            if (a == "True")
            {
                Credits = int.Parse((string)response.SelectToken("credits"));
                LblCredits.Text = Credits.ToString();
                bool flag = (Credits <= 0) ? true : false;
                bool flag2 = (Credits < CostLive) ? true : false;
                if (flag | flag2)
                {
                    AThread(3);
                    PbLoadStart.Hide();
                    PbLive.Hide();
                    PbDie.Hide();
                    BtnGenerateCards.Enabled = false;
                    BtnStart.Enabled = false;
                    BtnClearAll.Enabled = false;
                    BtnStop.Enabled = false;
                    if (flag)
                    {
                        text = "¡Gracias por apoyarnos!,\nPor favor, vuelva a recargar su licencia.";
                    }
                    else if (flag2)
                    {
                        text = "¡Gracias por apoyarnos!,\nTus créditos son insuficientes,\nPor favor, vuelva a recargar su licencia.";
                    }
                    MessageBox.Show(text, "¡Gracias!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show(text, "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CommunicateTycheSave(string cc)
        {
            iris.CommunicateComplete2 += CommunicateTycheSaveHandler;
            iris.url2 = Program.API + Program.VERSION + SAVE;
            iris.data2 = new NameValueCollection
            {
                ["licenseId"] = Id.ToString(),
                ["gateId"] = Gate.ToString(),
                ["cc"] = cc.Trim()
            };
            iris.ExecuteThread2();
        }

        protected void CommunicateTycheSaveHandler(JObject response)
        {
            BeginInvoke(new CTSHandler(ComTycheSaveHandler), response);
            iris.KillThread2();
        }

        public void ComTycheSaveHandler(JObject response)
        {
            string a = (string)response.SelectToken("status");
            string text = (string)response.SelectToken("exception");
            if (a == "False")
            {
                MessageBox.Show(text, "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CommunicateTartarusGate()
        {
            iris.CommunicateComplete3 += CommunicateTartarusGateHandler;
            iris.url3 = Program.API + Program.VERSION + GATE;
            iris.data3 = new NameValueCollection
            {
                ["gate"] = Gate.ToString()
            };
            iris.ExecuteThread3();
        }

        protected void CommunicateTartarusGateHandler(JObject response)
        {
            BeginInvoke(new CTGHandler(ComTartarusGateHandler), response);
            iris.KillThread3();
        }

        public void ComTartarusGateHandler(JObject response)
        {
            string a = (string)response.SelectToken("status");
            string text = (string)response.SelectToken("message");
            if (a == "True")
            {
                int num = (int)response.SelectToken("id");
                if (num == Gate)
                {
                    CostLive = (int)response.SelectToken("cost_live");
                    LblCostLive.Text = CostLive.ToString();
                    Url = (string)response.SelectToken("url");
                    PbLoadGate.Hide();
                    bool flag = (Credits <= 0) ? true : false;
                    bool flag2 = (Credits < CostLive) ? true : false;
                    if (flag | flag2)
                    {
                        BtnGenerateCards.Enabled = false;
                        BtnStart.Enabled = false;
                        BtnClearAll.Enabled = false;
                        BtnStop.Enabled = false;
                        if (flag)
                        {
                            text = "¡Gracias por apoyarnos! \nPor favor vuelva a recargar su licencia.";
                        }
                        else if (flag2)
                        {
                            text = "¡Gracias por apoyarnos! \nTus créditos son insuficientes. \nPor favor vuelva a recargar su licencia.";
                        }
                        MessageBox.Show(text, "¡Gracias!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        BtnStart.Enabled = true;
                        BtnGenerateCards.Enabled = true;
                        BtnStart.Enabled = true;
                        BtnClearAll.Enabled = true;
                        BtnStop.Enabled = true;
                    }
                }
                else
                {
                    RepairFailChecked(num);
                    MessageBox.Show("¡Lo sentimos! \nLa información del Gate Seleccionado, no se cargo correctamente.\nPor favor seleccione el Gate de nuevo.", "¡Advertencia!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show(text, "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CommunicateAuraGetBinInfo()
        {
            iris.CommunicateComplete4 += CommunicateAuraGetBinInfoHandler;
            iris.url4 = "https://lookup.binlist.net/" + TbxBin.Text;
            iris.data4 = new NameValueCollection
            {
                [""] = ""
            };
            iris.ExecuteThread4();
        }

        private void CommunicateAuraGetBinInfoHandler(JObject response)
        {
            BeginInvoke(new CTGHandler(ComAuraGetBinInfoHandler), response);
            iris.KillThread4();
        }

        public void ComAuraGetBinInfoHandler(JObject response)
        {
            try
            {
                string text = (string)response.SelectToken("scheme");
                string text2 = (string)response.SelectToken("type");
                JObject jObject = (JObject)response.SelectToken("country");
                string text3 = (string)jObject.SelectToken("name");
                JObject jObject2 = (JObject)response.SelectToken("bank");
                string text4 = (string)jObject2.SelectToken("name");
                MessageBox.Show("ESQUEMA: " + text + "\nTIPO: " + text2 + "\nPAIS: " + text3 + "\nBANCO: " + text4, "¡Info BIN!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Debe digitar almenos los primeros 6 digitos del BIN", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void RepairFailChecked(int idGate)
        {
            switch (idGate)
            {
                case 1:
                    RbZeus.Checked = true;
                    break;
                case 2:
                    RbPoseidon.Checked = true;
                    break;
                case 3:
                    RbHades.Checked = true;
                    break;
                case 4:
                    RbHera.Checked = true;
                    break;
                case 5:
                    RbAphrodite.Checked = true;
                    break;
                case 6:
                    RbAres.Checked = true;
                    break;
                case 7:
                    RbHermes.Checked = true;
                    break;
                case 8:
                    RbAthena.Checked = true;
                    break;
                case 9:
                    RbApollo.Checked = true;
                    break;
                case 10:
                    RbArtemis.Checked = true;
                    break;
                default:
                    RbApollo.Checked = true;
                    break;
            }
        }

        private void RbZeus_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbZeus.Checked, 1);
        }

        private void RbPoseidon_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbPoseidon.Checked, 2);
        }

        private void RbHades_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbHades.Checked, 3);
        }

        private void RbHera_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbHera.Checked, 4);
        }

        private void RbAphrodite_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbAphrodite.Checked, 5);
        }

        private void RbAres_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbAres.Checked, 6);
        }

        private void RbHermes_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbHermes.Checked, 7);
        }

        private void RbAthena_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbAthena.Checked, 8);
        }

        private void RbApollo_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbApollo.Checked, 9);
        }

        private void RbArtemis_CheckedChanged(object sender, EventArgs e)
        {
            IsCheckedGate(RbArtemis.Checked, 10);
        }

        private void IsCheckedGate(bool rbChecked, int gate)
        {
            if (rbChecked)
            {
                Gate = gate;
                switch (Gate)
                {
                    case 1:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises) \n 90% live Real para Sur America \n ¡Prohibido para Peru! ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 2:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises) \n 90% live Real para Sur America \n ¡Prohibido para Peru! .", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 3:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises). \n USAR VPN", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 4:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises).", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 5:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises). ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 6:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises). ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 7:
                        MessageBox.Show("MasterdCard (Todos los Paises). ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 8:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises). ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 9:
                        MessageBox.Show("Mc-Visa-Amex (Todos los Paises). ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                    case 10:
                        MessageBox.Show("Mc-Visa (Todos los Paises). ", ".::Información::.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        break;
                }
                PbLoadGate.Show();
                BtnStart.Enabled = false;
                CommunicateTartarusGate();
            }
        }

        private void AThread(int a = 1)
        {
            switch (Gate)
            {
                case 1:
                    switch (a)
                    {
                        case 1:
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            zeus.ReduceCardsComplete += ReduceCardsHandler;
                            zeus.LiveCardComplete += LiveCardHandler;
                            zeus.DieCardComplete += DieCardHandler;
                            zeus.VerifyCardsComplete += VerifyCardHandler;
                            zeus.RefreshProgressComplete += RefreshProgressHandler;
                            zeus.cardsList = TbxResult.Text.Split('\n').ToList();
                            zeus.Url = Url;
                            zeus.ExecuteThread();
                            break;
                        case 2:
                            zeus.KillThread();
                            break;
                        case 3:
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;

                            zeus.KillThread();
                            zeus.KillReduceCardsThread();
                            zeus.KillLiveCardThread();
                            zeus.KillDieCardThread();
                            break;
                        case 4:
                            zeus.KillReduceCardsThread();
                            break;
                        case 5:
                            zeus.KillLiveCardThread();
                            break;
                        case 6:
                            zeus.KillDieCardThread();
                            break;
                    }
                    break;
                case 2:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            poseidon.ReduceCardsComplete += ReduceCardsHandler;
                            poseidon.LiveCardComplete += LiveCardHandler;
                            poseidon.DieCardComplete += DieCardHandler;
                            poseidon.VerifyCardsComplete += VerifyCardHandler;
                            poseidon.RefreshProgressComplete += RefreshProgressHandler;
                            poseidon.CardsList = TbxResult.Text.Split('\n').ToList();
                            poseidon.Url = Url;
                            poseidon.ExecuteThread();
                            break;
                        case 2:
                            poseidon.KillThread();
                            poseidon.KillRefreshProgressThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;

                            poseidon.KillThread();
                            poseidon.KillReduceCardsThread();
                            poseidon.KillLiveCardThread();
                            poseidon.KillDieCardThread();
                            poseidon.KillRefreshProgressThread();
                            break;
                        case 4:
                            poseidon.KillReduceCardsThread();
                            break;
                        case 5:
                            poseidon.KillLiveCardThread();
                            break;
                        case 6:
                            poseidon.KillDieCardThread();
                            break;
                        case 7:
                            poseidon.KillRefreshProgressThread();
                            break;
                    }
                    break;
                case 3:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            hades.ReduceCardsComplete += ReduceCardsHandler;
                            hades.LiveCardComplete += LiveCardHandler;
                            hades.DieCardComplete += DieCardHandler;
                            hades.VerifyCardsComplete += VerifyCardHandler;
                            hades.RefreshProgressComplete += RefreshProgressHandler;
                            hades.cardsList = TbxResult.Text.Split('\n').ToList();
                            hades.url = Url;
                            hades.ExecuteThread();
                            break;
                        case 2:
                            hades.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;

                            hades.KillThread();
                            hades.KillReduceCardsThread();
                            hades.KillLiveCardThread();
                            hades.KillDieCardThread();
                            break;
                        case 4:
                            hades.KillReduceCardsThread();
                            break;
                        case 5:
                            hades.KillLiveCardThread();
                            break;
                        case 6:
                            hades.KillDieCardThread();
                            break;
                    }
                    break;
                case 4:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            hera.ReduceCardsComplete += ReduceCardsHandler;
                            hera.LiveCardComplete += LiveCardHandler;
                            hera.DieCardComplete += DieCardHandler;
                            hera.VerifyCardsComplete += VerifyCardHandler;
                            hera.RefreshProgressComplete += RefreshProgressHandler;
                            hera.CardsList = TbxResult.Text.Split('\n').ToList();
                            hera.Url = Url;
                            hera.ExecuteThread();
                            break;
                        case 2:
                            hades.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;

                            hera.KillThread();
                            hera.KillReduceCardsThread();
                            hera.KillLiveCardThread();
                            hera.KillDieCardThread();
                            break;
                        case 4:
                            hera.KillReduceCardsThread();
                            break;
                        case 5:
                            hera.KillLiveCardThread();
                            break;
                        case 6:
                            hera.KillDieCardThread();
                            break;
                    }
                    break;
                case 5:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            aphrodite.ReduceCardsComplete += ReduceCardsHandler;
                            aphrodite.LiveCardComplete += LiveCardHandler;
                            aphrodite.DieCardComplete += DieCardHandler;
                            aphrodite.VerifyCardsComplete += VerifyCardHandler;
                            aphrodite.RefreshProgressComplete += RefreshProgressHandler;
                            aphrodite.CardsList = TbxResult.Text.Split('\n').ToList();
                            aphrodite.Url = Url;
                            aphrodite.ExecuteThread();
                            break;
                        case 2:
                            aphrodite.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;

                            aphrodite.KillThread();
                            aphrodite.KillReduceCardsThread();
                            aphrodite.KillLiveCardThread();
                            aphrodite.KillDieCardThread();
                            break;
                        case 4:
                            aphrodite.KillReduceCardsThread();
                            break;
                        case 5:
                            aphrodite.KillLiveCardThread();
                            break;
                        case 6:
                            aphrodite.KillDieCardThread();
                            break;
                    }
                    break;
                case 6:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            ares.ReduceCardsComplete += ReduceCardsHandler;
                            ares.LiveCardComplete += LiveCardHandler;
                            ares.DieCardComplete += DieCardHandler;
                            ares.VerifyCardsComplete += VerifyCardHandler;
                            ares.RefreshProgressComplete += RefreshProgressHandler;
                            ares.CardsList = TbxResult.Text.Split('\n').ToList();
                            ares.Url = Url;
                            ares.ExecuteThread();
                            break;
                        case 2:
                            ares.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;

                            ares.KillThread();
                            ares.KillReduceCardsThread();
                            ares.KillLiveCardThread();
                            ares.KillDieCardThread();
                            break;
                        case 4:
                            ares.KillReduceCardsThread();
                            break;
                        case 5:
                            ares.KillLiveCardThread();
                            break;
                        case 6:
                            ares.KillDieCardThread();
                            break;
                    }
                    break;
                case 7:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            hermes.ReduceCardsComplete += ReduceCardsHandler;
                            hermes.LiveCardComplete += LiveCardHandler;
                            hermes.DieCardComplete += DieCardHandler;
                            hermes.VerifyCardsComplete += VerifyCardHandler;
                            hermes.RefreshProgressComplete += RefreshProgressHandler;
                            hermes.CardsList = TbxResult.Text.Split('\n').ToList();
                            hermes.Url = Url;
                            hermes.ExecuteThread();
                            break;
                        case 2:
                            hermes.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;
                            hermes.KillThread();
                            hermes.KillReduceCardsThread();
                            hermes.KillLiveCardThread();
                            hermes.KillDieCardThread();
                            break;
                        case 4:
                            hermes.KillReduceCardsThread();
                            break;
                        case 5:
                            hermes.KillLiveCardThread();
                            break;
                        case 6:
                            hermes.KillDieCardThread();
                            break;
                    }
                    break;
                case 8:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbHermes.Enabled = false;
                            LbInfoHermes.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            athena.ReduceCardsComplete += ReduceCardsHandler;
                            athena.LiveCardComplete += LiveCardHandler;
                            athena.DieCardComplete += DieCardHandler;
                            athena.VerifyCardsComplete += VerifyCardHandler;
                            athena.RefreshProgressComplete += RefreshProgressHandler;
                            athena.CardsList = TbxResult.Text.Split('\n').ToList();
                            athena.Url = Url;
                            athena.ExecuteThread();
                            break;
                        case 2:
                            athena.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbHermes.Enabled = true;
                            LbInfoHermes.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;
                            athena.KillThread();
                            athena.KillReduceCardsThread();
                            athena.KillLiveCardThread();
                            athena.KillDieCardThread();
                            break;
                        case 4:
                            athena.KillReduceCardsThread();
                            break;
                        case 5:
                            athena.KillLiveCardThread();
                            break;
                        case 6:
                            athena.KillDieCardThread();
                            break;
                    }
                    break;
                case 9:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbArtemis.Enabled = false;
                            LbInfoArtemis.Enabled = false;

                            apollo.ReduceCardsComplete += ReduceCardsHandler;
                            apollo.LiveCardComplete += LiveCardHandler;
                            apollo.DieCardComplete += DieCardHandler;
                            apollo.VerifyCardsComplete += VerifyCardHandler;
                            apollo.RefreshProgressComplete += RefreshProgressHandler;
                            apollo.CardsList = TbxResult.Text.Split('\n').ToList();
                            apollo.Url = Url;
                            apollo.ExecuteThread();
                            break;
                        case 2:
                            apollo.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbArtemis.Enabled = true;
                            LbInfoArtemis.Enabled = true;
                            apollo.KillThread();
                            apollo.KillReduceCardsThread();
                            apollo.KillLiveCardThread();
                            apollo.KillDieCardThread();
                            break;
                        case 4:
                            apollo.KillReduceCardsThread();
                            break;
                        case 5:
                            apollo.KillLiveCardThread();
                            break;
                        case 6:
                            apollo.KillDieCardThread();
                            break;
                    }
                    break;
                case 10:
                    switch (a)
                    {
                        case 1:
                            RbZeus.Enabled = false;
                            LbInfoZeus.Enabled = false;
                            RbPoseidon.Enabled = false;
                            LbInfoPoseidon.Enabled = false;
                            RbHades.Enabled = false;
                            LbInfoHades.Enabled = false;
                            RbHera.Enabled = false;
                            LbInfoHera.Enabled = false;
                            RbAphrodite.Enabled = false;
                            LbInfoAphrodite.Enabled = false;
                            RbAres.Enabled = false;
                            LbInfoAres.Enabled = false;
                            RbAthena.Enabled = false;
                            LbInfoAthena.Enabled = false;
                            RbApollo.Enabled = false;
                            LbInfoApollo.Enabled = false;

                            artemis.ReduceCardsComplete += ReduceCardsHandler;
                            artemis.LiveCardComplete += LiveCardHandler;
                            artemis.DieCardComplete += DieCardHandler;
                            artemis.VerifyCardsComplete += VerifyCardHandler;
                            artemis.RefreshProgressComplete += RefreshProgressHandler;
                            artemis.CardsList = TbxResult.Text.Split('\n').ToList();
                            artemis.Url = Url;
                            artemis.ExecuteThread();
                            break;
                        case 2:
                            artemis.KillThread();
                            break;
                        case 3:
                            RbZeus.Enabled = true;
                            LbInfoZeus.Enabled = true;
                            RbPoseidon.Enabled = true;
                            LbInfoPoseidon.Enabled = true;
                            RbHades.Enabled = true;
                            LbInfoHades.Enabled = true;
                            RbHera.Enabled = true;
                            LbInfoHera.Enabled = true;
                            RbAphrodite.Enabled = true;
                            LbInfoAphrodite.Enabled = true;
                            RbAres.Enabled = true;
                            LbInfoAres.Enabled = true;
                            RbAthena.Enabled = true;
                            LbInfoAthena.Enabled = true;
                            RbApollo.Enabled = true;
                            LbInfoApollo.Enabled = true;

                            artemis.KillThread();
                            artemis.KillReduceCardsThread();
                            artemis.KillLiveCardThread();
                            artemis.KillDieCardThread();
                            break;
                        case 4:
                            artemis.KillReduceCardsThread();
                            break;
                        case 5:
                            artemis.KillLiveCardThread();
                            break;
                        case 6:
                            artemis.KillDieCardThread();
                            break;
                    }
                    break;
            }
        }


        private void TbxResult_TextChanged(object sender, EventArgs e)
        {
            LbCounterCC.Text = TbxResult.Lines.Count().ToString();
        }

        private void PbFacebook_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.facebook.com/groups/2280862608909249");
        }

        private void PbTelegram_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/ChekerOlympus");
        }

        private void BtnGenerateCards_Click_1(object sender, EventArgs e)
        {
            GenerateCards();
        }

        private void BtnInfoBin_Click_1(object sender, EventArgs e)
        {
            GetBinInfo();
        }

        private void BtnStart_Click_1(object sender, EventArgs e)
        {
            Start();
        }

        private void BtnClearAll_Click_1(object sender, EventArgs e)
        {
            TbxResult.Text = "";
            TbxLive.Text = "";
            TbxDie.Text = "";
            LbCounterCC.Text = "0";
            LbCounterLive.Text = "0";
            LbCounterDie.Text = "0";
            CounterLive = 0;
            CounterDie = 0;
        }

        private void BtnStop_Click_1(object sender, EventArgs e)
        {
            Stop();
        }

        private static void CreditCard(KeyPressEventArgs V)
        {
            if (Char.IsDigit(V.KeyChar))
            {
                V.Handled = false;
            }
            else if (Char.IsSeparator(V.KeyChar))
            {
                V.Handled = false;
            }
            else if (Char.IsControl(V.KeyChar))
            {
                V.Handled = false;
            }else if (V.KeyChar.ToString().Equals("|"))
            {
                V.Handled = false;
            }
        }
    }
}
