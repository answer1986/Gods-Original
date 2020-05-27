using Bogus;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace _0lymp.us.Gods
{

    public partial class Hera : Component
    {
        public delegate void VerifyCardsCompleteHandler();
        public delegate void StateCardsCompleteHandler(string card);
        public delegate void ReduceCardsCompleteHandler(string cards);
        public delegate void RefreshProgressCompleteHandler(int percent, string message, string typeMessage);

        private static IWebDriver hera;
        public List<string> CardsList { get; set; }
        public string Url { get; set; }
        private Faker faker;
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string address;
        private string city;
        private string state;
        private string zip;

        public Thread VerifyCardsThread;
        public Thread StateCardsThread;
        public Thread ReduceCardsThread;
        public Thread RefreshProgressThread;

        public event VerifyCardsCompleteHandler VerifyCardsComplete;
        public event StateCardsCompleteHandler LiveCardComplete;
        public event StateCardsCompleteHandler DieCardComplete;
        public event ReduceCardsCompleteHandler ReduceCardsComplete;
        public event RefreshProgressCompleteHandler RefreshProgressComplete;

        public Hera()
        {
            InitializeComponent();
            faker = new Faker();
        }

        public Hera(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public void VerifyCards()
        {

            firstName = faker.Name.FirstName();
            lastName = faker.Name.LastName();
            email = faker.Internet.Email(firstName, lastName);
            phone = faker.Phone.PhoneNumber();
            address = faker.Address.StreetName();
            city = faker.Address.City();
            zip = faker.Address.ZipCode();
            Load();
            hera.Kill();
            ExecuteRefreshProgressThread(0, "", "Success");
            this.VerifyCardsComplete();
        }

        private void Load(bool fastRestart = false)
        {
            try
            {
                List<string> source = new List<string>
            {
                Url+"359434+1000020+2999951740&Eon=359434",
                Url+"359436+1000020+2999951740&Eon=359436",
                Url+"359438+1000020+2999951740&Eon=359438",
                Url+"359441+1000020+2999951740&Eon=359441",
                Url+"359443+1000020+2999951740&Eon=359443",
                Url+"359453+1000020+2999951740&Eon=359453"
            };
                ExecuteRefreshProgressThread(10, "HERA Cargando...", "Success");
                if (!fastRestart)
                {
                    hera = Driver.Chrome();
                }
                hera.GoUrl((from x in source
                            orderby Guid.NewGuid()
                            select x).FirstOrDefault());
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement element = hera.ElementRandomVisible(By.Name("add-to-cart-btn"), "ExceptionLoad");

            if (element.IsNotNull())
            {
                Item(element);
            }
            else
            {
                FastRestart();
            }
        }

        private void Item(IWebElement element)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Seleccion aleatoria de producto...", "Success");
                element.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.Id("addToCart-cart-checkout"), "ExceptionItem");
            if (_element.IsNotNull())
            {
                AddToCart(_element);
            }
            else
            {
                FastRestart();
            }
        }

        private void AddToCart(IWebElement element)
        {
            try
            {
                ExecuteRefreshProgressThread(30, "Producto añadido al carrito...", "Success");
                element.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.Id("wag-cart-proceed-to-checkout"), "ExceptionAddToCart");

            if (_element.IsNotNull())
            {
                Checkout(_element);
            }
            else
            {
                ExecuteRefreshProgressThread(30, "ERROR Producto añadido al carrito, Reiniciar proceso...", "Success");
                Restart();
            }

        }

        private void Checkout(IWebElement element)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "Proceso de Pago seguro...", "Success");
                element.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.Id("checkout_btn"), "ExceptionCheckout");

            if (_element.IsNotNull())
            {
                Guest(_element);
            }
            else
            {
                ExecuteRefreshProgressThread(40, "ERROR Proceso de Pago seguro, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void Guest(IWebElement element)
        {
            try
            {
                //hera.FindElement(By.Id("user-name")).SendKeys(email);
                ExecuteRefreshProgressThread(50, "Compra como invitado...", "Success");
                element.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.CssSelector("label[for=\"ship-to-STS\"]"), "ExceptionGuest");

            if (_element.IsNotNull())
            {
                ShipToStore(_element);
            }
            else
            {
                ExecuteRefreshProgressThread(50, "ERROR Compra como invitado, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void ShipToStore(IWebElement element)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "Compra con recogida en tienda...", "Success");
                element.JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.Id("store-search-address"), "ExceptionShipToStore");

            if (_element.IsNotNull())
            {
                StoresByState(_element);
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR Compra con recogida en tienda, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void StoresByState(IWebElement element)
        {
            state = faker.Address.StateAbbr();
            try
            {
                ExecuteRefreshProgressThread(70, "Listando tiendas por estado...", "Success");

                element.Clear();
                element.SendKeys(state);
                hera.FindElement(By.Id("store_search")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementTextContains(By.CssSelector("#zero-location-header > div > strong"), "No locations", "ExceptionStoresByStateNoLocations");

            if (_element.IsNotNull())
            {
                StoresByState(element);
            }
            else
            {
                IWebElement __element = hera.ElementRandomVisible(By.Id("rx_select_store"), "ExceptionStoresByStateSelectStores");

                if (__element.IsNotNull())
                {
                    SelectStore(__element);
                }
                else
                {
                    ExecuteRefreshProgressThread(70, "ERROR Listando tiendas por estado, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }

        private void SelectStore(IWebElement element)
        {
            try
            {
                Thread.Sleep(2000);
                ExecuteRefreshProgressThread(80, "Selecccionando tienda...", "Success");
                element.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.CssSelector("#continue-checkout > div.row.row-m0.mt25.justify-right.hidden-xs > span > button"), "ExceptionSelectStore");

            if (_element.IsNotNull())
            {
                InfoUser();
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR Selecccionando tienda, Reiniciando proceso...", "Success");
                Restart();
            }
        }

        private void InfoUser()
        {
            try
            {
                ExecuteRefreshProgressThread(85, "Insertando datos de usuario...", "Success");
                Thread.Sleep(3000);
                hera.FindElement(By.Id("firstName")).Clear();
                hera.FindElement(By.Id("lastName")).Clear();
                hera.FindElement(By.Id("email")).Clear();
                hera.FindElement(By.Id("phoneNumber")).Clear();
                hera.FindElement(By.Id("firstName")).SendKeys(firstName);
                hera.FindElement(By.Id("lastName")).SendKeys(lastName);
                hera.FindElement(By.Id("email")).SendKeys(email);
                hera.FindElement(By.Id("phoneNumber")).SendKeys(phone);
                Thread.Sleep(3000);
                hera.FindElement(By.CssSelector("#continue-checkout > div.row.row-m0.mt25.justify-right.hidden-xs > span > button")).JsClick();
                //element.Click();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            Thread.Sleep(3000);
            IWebElement _element = hera.ElementTextContains(By.CssSelector("span[role=\"alert\"]"), "Please", "ExceptionInfoUserAlert");

            if (_element.IsNotNull())
            {
                firstName = faker.Name.FirstName();
                lastName = faker.Name.LastName();
                email = faker.Internet.Email(firstName, lastName);
                phone = faker.Phone.PhoneNumber();
                InfoUser();
            }
            else
            {

                if (hera.ElementVisible(By.Id("saveCreditCardAnony"), "ExceptionInfoUser").IsNotNull())
                {
                    LastProccess();
                }
                else
                {
                    ExecuteRefreshProgressThread(85, "ERROR Insertando datos de usuario, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }

        private void LastProccess()
        {
            try
            {
                int num = 0;
                bool firstTime = true;
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
                    if (num > 0)
                    {
                        firstTime = false;
                    }

                    Payment(list[0], list[1], list[2], firstTime);

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
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }

        private void Payment(string number, string month, string year, bool firstTime)
        {
            try
            {
                ExecuteRefreshProgressThread(90, "Insertando datos de compra...", "Success");

                hera.FindElement(By.Id("credit_card_value")).Clear();
                for (int i = 0; i < number.Length; i++)
                {
                    hera.FindElement(By.Id("credit_card_value")).SendKeys(number.Substring(i, 1));
                    Thread.Sleep(300);
                }
                hera.FindElement(By.Id("wag-cko-pm-sel-cc-exp-mon")).Clear();
                hera.FindElement(By.Id("wag-cko-pm-sel-cc-exp-mon")).SendKeys(month + "/" + year.Substring(year.Length - 2));
                Thread.Sleep(1000);

                hera.FindElement(By.Id("addstreet1")).Clear();
                hera.FindElement(By.Id("addstreet1")).SendKeys(address);
                Thread.Sleep(1000);
                hera.FindElement(By.Id("addcity")).Clear();
                hera.FindElement(By.Id("addcity")).SendKeys(city);
                Thread.Sleep(1000);
                new SelectElement(hera.FindElement(By.Id("addstate"))).SelectByValue(state);
                Thread.Sleep(1000);
                hera.FindElement(By.Id("addzipcode")).Clear();
                hera.FindElement(By.Id("addzipcode")).SendKeys(zip);

                Thread.Sleep(1000);
                hera.FindElement(By.Id("saveCreditCardAnony")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement _element = hera.ElementVisible(By.CssSelector("#order-review > section > div > section > section.wag-checkout-order-summary-button-container > div > div > label"), "ExceptionPayment");

            if (_element.IsNotNull())
            {
                Order(_element);
            }
            else
            {
                IWebElement __element = hera.ElementVisible(By.CssSelector("#order-review > section > div > section > section.wag-checkout-order-summary-button-container > div > div > label > span.wag-ar-product"), "ExceptionPaymentOrderReview");

                if (__element.IsNotNull())
                {
                    IWebElement ___element = hera.ElementVisible(By.Id("wag-Payment-globle-error"), "ExceptionPaymentError");
                    if (___element.IsNotNull())
                    {
                        ___element.RemoveElement();
                        Payment(number, month, year, firstTime);
                    }
                }
                else
                {
                    ExecuteRefreshProgressThread(90, "ERROR Insertando datos de compra, Reiniciando proceso...", "Success");
                    Restart();
                }
            }
        }

        private void Order(IWebElement element)
        {
            try
            {
                ExecuteRefreshProgressThread(95, "Comprobando orden...", "Success");
                element.JsClick();
                hera.FindElement(By.CssSelector("#order-review > section > div > section > section.wag-checkout-order-summary-button-container > section.wag-cko-buttons-right-column.hidden-xs > div.row.row-m0.pb20.pt10 > span > button")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(97, "Estado de la tarjeta...", "Success");
            Thread.Sleep(3000);
            if (hera.ElementVisible(By.Id("wag-Payment-globle-error"), "Die").IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE...", "Success");
                Thread.Sleep(3000);
                return false;
            }
            else
            {
                if (hera.ElementTextEquals(By.CssSelector("#printableContent > section:nth-child(1) > div.col-lg-12.col-md-12.col-sm-12.col-xs-12.pb15 > div > h1 > span"), "Order Confirmation", "live").IsNotNull())
                {
                    ExecuteRefreshProgressThread(100, "LIVE...", "Success");
                    Thread.Sleep(3000);
                    return true;
                }
                else
                {
                    ExecuteRefreshProgressThread(100, "ERROR LIVE or DIE.", "Success");
                    Restart();
                }
            }

            return false;
        }

        private void Restart()
        {
            hera.Kill();
            VerifyCards();
        }

        private void FastRestart()
        {
            Load(fastRestart: true);
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
                hera.Kill();
            }
            catch (NullReferenceException) { }
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
            catch (NullReferenceException) { }
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
            catch (NullReferenceException) { }
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

    }

}
