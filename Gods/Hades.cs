using Bogus;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace _0lymp.us.Gods
{
    public partial class Hades : Component
    {
        private static IWebDriver hades;
        private static IWebDriver message;
        private static IWebDriver hadesAlt;

        public List<string> cardsList;
        public string url;
        private Faker faker;


        public Thread VerifyCardsThread;
        public Thread StateCardsThread;
        public Thread ReduceCardsThread;
        public Thread RefreshProgressThread;

        public delegate void VerifyCardsCompleteHandler();
        public delegate void StateCardsCompleteHandler(string card);
        public delegate void ReduceCardsCompleteHandler(string cards);
        public delegate void RefreshProgressCompleteHandler(int percent, string message, string typeMessage);

        public event VerifyCardsCompleteHandler VerifyCardsComplete;
        public event StateCardsCompleteHandler LiveCardComplete;
        public event StateCardsCompleteHandler DieCardComplete;
        public event ReduceCardsCompleteHandler ReduceCardsComplete;
        public event RefreshProgressCompleteHandler RefreshProgressComplete;

        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string Address { get; set; }
        private string Address2 { get; set; }
        private string City { get; set; }
        private string State { get; set; }
        private string Zip { get; set; }
        private string Phone { get; set; }
        private string VerificationCode { get; set; }
        private ResolveCaptcha Captcha { get; set; }
        private string ImageCaptchaResolve { get; set; }
        public Hades()
        {
            InitializeComponent();
            faker = new Faker();
            Captcha = new ResolveCaptcha();
        }

        public Hades(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void VerifyCards()
        {
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            UserName = faker.Internet.UserName();
            Password = faker.Internet.Password();
            Address = faker.Address.StreetAddress();
            Address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : faker.Address.SecondaryAddress());
            City = faker.Address.City();
            State = faker.Address.State();
            Zip = faker.Address.ZipCode();
            Phone = faker.Phone.PhoneNumber();
            Load();
            hades.Kill();
            message.Kill();
            ExecuteRefreshProgressThread(0, "", "Success");
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando HADES...", "Success");
                hades = Driver.Chrome();
                hades.GoUrl(url);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement anchorSignUp = hades.ElementVisible(By.Id("signup"), "ExceptionLoad");
            if (anchorSignUp.IsNotNull())
            {
                GoToSignUp(anchorSignUp);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR HADES, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void GoToSignUp(IWebElement anchorSignUp)
        {
            try
            {
                ExecuteRefreshProgressThread(5, "HADES Cargado...", "Success");
                anchorSignUp.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement anchorEasiSwitch = hades.ElementVisible(By.Id("easiSwitch"), "ExceptionGoToSignUp");
            if (anchorEasiSwitch.IsNotNull())
            {
                Message();
                SignUp(anchorEasiSwitch);
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR HADES", "Success");
                Restart();
            }
        }

        private void SignUp(IWebElement anchorEasiSwitch)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Accediendo...", "Success");
                anchorEasiSwitch.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement inputMemberName = hades.ElementVisible(By.Id("MemberName"), "ExceptionSignUp");
            if (inputMemberName.IsNotNull())
            {
                SetMemberName(inputMemberName);
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR Accediendo, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void SetMemberName(IWebElement inputMemberName)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Configurando Correo", "Success");
                inputMemberName.SendKeys(Email);
                hades.FindElement(By.Id("iSignupAction")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement inputPassword = hades.ElementVisible(By.Id("PasswordInput"), "ExceptionMemberName");
            if (inputPassword.IsNotNull())
            {
                SetPassword(inputPassword);
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR Configurando Correo", "Success");
                Restart();
            }
        }

        private void SetPassword(IWebElement inputPassword)
        {
            try
            {
                ExecuteRefreshProgressThread(25, "Configurando contraseña", "Success");
                inputPassword.SendKeys(Password);
                hades.FindElement(By.Id("iSignupAction")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement inputFirstName = hades.ElementVisible(By.Id("FirstName"), "ExceptionMemberName");
            if (inputPassword.IsNotNull())
            {
                SetName(inputFirstName);
            }
            else
            {
                ExecuteRefreshProgressThread(25, "ERROR Configurando contraseña, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void SetName(IWebElement inputFirstName)
        {
            try
            {
                ExecuteRefreshProgressThread(30, "Configurando Nombres", "Success");
                inputFirstName.SendKeys(FirstName);
                hades.FindElement(By.Id("LastName")).SendKeys(LastName);
                hades.FindElement(By.Id("iSignupAction")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement inputVerificationCode = hades.ElementVisible(By.Id("VerificationCode"), "ExceptionMemberName");
            if (inputVerificationCode.IsNotNull())
            {
                IWebElement tableDataSubject = message.ElementVisible(By.CssSelector("#maillists > tr > td:nth-child(3)"), "ExceptionRegisterNotFoundEmail", 20);
                if (tableDataSubject.IsNotNull())
                {
                    GoToReadEmail(tableDataSubject);
                    SetVerificationCode(inputVerificationCode);
                }
                else
                {
                    ExecuteRefreshProgressThread(30, "ERROR Configurando Nombres", "Success");
                    Restart();
                }
            }
            else
            {
                IWebElement SelectCountry = hades.ElementVisible(By.Id("Country"), "ExceptionSignUp");
                if (SelectCountry.IsNotNull())
                {
                    SetBirthDate(SelectCountry);
                }
                else
                {
                    ExecuteRefreshProgressThread(30, "ERROR Configurando Nombres", "Success");
                    Restart();
                }
            }
        }

        private void SetBirthDate(IWebElement SelectCountry)
        {
            try
            {
                ExecuteRefreshProgressThread(35, "Configurando fecha de nacimiento", "Success");
                SelectElement selectCountry = new SelectElement(SelectCountry);
                int maxValue = selectCountry.Options.Count();
                selectCountry.SelectByIndex(new Random().Next(1, maxValue));
                Thread.Sleep(1000);
                SelectElement selectBirthDay = new SelectElement(hades.FindElement(By.Id("BirthDay")));
                int maxDay = selectBirthDay.Options.Count();
                selectBirthDay.SelectByIndex(new Random().Next(1, maxDay));
                Thread.Sleep(1000);
                SelectElement selectBirthMonth = new SelectElement(hades.FindElement(By.Id("BirthMonth")));
                int maxMonth = selectBirthMonth.Options.Count();
                selectBirthMonth.SelectByIndex(new Random().Next(1, maxMonth));
                Thread.Sleep(1000);
                SelectElement selectBirthYear = new SelectElement(hades.FindElement(By.Id("BirthYear")));
                string year = new Random().Next(1970, 2000).ToString();
                selectBirthYear.SelectByValue(year);
                Thread.Sleep(1000);
                hades.FindElement(By.Id("iSignupAction")).Click();
            }
            catch (NoSuchElementException) { /**/ }
            catch (StaleElementReferenceException) { /**/ }
            catch (WebDriverException) { /**/ }
            catch (NullReferenceException) { /**/ }

            IWebElement inputVerificationCode = hades.ElementVisible(By.Id("VerificationCode"), "ExceptionMemberName");
            if (inputVerificationCode.IsNotNull())
            {
                IWebElement tableDataSubject = message.ElementVisible(By.CssSelector("#maillists > tr > td:nth-child(3)"), "ExceptionRegisterNotFoundEmail", 30);
                if (tableDataSubject.IsNotNull())
                {
                    GoToReadEmail(tableDataSubject);
                    SetVerificationCode(inputVerificationCode);
                }
                else
                {
                    ExecuteRefreshProgressThread(35, "ERROR  Configurando fecha de nacimiento, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }

        private void SetVerificationCode(IWebElement inputVerificationCode)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Codigo de verificacion extraido...", "Success");
                inputVerificationCode.SendKeys(VerificationCode);
                hades.FindElement(By.Id("iSignupAction")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement imageCaptcha = hades.ElementVisible(By.CssSelector("#hipTemplateContainer > div:nth-child(1) > img"), "ExceptionContinueCaptcha");
            if (imageCaptcha.IsNotNull())
            {
                CaptchaSolve(imageCaptcha);
            }
            else
            {
                if (hades.ElementVisible(By.CssSelector("#wlspispHipChallengeContainer > div:nth-child(2) > input"), "ExceptionSetVerificationCode").IsNotNull())
                {
                    MessageBox.Show("Debe cambiar la Ip para continuar usando este Gate", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    hades.Kill();
                    message.Kill();
                    this.VerifyCardsComplete();

                }
                ExecuteRefreshProgressThread(35, "configurando fecha de nacimiento", "Success");
                Restart();
            }
        }

        private void GoToReadEmail(IWebElement tableDataSubject)
        {
            try
            {
                ExecuteRefreshProgressThread(45, "Verificacion de usuario fake...", "Success");
                tableDataSubject.JsClick();
                Thread.Sleep(2000);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement paragraphVerificationCode = message.ElementVisible(By.CssSelector("body > div.wrapper.theme-2-active.navbar-top-light.horizontal-nav > div > div.container > div.row > div > div > div > div > div > div > aside.col-sm-3.col-lg-9.col-md-8.pl-0 > div > div.panel-wrapper.collapse.in > div > div.container-fluid.view-mail.mt-20.col-lg-9.col-md-8 > table > tbody > tr:nth-child(4) > td > span"), "ExceptionGoToReadEmail");
            if (paragraphVerificationCode.IsNotNull())
            {
                TakeVerificationCode(paragraphVerificationCode);
            }
            else
            {
                ExecuteRefreshProgressThread(45, "ERROR Verificacion de usuario fake, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void TakeVerificationCode(IWebElement paragraphVerificationCode)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Extrayendo codigo de verificacion...", "Success");
                VerificationCode = paragraphVerificationCode.Text;
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (VerificationCode == null)
            {
                ExecuteRefreshProgressThread(50, "ERROR Extrayendo codigo de verificacion, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void Message()
        {
            try
            {
                ExecuteRefreshProgressThread(45, "Cargando servidor de datos externos...", "Success");
                message = Driver.Chrome();
                message.GoUrl("https://faketempmail.com/t/" + UserName);
                ExecuteRefreshProgressThread(28, "Servidor de datos externos cargado...", "Success");
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement spanCurrentEmail = message.ElementVisible(By.Id("currentmail"), "ExceptionMessage");
            if (spanCurrentEmail.IsNotNull())
            {
                TakeEmail(spanCurrentEmail);
            }
            else
            {
                ExecuteRefreshProgressThread(26, "ERROR Cargando servidor de datos externos, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void TakeEmail(IWebElement spanCurrentEmail)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Datos externos extraidos...", "Success");
                Email = spanCurrentEmail.Text;
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (Email == null)
            {
                ExecuteRefreshProgressThread(50, "ERROR Datos externos extraidos, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void CaptchaSolve(IWebElement imageCaptcha)
        {
            try
            {
                ExecuteRefreshProgressThread(55, "Resolviendo captcha...", "Success");
                string imageCaptchaSource = imageCaptcha.GetAttribute("src");
                ImageCaptchaResolve = Captcha.Image(imageCaptchaSource);
                if (ImageCaptchaResolve != null)
                {
                    hades.FindElement(By.CssSelector("#hipTemplateContainer > div:nth-child(3) > input")).SendKeys(ImageCaptchaResolve.Replace(" ", ""));
                }
                else
                {
                    ShowMyDialogBoxCaptcha(imageCaptchaSource);
                    Thread.Sleep(5000);

                    hades.FindElement(By.CssSelector("#hipTemplateContainer > div:nth-child(3) > input")).SendKeys(ImageCaptchaResolve.Replace(" ",""));
                }
                hades.FindElement(By.Id("iSignupAction")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement buttonBalance = hades.ElementVisible(By.CssSelector("body > div.app-container > div > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(3) > div:nth-child(2) > div > button:nth-child(2)"));

            if (buttonBalance.IsNotNull())
            {
                GoToBalance(buttonBalance);
            }
            else
            {
                IWebElement imageCaptcha2 = hades.ElementVisible(By.CssSelector("#hipTemplateContainer > div:nth-child(1) > img"), "ExceptionContinueCaptcha");
                if (imageCaptcha2.IsNotNull())
                {
                    CaptchaSolve(imageCaptcha2);
                }
                else
                {
                    ExecuteRefreshProgressThread(43, "ERROR Resolviendo captcha, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }

        private void GoToBalance(IWebElement buttonBalance)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Consultando Saldo...", "Success");
                buttonBalance.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement buttonContinue = hades.ElementVisible(By.CssSelector("body > div.app-container > div > div:nth-child(1) > div:nth-child(2) > div > div:nth-child(1) > div:nth-child(2) > div:nth-child(3) > div > div > div > div.rxCustomScroll.rxCustomScrollV > div.scrollViewport.scrollViewportV > div > div > button"), "ExceptionGoToBalance");

            if (buttonContinue.IsNotNull())
            {
                BalanceAccept(buttonContinue);
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR Consultando Saldo, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void BalanceAccept(IWebElement buttonContinue)
        {
            try
            {
                ExecuteRefreshProgressThread(65, "Calculando Saldo...", "Success");
                buttonContinue.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement divCheckout = hades.ElementVisible(By.CssSelector("button[title=\"Continuar\"]"), "ExceptionBalanceAcceptDivCheckout");

            if (divCheckout.IsNotNull())
            {
                Checkout(divCheckout);
            }
            else
            {
                IWebElement buttonOptions = hades.ElementVisible(By.CssSelector("button[title=\"$5.00\"]"), "ExceptionBalanceAcceptbuttonOptions");

                if (buttonOptions.IsNotNull())
                {
                    SelectOption(buttonOptions);
                }
                else
                {
                    ExecuteRefreshProgressThread(65, "ERROR Calculando Saldo, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }

        private void SelectOption(IWebElement buttonOptions)
        {
            try
            {
                ExecuteRefreshProgressThread(70, "Seleccionando Opciones", "Success");
                buttonOptions.JsClick();
                hades.FindElement(By.CssSelector("button[title=\"Comprar $5.00 de crédito\"]")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            Thread.Sleep(5000);
            string newTabHandle = hades.WindowHandles.Last();
            hadesAlt = hades.SwitchTo().Window(newTabHandle);
            Thread.Sleep(5000);

            IWebElement inputFirstName = hadesAlt.ElementVisible(By.Id("firstName"), "ExceptionCheckout");

            if (inputFirstName.IsNotNull())
            {
                CheckoutAddress(inputFirstName);
            }
            else
            {
                ExecuteRefreshProgressThread(70, "ERROR seleccionando opciones, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void Checkout(IWebElement divCheckout)
        {
            try
            {
                ExecuteRefreshProgressThread(75, "Procesar pago...", "Success");
                divCheckout.JsClick();
            }
            catch (NoSuchElementException) { /**/ }
            catch (StaleElementReferenceException) { /**/ }
            catch (WebDriverException) { /**/ }
            catch (NullReferenceException) { /**/ }

            ExecuteRefreshProgressThread(35, "Cambiando a nueva pestaña...", "Success");
            Thread.Sleep(5000);
            string newTabHandle = hades.WindowHandles.Last();
            hadesAlt = hades.SwitchTo().Window(newTabHandle);
            Thread.Sleep(5000);

            IWebElement inputFirstName = hadesAlt.ElementVisible(By.Id("firstName"), "ExceptionCheckout");

            if (inputFirstName.IsNotNull())
            {
                CheckoutAddress(inputFirstName);
            }
            else
            {
                ExecuteRefreshProgressThread(75, "ERROR procesar pago, Reiniciando proceso...", "Success");
                Restart();
            }

        }

        private void CheckoutAddress(IWebElement inputFirstName)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Ingresando direccion fake...", "Success");
                inputFirstName.SendKeys(FirstName);
                hadesAlt.FindElement(By.Id("lastName")).SendKeys(LastName);
                hadesAlt.FindElement(By.Id("addressLine1")).SendKeys(Address);
                hadesAlt.FindElement(By.Id("addressLine2")).SendKeys(Address2);
                hadesAlt.FindElement(By.Id("city")).SendKeys(City);
                Thread.Sleep(2000);
                SelectElement selectState = new SelectElement(hadesAlt.FindElement(By.Id("state")));
                int maxValue = selectState.Options.Count();
                selectState.SelectByIndex(new Random().Next(1, maxValue));
                Thread.Sleep(2000);
                hadesAlt.FindElement(By.Id("zip")).SendKeys(Zip);
                Thread.Sleep(1000);
                hadesAlt.FindElement(By.Id("continueButton")).Click();

            }
            catch (NoSuchElementException) { /**/ }
            catch (StaleElementReferenceException) { /**/ }
            catch (WebDriverException) { /**/ }
            catch (NullReferenceException) { /**/ }

            IWebElement iframeCheckoutSetup = hadesAlt.ElementVisible(By.Id("frame"), "ExceptionCheckoutAddress", 20);
            if (iframeCheckoutSetup.IsNotNull())
            {
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR direccion fake, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void LastProccess()
        {
            try
            {
                int num = 0;
                while (num < cardsList.Count())
                {
                    List<string> list = cardsList[num].Split('|').ToList();
                    if (cardsList.Count() > 1)
                    {
                        cardsList[num] = cardsList[num].Replace("\r", "");
                    }
                    string text = cardsList[num];
                    string cards = cardsList[num].ToString();
                    if (cardsList.Count() > 1)
                    {
                        string text3 = cardsList[num] = (cardsList[num] += "\r");
                        cards = string.Join("", cardsList.ToArray());
                        text += "\r";
                    }
                    CheckoutSetup(list[0], list[1], list[2], list[3]);
                    if (Confirmation())
                    {
                        ExecuteReduceCardsThread(cards);
                        ExecuteLiveCardThread(text);
                        cardsList.Remove(cardsList[num]);
                        if (cardsList.Count() > 0)
                        {
                            Restart();
                        }
                    }
                    else
                    {
                        ExecuteReduceCardsThread(cards);
                        ExecuteDieCardThread(text);
                        cardsList.Remove(cardsList[num]);
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }


        private void CheckoutSetup(string number, string month, string year, string cvv)
        {
            try
            {
                ExecuteRefreshProgressThread(85, "Añadiendo tarjeta: " + number, "Success");
                Thread.Sleep(5000);
                hadesAlt.SwitchTo().Frame(hadesAlt.FindElement(By.Id("frame")));
                Thread.Sleep(1000);
                for (int i = 0; i < number.Length; i++)
                {
                    hadesAlt.FindElement(By.Id("cardNumber")).SendKeys(number.Substring(i, 1));
                }
                Thread.Sleep(1000);
                new SelectElement(hadesAlt.FindElement(By.Id("dateId"))).SelectByValue(month.TrimStart('0'));
                Thread.Sleep(1000);
                new SelectElement(hadesAlt.FindElement(By.Id("dateId2"))).SelectByValue(year);
                Thread.Sleep(1000);
                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        hadesAlt.FindElement(By.Id("cvcInput")).SendKeys("000");
                        break;
                    case 4:
                        hadesAlt.FindElement(By.Id("cvcInput")).SendKeys("0000");
                        break;
                }

                hadesAlt.SwitchTo().DefaultContent();

                Thread.Sleep(1000);
                hadesAlt.FindElement(By.Id("continueButton")).Click();

            }
            catch (NoSuchElementException) {  }
            catch (StaleElementReferenceException) {  }
            catch (WebDriverException) {  }
            catch (NullReferenceException) {  }

            Thread.Sleep(10000);
        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Estado de la tarjeta...", "Success");
            Thread.Sleep(2000);
            IWebElement webElement = hadesAlt.ElementVisible(By.CssSelector("div[data-notification-key=\"order-creation-errors\"]"), "Die");

            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE...", "Success");
                Thread.Sleep(2000);
                return false;
            }
            else
            {
                if (hadesAlt.Url.Contains("fatal"))
                {
                    ExecuteRefreshProgressThread(100, "DIE...", "Success");
                    Thread.Sleep(2000);
                    Restart();
                    return false;
                }
                else
                {
                    string newTabHandle = hadesAlt.WindowHandles.First();
                    hades = hadesAlt.SwitchTo().Window(newTabHandle);

                    IWebElement divBalance = hades.ElementVisible(By.CssSelector("div[data-text-as-pseudo-element=\"$5.00\"]"),"live");
                    if (divBalance.IsNotNull())
                    {
                        ExecuteRefreshProgressThread(100, "LIVE...", "Success");
                        Thread.Sleep(2000);
                        return true;
                    }
                }
            }
            Restart();
            return false;
        }

        private void Restart()
        {
            message.Kill();
            hades.Kill();
            VerifyCards();
        }

        public void ExecuteThread()
        {
            VerifyCardsThread = new Thread(VerifyCards);
            VerifyCardsThread.Start();
        }

        private void ExecuteRefreshProgressThread(int percent, string message, string typeMessage)
        {
            RefreshProgressThread = new Thread((ThreadStart)delegate
            {
                RefreshProgress(percent, message, typeMessage);
            });
            RefreshProgressThread.Start();
        }

        public void KillThread()
        {
            try
            {
                VerifyCardsThread.Abort();
                VerifyCardsThread.Join();
                hades.Kill();
            }
            catch (NullReferenceException)
            {
            }
        }

        private void ExecuteLiveCardThread(string card)
        {
            StateCardsThread = new Thread((ThreadStart)delegate
            {
                LiveCard(card);
            });
            StateCardsThread.Start();
        }

        public void KillLiveCardThread()
        {
            try
            {
                StateCardsThread.Abort();
                StateCardsThread.Join();
            }
            catch (NullReferenceException)
            {
            }
        }

        private void ExecuteDieCardThread(string card)
        {
            StateCardsThread = new Thread((ThreadStart)delegate
            {
                DieCard(card);
            });
            StateCardsThread.Start();
        }

        public void KillDieCardThread()
        {
            try
            {
                StateCardsThread.Abort();
                StateCardsThread.Join();
            }
            catch (NullReferenceException)
            {
            }
        }

        private void ExecuteReduceCardsThread(string cards)
        {
            ReduceCardsThread = new Thread((ThreadStart)delegate
            {
                ReduceCards(cards);
            });
            ReduceCardsThread.Start();
        }

        public void KillReduceCardsThread()
        {
            try
            {
                ReduceCardsThread.Abort();
                ReduceCardsThread.Join();
            }
            catch (NullReferenceException)
            {
            }
        }

        public void ReduceCards(string cards)
        {
            this.ReduceCardsComplete(cards);
        }

        private void LiveCard(string card)
        {
            this.LiveCardComplete(card);
        }

        private void DieCard(string card)
        {
            this.DieCardComplete(card);
        }

        private void RefreshProgress(int percent, string message, string typeMessage)
        {
            this.RefreshProgressComplete(percent, message, typeMessage);
        }

        public void ShowMyDialogBoxCaptcha(string imageCaptcha)
        {

            Captcha captcha = new Captcha();
            captcha.PbCaptcha.LoadAsync(imageCaptcha);
            captcha.ShowDialog();
            if (captcha.DialogResult == DialogResult.Yes)
            {
                ImageCaptchaResolve = captcha.TxCaptcha.Text;
            }
            captcha.Dispose();
        }
    }
}
