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
    public partial class Poseidon : Component
    {
        private static IWebDriver poseidon;
        private static IWebDriver message;
        public List<string> CardsList;
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



        public Poseidon()
        {
            InitializeComponent();
            faker = new Faker();
            Captcha = new ResolveCaptcha();
        }

        public Poseidon(IContainer container)
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
            poseidon.Kill();
            message.Kill();
            ExecuteRefreshProgressThread(0, "", "Success");
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando POSEIDON...", "Success");
                poseidon = Driver.ChromeNoImage();
                poseidon.GoUrl(Url);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = poseidon.ElementVisible(By.CssSelector("#prime-header-CTA > span > input"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                GoToLogin(webElement);
            }
            else
            {
                IWebElement webElement2 = poseidon.ElementVisible(By.CssSelector("body > div > div.a-row.a-spacing-double-large > div.a-section > div > div > form > div.a-row.a-spacing-large > div > div > div.a-row.a-text-center > img"), "ExceptionLoadRobotCheck");
                if (webElement2.IsNotNull())
                {
                    string attribute = webElement2.GetAttribute("src");
                    ImageCaptchaResolve = Captcha.Image(attribute);
                    if (ImageCaptchaResolve != null)
                    {
                        RobotCheck();
                    }
                    else
                    {
                        ShowMyDialogBoxCaptcha(attribute, true);
                    }
                }
                else
                {
                    ExecuteRefreshProgressThread(1, "ERROR POSEIDON, Reiniciando proceso", "Success");
                    Restart();
                }
            }
        }

        private void RobotCheck()
        {
            try
            {
                poseidon.FindElement(By.Id("captchacharacters")).SendKeys(ImageCaptchaResolve);
                poseidon.FindElement(By.CssSelector("body > div > div.a-row.a-spacing-double-large > div.a-section > div > div > form > div.a-section.a-spacing-extra-large > div > span > span > button")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = poseidon.ElementVisible(By.Id("signup-button-native-id"), "ExceptionRobotCheck");
            if (webElement.IsNotNull())
            {
                GoToLogin(webElement);
            }
            else
            {
                Restart();
            }
        }

        private void GoToLogin(IWebElement inputSignup)
        {
            try
            {
                ExecuteRefreshProgressThread(5, "Zeus Cargado...", "Success");
                inputSignup.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = poseidon.ElementVisible(By.Id("createAccountSubmit"), "ExceptionGoToLogin");
            if (element.IsNotNull())
            {
                GoToCreateAccount(element);
            }
            else
            {
                Restart();
            }
        }

        private void GoToCreateAccount(IWebElement element)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Iniciando Creacion usuario fake.", "Success");
                element.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = poseidon.ElementVisible(By.Id("continue"), "ExceptionGoToCreateAccount");
            if (_element.IsNotNull())
            {
                Message();
                Register();
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
                ExecuteRefreshProgressThread(26, "cargando servidor de datos externos...", "Success");
                message = Driver.ChromeNoImage();
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

        private void Register(bool internalError = false)
        {
            try
            {
                ExecuteRefreshProgressThread(35, "Ingresando datos de usuarios fake...", "Success");
                if (!internalError)
                {
                    poseidon.FindElement(By.Id("ap_customer_name")).SendKeys(UserName);
                    poseidon.FindElement(By.Id("ap_email")).SendKeys(Email);
                    poseidon.FindElement(By.Id("ap_password")).SendKeys(Password);
                    poseidon.FindElement(By.Id("ap_password_check")).SendKeys(Password);
                }
                else
                {
                    poseidon.FindElement(By.Id("ap_email")).Clear();
                    poseidon.FindElement(By.Id("ap_email")).SendKeys(Email);
                    poseidon.FindElement(By.Id("ap_password")).Clear();
                    poseidon.FindElement(By.Id("ap_password")).SendKeys(Password);
                    poseidon.FindElement(By.Id("ap_password_check")).Clear();
                    poseidon.FindElement(By.Id("ap_password_check")).SendKeys(Password);
                }
                poseidon.FindElement(By.Id("continue")).Click();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            // Thread.Sleep(5000);

            IWebElement webElement = poseidon.ElementVisible(By.Id("auth-captcha-image"), "ExceptionRegisterCaptcha");
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

                //#cvf-page-content > div > div > div > form > div:nth-child(7) > input
                //#image-captcha-section > div > div.a-section.a-text-center > img
                //IWebElement webElement4 = poseidon.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div > div:nth-child(3) > div > img"), "ExceptionRegisterNotFoundVerifyCode2");
                IWebElement webElement4 = poseidon.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div > form > div:nth-child(7) > input"), "ExceptionRegisterNotFoundVerifyCode2");
                if (webElement4.IsNotNull())
                {
                    MessageBox.Show("Second Captcha");
                    ExecuteRefreshProgressThread(40, "Resolviendo captcha...", "Success");
                    string attribute = poseidon.FindElement(By.CssSelector("#cvf-page-content > div > div > div > form > div:nth-child(7) > input")).GetAttribute("src");
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
                    IWebElement webElement2 = poseidon.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div:nth-child(1) > form > div:nth-child(2) > input"), "ExceptionRegisterNotFoundVerifyCode");
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
                        //IWebElement webElement4 = poseidon.ElementTextContains(By.CssSelector("#auth-error-message-box > div > div > ul > li > span"), "Error", "ExceptionContinueCaptchaInternalError");
                        //if (webElement4.IsNotNull())
                        //{
                        //    poseidon.FindElement(By.Id("auth-error-message-box")).RemoveElement();
                        //    ChangeEmail();
                        //    Register(true);
                        //}
                        //else
                        //{
                        ExecuteRefreshProgressThread(35, "ERROR validar datos externos, Reiniciando proceso...", "Success");
                        Restart();
                        //}
                    }
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
              //  if (poseidon.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div > form > div:nth-child(7) > input")).IsNotNull())
                if (poseidon.ElementVisible(By.CssSelector("input.a-input-text.a-span12.cvf-widget-input.cvf-widget-input-code.cvf-widget-input-captcha.fwcim-captcha-guess")).IsNotNull())
                {
                    ExecuteRefreshProgressThread(40, "Segundo Captcha resuelto...", "Success");

                    Thread.Sleep(2000);

                    ExecuteRefreshProgressThread(42, "Insertando segundo captcha resuelto...", "Success");

                    poseidon.FindElement(By.CssSelector("input.a-input-text.a-span12.cvf-widget-input.cvf-widget-input-code.cvf-widget-input-captcha.fwcim-captcha-guess")).SendKeys(ImageCaptchaResolve);
                    poseidon.FindElement(By.CssSelector("#a-autoid-0 > span > input")).Click();

                }
                else
                {
                    ExecuteRefreshProgressThread(40, "Captcha resuelto...", "Success");

                    Thread.Sleep(2000);

                    poseidon.FindElement(By.Id("ap_password")).SendKeys(Password);
                    poseidon.FindElement(By.Id("ap_password_check")).SendKeys(Password);

                    ExecuteRefreshProgressThread(42, "Insertando captcha resuelto...", "Success");
                    poseidon.FindElement(By.Id("auth-captcha-guess")).SendKeys(ImageCaptchaResolve);
                    poseidon.FindElement(By.Id("continue")).Click();
                }

            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            Thread.Sleep(5000);

            IWebElement webElement = poseidon.ElementVisible(By.Id("auth-captcha-image"), "ExceptionContinueCaptcha");

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
            else
            {
                IWebElement webElement4 = poseidon.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div > div:nth-child(3) > div > img"), "ExceptionRegisterNotFoundVerifyCode2");
                if (webElement4.IsNotNull())
                {
                    ExecuteRefreshProgressThread(40, "Resolviendo captcha...", "Success");
                    string attribute = webElement4.GetAttribute("src");
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
                    IWebElement webElement2 = poseidon.ElementVisible(By.CssSelector("#cvf-page-content > div > div > div:nth-child(1) > form > div:nth-child(2) > input"), "ExceptionContinueCaptchaNotFoundVerifyCode");
                    if (webElement2.IsNotNull())
                    {
                        IWebElement webElement3 = message.ElementVisible(By.CssSelector("#maillists > tr > td:nth-child(3)"), "ExceptionContinueCaptchaNotFoundEmail");
                        if (webElement3.IsNotNull())
                        {
                            GoToReadEmail(webElement3);
                            InsertOTP(webElement2);
                        }
                        else
                        {
                            ResendVerificationCode();
                            ExecuteRefreshProgressThread(40, "ERROR codigo de verificacion. Reiniciando proceso..", "Success");
                            //Restart();
                        }
                    }
                    else
                    {
                        //IWebElement webElement4 = poseidon.ElementTextContains(By.CssSelector("#auth-error-message-box > div > div > ul > li > span"), "Error", "ExceptionContinueCaptchaInternalError");
                        //if (webElement4.IsNotNull())
                        //{
                        //    poseidon.FindElement(By.Id("auth-error-message-box")).RemoveElement();
                        //    ChangeEmail();
                        //    Register(true);
                        //}
                        //else
                        //{
                        ExecuteRefreshProgressThread(40, "ERROR validar datos externos", "Success");
                        Restart();
                        //}
                    }
                }
            }
        }


        private void GoToReadEmail(IWebElement tableDataSubject)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Verificacion de usuario fake...", "Success");
                tableDataSubject.Click();
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
                ResendVerificationCode();
                ExecuteRefreshProgressThread(50, "ERROR Extrayendo codigo de verificacion, Reiniciando Proceso...", "Success");
                //Restart();
            }
        }


        private void ResendVerificationCode()
        {
            try
            {
                poseidon.FindElement(By.CssSelector("#cvf-page-content > div > div > div:nth-child(1) > form > div.a-section.a-spacing-none.a-spacing-top-large.a-text-center.cvf-widget-section-js > a")).Click();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            Thread.Sleep(3000);
            ContinueCaptcha();
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
                ExecuteRefreshProgressThread(55, "ERROR Codigo de verificacion no extraido, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void InsertOTP(IWebElement inputOTP)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Ingresando codigo de verificacion...", "Success");
                inputOTP.SendKeys(OTP);
                poseidon.FindElement(By.CssSelector("#a-autoid-0 > span > input")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = poseidon.ElementVisible(By.Name("ppw-widgetEvent:AddCreditCardEvent"), "ExceptionInsertOTP");
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
                while (num < CardsList.Count())
                {
                    List<string> list = CardsList[num].Split('|').ToList();
                    if (CardsList.Count() > 1)
                    {
                        CardsList[num] = CardsList[num].Replace("\r", "");
                    }
                    string text = CardsList[num];
                    string cards = CardsList[num].ToString();
                    if (CardsList.Count() > 1)
                    {
                        string text3 = CardsList[num] = (CardsList[num] += "\r");
                        cards = string.Join("", CardsList.ToArray());
                        text += "\r";
                    }
                    AddCard(list[0], list[1], list[2]);
                    Order();
                    if (Confirmation())
                    {
                        ExecuteReduceCardsThread(cards);
                        ExecuteLiveCardThread(text);
                        CardsList.Remove(CardsList[num]);
                        if (CardsList.Count() > 0)
                        {
                            Restart();
                        }
                    }
                    else
                    {
                        ExecuteReduceCardsThread(cards);
                        ExecuteDieCardThread(text);
                        CardsList.Remove(CardsList[num]);
                        //break;
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }

        private void AddCard(string number, string month, string year)
        {
            try
            {
                ExecuteRefreshProgressThread(70, "Añadiendo tarjeta: " + number, "Success");
                Thread.Sleep(3000);
                poseidon.FindElement(By.Name("ppw-accountHolderName")).SendKeys(FullName);
                poseidon.FindElement(By.Name("addCreditCardNumber")).SendKeys(number);
                new SelectElement(poseidon.FindElement(By.Name("ppw-expirationDate_month"))).SelectByText(month);
                new SelectElement(poseidon.FindElement(By.Name("ppw-expirationDate_year"))).SelectByText(year);
                poseidon.FindElement(By.Name("ppw-widgetEvent:AddCreditCardEvent")).Click();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            IWebElement webElement = poseidon.ElementVisible(By.CssSelector("span.a-button.a-button-primary.pmts-use-selected-address.pmts-button-input > span.a-button-inner > input.a-button-input"), "ExceptionAddOtherCard");
            if (webElement.IsNotNull())
            {
                UseSameAddress(webElement);
            }
            else
            {
                IWebElement element = poseidon.ElementVisible(By.Name("ppw-widgetEvent:AddAddressEvent"), "ExceptionAddCard");
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

            IWebElement element = poseidon.ElementVisible(By.Name("ppw-widgetEvent:PreferencePaymentOptionSelectionEvent"), "ExceptionUseSameAddress");
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
                poseidon.FindElement(By.Name("ppw-widgetEvent:PreferencePaymentOptionSelectionEvent")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!poseidon.ElementVisible(By.CssSelector("#a-autoid-0 > span > input"), "ExceptionContinue").IsNotNull())
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
                    poseidon.FindElement(By.Name("ppw-line1")).SendKeys(address);
                    poseidon.FindElement(By.Name("ppw-line2")).SendKeys(address2);
                    poseidon.FindElement(By.Name("ppw-city")).SendKeys(city);
                    poseidon.FindElement(By.Name("ppw-stateOrRegion")).SendKeys(state);
                    poseidon.FindElement(By.Name("ppw-postalCode")).SendKeys(zip);
                    SelectElement selectElement = new SelectElement(poseidon.FindElement(By.Name("ppw-countryCode")));
                    int maxValue = selectElement.Options.Count();
                    selectElement.SelectByIndex(new Random().Next(0, maxValue));
                    poseidon.FindElement(By.Name("ppw-phoneNumber")).SendKeys(phone);
                }
                else
                {
                    poseidon.FindElement(By.Name("ppw-postalCode")).Clear();
                    poseidon.FindElement(By.Name("ppw-postalCode")).SendKeys(faker.Address.ZipCode("#####"));
                    SelectElement selectElement2 = new SelectElement(poseidon.FindElement(By.Name("ppw-countryCode")));
                    int maxValue2 = selectElement2.Options.Count();
                    selectElement2.SelectByIndex(new Random().Next(0, maxValue2));
                }
                poseidon.FindElement(By.Name("ppw-widgetEvent:AddAddressEvent")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = poseidon.ElementVisible(By.CssSelector("div.a-column.a-span12 > div.a-box.a-alert.a-alert-error"), "ExceptionDataAddress");
            if (element.IsNotNull())
            {
                ExecuteRefreshProgressThread(75, "ERROR añadiendo direccion fake, Reintentando...", "Success");
                element.RemoveElement();
                AddAddress(newZip: true);
            }
            IWebElement element2 = poseidon.ElementVisible(By.CssSelector("#a-autoid-0 > span > input"), "ExceptionAddAddress");
            if (!element2.IsNotNull())
            {
                ExecuteRefreshProgressThread(75, "ERROR añadiendo direccion fake, Reiniciando proceso...", "Success");
                Restart();
            }
        }



        private void Order()
        {
            try
            {
                ExecuteRefreshProgressThread(85, "Realizando Pago...", "Success");
                Thread.Sleep(3000);
                poseidon.FindElement(By.CssSelector("#a-autoid-0 > span > input")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (poseidon.ElementVisible(By.CssSelector("#a-page > div.prime-notification > div > div > i"), "Alert").IsNotNull())
            {
                Order();
            }
        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Estado de la tarjeta...", "Success");

            IWebElement webElement = poseidon.ElementVisible(By.CssSelector("#a-page > div.a-box.a-alert.a-alert-error.a-spacing-base.a-spacing-top-base"), "Die");
            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE...", "Success");
                Thread.Sleep(3000);
                PreparingForAddNewCard(webElement);
                return false;
            }
            IWebElement element = poseidon.ElementTextEquals(By.CssSelector("#nav-link-prime > span.nav-line-2"), "Prime", "Live");
            if (element.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "LIVE...", "Success");
                return true;
            }
            //KillRefreshProgressThread();
            Restart();
            return false;
        }

        private void PreparingForAddNewCard(IWebElement divDie)
        {
            RemoveErrorPayment(divDie);
            GoToAddNewCard();
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

            if (!poseidon.ElementDisappear(By.CssSelector("#a-page > div.a-box.a-alert.a-alert-error.a-spacing-base.a-spacing-top-base"), "ExceptionRemoveErrorPayment"))
            {
                Restart();
            }
        }

        private void GoToAddNewCard()
        {
            try
            {
                IWebElement element = poseidon.ElementVisible(By.CssSelector("div.a-column.a-span6 > div:nth-child(2) > div > span.pmts-custom-link-button"), "ExceptionGoToAddNewCard");
                if (element.IsNotNull())
                {
                    if (poseidon.ElementDisappear(By.ClassName("wlp-prime-overlay-container"), "ExceptionOverlayGoToAddNewCard"))
                    {
                        poseidon.FindElement(By.CssSelector("div.a-column.a-span6 > div:nth-child(2) > div > span.pmts-custom-link-button")).Click();
                    }
                }
                else
                {
                    Restart();
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
        }

        private void AddNewCard()
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Añadiendo otra tarjeta...", "Success");
                IWebElement element = poseidon.ElementVisible(By.CssSelector("a[data-action=\"a-expander-toggle\"]"), "ExceptionPreparingForAddNewCardAddCard");
                if (element.IsNotNull())
                {
                    poseidon.FindElement(By.CssSelector("a[data-action=\"a-expander-toggle\"]")).Click();
                }
                else
                {
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
            poseidon.Kill();
            VerifyCards();
        }

        public void ExecuteThread()
        {
            VerifyCardsThread = new Thread(VerifyCards);
            VerifyCardsThread.Start();
        }

        public void KillThread()
        {
            try
            {
                ExecuteRefreshProgressThread(0, "", "Success");
                VerifyCardsThread.Abort();
                VerifyCardsThread.Join();
                poseidon.Kill();
                message.Kill();
            }
            catch (NullReferenceException)
            {
            }
        }

        public void ExecuteRefreshProgressThread(int percent, string message, string typeMessage)
        {
            RefreshProgressThread = new Thread((ThreadStart)delegate
            {
                RefreshProgress(percent, message, typeMessage);
            });
            RefreshProgressThread.Start();
        }

        public void KillRefreshProgressThread()
        {
            try
            {
                RefreshProgressThread.Abort();
                RefreshProgressThread.Join();
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
            try
            {
                this.RefreshProgressComplete(percent, message, typeMessage);
            }
            catch (NullReferenceException) { }
        }

        public void ShowMyDialogBoxCaptcha(string imageCaptcha, bool robotCheck = false)
        {
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
                RobotCheck();
            }
            else
            {
                ContinueCaptcha();
            }

        }
    }
}
