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
    public partial class Zeus : Component
    {
        private static IWebDriver zeus;
        private static IWebDriver message;
        public List<string> cardsList;
        public string Url { get; set; }
        private Faker faker;
        private string address;
        private string address2;
        private string city;
        private string state;
        private string zip;
        private string phone;

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

        private string FullName { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string OTP { get; set; }
        private ResolveCaptcha Captcha { get; set; }
        private string ImageCaptchaResolve { get; set; }

        public Zeus()
        {
            InitializeComponent();
            faker = new Faker();
            Captcha = new ResolveCaptcha();
        }

        public Zeus(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void VerifyCards()
        {
            FullName = faker.Name.FullName();
            UserName = faker.Internet.UserName();
            Password = faker.Internet.Password();
            address = faker.Address.StreetAddress();
            address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : faker.Address.SecondaryAddress());
            city = faker.Address.City();
            state = faker.Address.State();
            zip = faker.Address.ZipCode("#####");
            phone = faker.Phone.PhoneNumber();
            Load();
            zeus.Kill();
            message.Kill();
            ExecuteRefreshProgressThread(0, "", "Success");
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando HERA...", "Success");
                zeus = Driver.Chrome();
                zeus.GoUrl(Url);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement anchorVideoPrime = zeus.ElementVisible(By.CssSelector("#dvm_MLP_NA_Join_1 > div > div.dv-row-item.dv-copy.dv-push-left > a"), "ExceptionLoad");
            if (anchorVideoPrime.IsNotNull())
            {
                GoToSignIn(anchorVideoPrime);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR HERA, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void GoToSignIn(IWebElement anchorVideoPrime)
        {
            try
            {
                ExecuteRefreshProgressThread(5, "HERA Cargado...", "Success");
                anchorVideoPrime.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement anchorCreateAcountSubmit = zeus.ElementVisible(By.Id("createAccountSubmit"), "ExceptionGoToSignIn");
            if (anchorCreateAcountSubmit.IsNotNull())
            {
                GoToCreateAccount(anchorCreateAcountSubmit);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR Accediendo. Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void GoToCreateAccount(IWebElement anchorCreateAcountSubmit)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Iniciando Creacion usuario fake...", "Success");
                anchorCreateAcountSubmit.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = zeus.ElementVisible(By.Id("continue"), "ExceptionGoToCreateAccount");
            if (element.IsNotNull())
            {
                Message();
                CreateAccount();
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR creacion de usuario fake. Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void Message()
        {
            try
            {
                ExecuteRefreshProgressThread(26, "Cargando servidor de datos externos...", "Success");
                message = Driver.Chrome();
                message.GoUrl("https://faketempmail.com/t/" + UserName);
                ExecuteRefreshProgressThread(28, "Servidor de datos externos cargado...", "Success");
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = message.ElementVisible(By.Id("currentmail"), "ExceptionMessage");
            if (webElement.IsNotNull())
            {
                TakeEmail(webElement);
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
                ExecuteRefreshProgressThread(29, "Extrayendo datos externos...", "Success");
                if (!spanCurrentEmail.Text.Contains("@fivemails.com"))
                {
                    Email = spanCurrentEmail.Text;
                }
                else
                {
                    ChangeEmail();
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (Email == null)
            {
                ExecuteRefreshProgressThread(29, "ERROR extrayendo datos externos, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void CreateAccount(bool internalError = false)
        {
            try
            {
                ExecuteRefreshProgressThread(35, "Ingresando datos de usuario fake...", "Success");
                if (!internalError)
                {
                    zeus.FindElement(By.Id("ap_customer_name")).SendKeys(UserName);
                    zeus.FindElement(By.Id("ap_email")).SendKeys(Email);
                    zeus.FindElement(By.Id("ap_password")).SendKeys(Password);
                    zeus.FindElement(By.Id("ap_password_check")).SendKeys(Password);
                }
                else
                {
                    zeus.FindElement(By.Id("ap_email")).Clear();
                    zeus.FindElement(By.Id("ap_email")).SendKeys(Email);
                    zeus.FindElement(By.Id("ap_password")).Clear();
                    zeus.FindElement(By.Id("ap_password")).SendKeys(Password);
                    zeus.FindElement(By.Id("ap_password_check")).Clear();
                    zeus.FindElement(By.Id("ap_password_check")).SendKeys(Password);
                }
                zeus.FindElement(By.Id("continue")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = zeus.ElementVisible(By.Id("auth-captcha-image"), "ExceptionContinueCaptcha");

            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(40, "Resolviendo captcha...", "Success");
                string attribute = webElement.GetAttribute("src");
                ImageCaptchaResolve = Captcha.Image(attribute);
                if (ImageCaptchaResolve != null)
                {
                    ContinueCaptcha();
                }
                else
                {
                    ShowMyDialogBoxCaptcha(attribute);
                }
            }
            else
            {
                IWebElement webElement2 = zeus.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div:nth-child(1) > form > div:nth-child(2) > input"), "ExceptionContinueCaptchaNotFoundVerifyCode");
                if (webElement2.IsNotNull())
                {
                    IWebElement webElement3 = message.ElementVisible(By.CssSelector("#maillists > tr > td:nth-child(3)"), "ExceptionRegisterNotFoundEmail");
                    if (webElement3.IsNotNull())
                    {
                        GoToReadEmail(webElement3);
                        InsertOTP(webElement2);
                    }
                    else
                    {
                        ExecuteRefreshProgressThread(35, "ERROR codigo de verificacion. Reiniciando proceso..", "Success");
                        Restart();
                    }
                }
                else
                {

                    //IWebElement webElement4 = zeus.ElementTextContains(By.CssSelector("#auth-error-message-box > div > div > ul > li > span"), "Error", "ExceptionContinueCaptchaInternalError");
                    //if (webElement4.IsNotNull())
                    //{
                    //    zeus.FindElement(By.Id("auth-error-message-box")).RemoveElement();
                    //    ChangeEmail();
                    //    CreateAccount(true);
                    //}
                    //else
                    //{
                        ExecuteRefreshProgressThread(40, "ERROR validar datos externos, Reiniciando proceso...", "Success");
                        Restart();
                    //}
                }
            }
        }

        private void ChangeEmail()
        {
            try
            {
                ExecuteRefreshProgressThread(39, "Datos externos actualizados", "Success");
                message.FindElement(By.CssSelector("body > div.wrapper.theme-2-active.navbar-top-light.horizontal-nav > div > div.container > div:nth-child(2) > div > div > div > div > div > div > aside.col-lg-4.col-md-6.pl-0 > ul > li:nth-child(2) > button.randomBtn.btn.btn-primary.btn-rounded.text-center")).Click();
                ExecuteRefreshProgressThread(42, "Extrayendo datos externos actualizados..", "Success");

            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = message.ElementVisible(By.Id("currentmail"), "ExceptionChangeEmail");
            if (webElement.IsNotNull())
            {
                TakeEmail(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(39, "ERROR extrayendo datos externos actualizados, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void ContinueCaptcha()
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Captcha resuelto...", "Success");

                Thread.Sleep(2000);

                zeus.FindElement(By.Id("ap_password")).SendKeys(Password);
                zeus.FindElement(By.Id("ap_password_check")).SendKeys(Password);

                ExecuteRefreshProgressThread(42, "Insertando captcha resuelto...", "Success");

                zeus.FindElement(By.Id("auth-captcha-guess")).SendKeys(ImageCaptchaResolve);
                zeus.FindElement(By.Id("continue")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = zeus.ElementVisible(By.Id("auth-captcha-image"), "ExceptionContinueCaptcha");

            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(43, "Resolviendo captcha...", "Success");
                string attribute = webElement.GetAttribute("src");
                ImageCaptchaResolve = Captcha.Image(attribute);
                if (ImageCaptchaResolve != null)
                {
                    ContinueCaptcha();
                }
                else
                {
                    ShowMyDialogBoxCaptcha(attribute);
                }
            }

            IWebElement webElement2 = zeus.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div:nth-child(1) > form > div:nth-child(2) > input"), "ExceptionContinueCaptchaNotFoundVerifyCode");
            if (webElement2.IsNotNull())
            {
                IWebElement webElement3 = message.ElementVisible(By.CssSelector("#maillists > tr > td:nth-child(3)"), "ExceptionRegisterNotFoundEmail");
                if (webElement3.IsNotNull())
                {
                    GoToReadEmail(webElement3);
                    InsertOTP(webElement2);
                }
                else
                {
                    ExecuteRefreshProgressThread(40, "ERROR codigo de verificacion. Reiniciando proceso..", "Success");
                    Restart();
                }
            }
            else
            {

                //IWebElement webElement4 = zeus.ElementTextContains(By.CssSelector("#auth-error-message-box > div > div > ul > li > span"), "Error", "ExceptionContinueCaptchaInternalError");
                //if (webElement4.IsNotNull())
                //{
                //    zeus.FindElement(By.Id("auth-error-message-box")).RemoveElement();
                //    ChangeEmail();
                //    CreateAccount(true);
                //}
                //else
                //{
                    ExecuteRefreshProgressThread(40, "ERROR validar datos externos", "Success");
                    Restart();
                //}
            }
        }

        private void GoToReadEmail(IWebElement tableDataSubject)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Verificacion de usuario fake...", "Success");
                tableDataSubject.JsClick();
                Thread.Sleep(2000);
                ExecuteRefreshProgressThread(50, "Extrayendo codigo de verificacion...", "Success");
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = message.ElementVisible(By.CssSelector("#verificationMsg > p.otp"), "ExceptionGoToReadEmail");
            if (webElement.IsNotNull())
            {
                TakeOTP(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(50, "ERROR Extrayendo codigo de verificacion, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void TakeOTP(IWebElement paragraphOTP)
        {
            try
            {
                ExecuteRefreshProgressThread(55, "Codigo de verificacion extraido...", "Success");
                OTP = paragraphOTP.Text;
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (OTP == null)
            {
                ExecuteRefreshProgressThread(55, "Codigo de verificacion no extraido, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void InsertOTP(IWebElement inputOTP)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Ingresando codigo de verificacion...", "Success");
                inputOTP.SendKeys(OTP);
                zeus.FindElement(By.CssSelector("#a-autoid-0 > span > input")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = zeus.ElementVisible(By.Name("ppw-widgetEvent:AddCreditCardEvent"), "ExceptionInsertOTP");
            if (element.IsNotNull())
            {
                ExecuteRefreshProgressThread(63, "Codigo de verificacion Correcto...", "Success");
                Thread.Sleep(3000);
                ExecuteRefreshProgressThread(65, "Iniciando comprobacion de tarjetas...", "Success");
                Thread.Sleep(3000);
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR Ingresando codigo de verificacion, Reiniciando proceso...", "Success");
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

                    AddCard(list[0], list[1], list[2]);
                    PaymentMethod();
                    //Order();
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
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void AddCard(string number, string month, string year)
        {
            try
            {
                ExecuteRefreshProgressThread(70, "Añadiendo tarjeta: " + number, "Success");
                Thread.Sleep(3000);
                zeus.FindElement(By.Name("ppw-accountHolderName")).SendKeys(FullName);
                zeus.FindElement(By.Name("addCreditCardNumber")).SendKeys(number);
                new SelectElement(zeus.FindElement(By.Name("ppw-expirationDate_month"))).SelectByText(month);
                new SelectElement(zeus.FindElement(By.Name("ppw-expirationDate_year"))).SelectByText(year);
                zeus.FindElement(By.Name("ppw-widgetEvent:AddCreditCardEvent")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = zeus.ElementVisible(By.CssSelector("span.a-button.a-button-primary.pmts-use-selected-address.pmts-button-input > span.a-button-inner > input.a-button-input"), "ExceptionAddOtherCard");
            if (webElement.IsNotNull())
            {
                UseSameAddress(webElement);
            }
            else
            {
                IWebElement element = zeus.ElementVisible(By.Name("ppw-widgetEvent:AddAddressEvent"), "ExceptionAddCard");
                if (element.IsNotNull())
                {
                    AddAddress();
                }
                else
                {
                    ExecuteRefreshProgressThread(70, "ERROR añadiendo tarjeta, Reiniciando Proceso...", "Success");
                    Restart();
                }
            }
        }

        private void UseSameAddress(IWebElement inputAddOtherCard)
        {
            try
            {
                ExecuteRefreshProgressThread(75, "Direccion fake...", "Success");
                inputAddOtherCard.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            //#ctaGroup > div.btn-wrapper > input
            IWebElement element = zeus.ElementVisible(By.CssSelector("#ctaGroup > div.btn-wrapper > input"), "ExceptionUseSameAddress");
            if (element.IsNotNull())
            {
                Continue();
            }
            else
            {
                ExecuteRefreshProgressThread(75, "ERROR Direccion Fake", "Success");
                Restart();
            }
        }

        private void Continue()
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Continuando...", "Success");
                Thread.Sleep(3000);
                zeus.FindElement(By.CssSelector("#ctaGroup > div.btn-wrapper > input")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (zeus.ElementVisible(By.CssSelector("#ctaGroup > button"), "ExceptionContinue").IsNotNull())
            {
                Order();
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR Continuar, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void AddAddress(bool newZip = false)
        {
            try
            {
                ExecuteRefreshProgressThread(75, "Añadiendo direccion fake...", "Success");
                if (!newZip)
                {
                    zeus.FindElement(By.Name("ppw-line1")).SendKeys(address);
                    zeus.FindElement(By.Name("ppw-line2")).SendKeys(address2);
                    zeus.FindElement(By.Name("ppw-city")).SendKeys(city);
                    zeus.FindElement(By.Name("ppw-stateOrRegion")).SendKeys(state);
                    zeus.FindElement(By.Name("ppw-postalCode")).SendKeys(zip);
                    SelectElement selectElement = new SelectElement(zeus.FindElement(By.Name("ppw-countryCode")));
                    int maxValue = selectElement.Options.Count();
                    selectElement.SelectByIndex(new Random().Next(0, maxValue));
                    zeus.FindElement(By.Name("ppw-phoneNumber")).SendKeys(phone);
                }
                else
                {
                    zeus.FindElement(By.Name("ppw-postalCode")).Clear();
                    zeus.FindElement(By.Name("ppw-postalCode")).SendKeys(faker.Address.ZipCode("#####"));
                    SelectElement selectElement2 = new SelectElement(zeus.FindElement(By.Name("ppw-countryCode")));
                    int maxValue2 = selectElement2.Options.Count();
                    selectElement2.SelectByIndex(new Random().Next(0, maxValue2));
                }
                zeus.FindElement(By.Name("ppw-widgetEvent:AddAddressEvent")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = zeus.ElementVisible(By.CssSelector("div.a-column.a-span12 > div.a-box.a-alert.a-alert-error"), "ExceptionDataAddress");
            if (element.IsNotNull())
            {
                ExecuteRefreshProgressThread(75, "ERROR añadiendo direccion fake, Reintentando...", "Success");
                element.RemoveElement();
                AddAddress(true);
            }
        }

        private void PaymentMethod()
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Realizando Pago...", "Success");
                Thread.Sleep(3000);
                zeus.FindElement(By.CssSelector("#ctaGroup > div.btn-wrapper > input")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (zeus.ElementVisible(By.CssSelector("#ctaGroup > button"), "ExceptionPaymentMethod").IsNotNull())
            {
                Order();
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR realizando pago, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void Order()
        {
            try
            {
                ExecuteRefreshProgressThread(85, "Realizando Orden...", "Success");
                Thread.Sleep(3000);
                zeus.FindElement(By.CssSelector("#ctaGroup > button")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Estado de la tarjeta...", "Success");

            IWebElement webElement = zeus.ElementVisible(By.Id("validation-error"), "Die");
            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE...", "Success");
                Thread.Sleep(3000);
                PreparingForAddNewCard(webElement);
                return false;
            }
            IWebElement element = zeus.ElementVisible(By.CssSelector("#pv-nav-accounts > label > span"), "Live", 40);
            if (element.IsNotNull())
            {
                return VerifyLive();
            }
            Restart();
            return false;
        }

        private bool VerifyLive()
        {
            try
            {
                ExecuteRefreshProgressThread(95, "Verificando LIVE...", "Success");
                message.GoUrl("https://faketempmail.com/t/" + UserName);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            Thread.Sleep(3000);
            if (message.ElementTextContains(By.CssSelector("#maillists > tr.readmail.unread > td.view-message.dont-show"), "<no-reply@primevideo.com>", "ExceptionVerifyLive").IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "LIVE...", "Success");
                return true;
            }
            else
            {
                ExecuteRefreshProgressThread(100, "DIE...", "Success");
                return false;
            }
        }

        private void PreparingForAddNewCard(IWebElement divDie)
        {
            RemoveErrorPayment(divDie);
            //GoToAddNewCard();
            AddNewCard();
        }

        private void RemoveErrorPayment(IWebElement divDie)
        {
            try
            {
                divDie.RemoveElement();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!zeus.ElementDisappear(By.CssSelector("#a-page > div.a-box.a-alert.a-alert-error.a-spacing-base.a-spacing-top-base"), "ExceptionRemoveErrorPayment"))
            {
                Restart();
            }
        }

        private void AddNewCard()
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Añadiendo otra tarjeta...", "Success");
                IWebElement element = zeus.ElementVisible(By.CssSelector("a[data-action=\"a-expander-toggle\"]"), "ExceptionPreparingForAddNewCardAddCard");
                if (element.IsNotNull())
                {
                    zeus.FindElement(By.CssSelector("a[data-action=\"a-expander-toggle\"]")).Click();
                }
                else
                {
                    ExecuteRefreshProgressThread(50, "Error al añadir otra tarjeta...", "Success");
                    Restart();
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

        }


        private void Restart()
        {
            message.Kill();
            zeus.Kill();
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
                zeus.Kill();
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
        private void RefreshProgress(int percent, string message, string typeMessage)
        {
            this.RefreshProgressComplete(percent, message, typeMessage);
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

        public void ShowMyDialogBoxCaptcha(string imageCaptcha, bool robotCheck = false)
        {
            ExecuteRefreshProgressThread(40, "Por favor resolver captcha...", "Success");

            Captcha captcha = new Captcha();
            captcha.PbCaptcha.LoadAsync(imageCaptcha);
            captcha.ShowDialog();
            if (captcha.DialogResult == DialogResult.Yes)
            {
                ImageCaptchaResolve = captcha.TxCaptcha.Text;
            }
            captcha.Dispose();
            if (robotCheck)
            {
                //RobotCheck();
            }
            else
            {
                ContinueCaptcha();
            }

        }
    }
}
