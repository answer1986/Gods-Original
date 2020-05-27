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
    public partial class dionisio : Component
    {
        private static IWebDriver poseidon;
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

        public Thread VerifyCardsThread;
        public Thread StateCardsThread;
        public Thread ReduceCardsThread;

        public delegate void VerifyCardsCompleteHandler();
        public delegate void StateCardsCompleteHandler(string card);
        public delegate void ReduceCardsCompleteHandler(string cards);

        public event VerifyCardsCompleteHandler VerifyCardsComplete;
        public event StateCardsCompleteHandler LiveCardComplete;
        public event StateCardsCompleteHandler DieCardComplete;
        public event ReduceCardsCompleteHandler ReduceCardsComplete;

        public dionisio()
        {
            InitializeComponent();
            Faker = new Faker();
        }

        public dionisio(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void VerifyCards()
        {
            FirstName = Faker.Name.FirstName();
            LastName = Faker.Name.LastName();
            UserName = Faker.Internet.UserName();
            Password = Faker.Internet.Password();
            Email = Faker.Internet.Email();
            Company = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : Faker.Company.CompanyName());
            Address = Faker.Address.StreetAddress();
            Address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : Faker.Address.SecondaryAddress());
            City = Faker.Address.City();
            State = Faker.Address.StateAbbr();
            Zip = Faker.Address.ZipCode();
            Phone = Faker.Phone.PhoneNumber();
            Load();
            poseidon.Kill();
            this.VerifyCardsComplete();
        }

        private void Load(bool fastRestart = false)
        {
            try
            {
                if (!fastRestart)
                {
                    poseidon = Driver.Chrome();
                }
                poseidon.GoUrl(Url);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            IWebElement anchorAddToCart = poseidon.ElementRandomVisible(By.CssSelector("#CategoryContent > ul > li > div.ProductDetails > div:nth-child(8) > div > a"), "ExceptionLoad");
            if (anchorAddToCart.IsNotNull())
            {
                AddToCart(anchorAddToCart);
            }
            else
            {
                FastRestart();
            }
        }

        private void AddToCart(IWebElement anchorAddToCart)
        {
            try
            {
                anchorAddToCart.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            IWebElement anchorProceedToCheckout = poseidon.ElementVisible(By.CssSelector("#CartContent > div > div:nth-child(1) > div > div.CheckoutButton > a"), "ExceptionLoad");
            if (anchorProceedToCheckout.IsNotNull())
            {
                ProceedToCheckout(anchorProceedToCheckout);
            }
            else
            {
                Restart();
            }
        }

        private void ProceedToCheckout(IWebElement anchorProceedToCheckout)
        {
            try
            {
                anchorProceedToCheckout.Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            IWebElement inputEmail = poseidon.ElementVisible(By.Id("email"), "ExceptionProceedToCheckout", 30);
            if (inputEmail.IsNotNull())
            {
                Customer(inputEmail);
            }
            else
            {
                Restart();
            }
        }

        private void Customer(IWebElement inputEmail)
        {
            try
            {
                inputEmail.SendKeys(Email);
                poseidon.FindElement(By.Id("checkout-customer-continue")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            IWebElement inputFirstName = poseidon.ElementVisible(By.Id("firstNameInput"), "ExceptionCustomer");
            if (inputFirstName.IsNotNull())
            {
                ShippingAddress(inputFirstName);
            }
            else
            {
                Restart();
            }
            //System.Windows.Forms.MessageBox.Show("OK");
        }

        private void ShippingAddress(IWebElement inputFirstName)
        {
            try
            {
                inputFirstName.SendKeys(FirstName);
                poseidon.FindElement(By.Id("lastNameInput")).SendKeys(LastName);
                poseidon.FindElement(By.Id("phoneInput")).SendKeys(Phone);
                poseidon.FindElement(By.Id("companyInput")).SendKeys(Company);
                poseidon.FindElement(By.Id("addressLine1Input")).SendKeys(Address);
                poseidon.FindElement(By.Id("addressLine2Input")).SendKeys(Address2);
                poseidon.FindElement(By.Id("cityInput")).SendKeys(City);
                new SelectElement(poseidon.FindElement(By.Id("countryCodeInput"))).SelectByValue("US");
                Thread.Sleep(2000);
                new SelectElement(poseidon.FindElement(By.Id("provinceCodeInput"))).SelectByValue(State);

                poseidon.FindElement(By.Id("postCodeInput")).SendKeys("31076");

                Thread.Sleep(10000);

                poseidon.FindElement(By.Id("checkout-shipping-continue")).JsClick();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }

            if (poseidon.ElementVisible(By.Id("ccNumber"), "ExceptionShippingAddress").IsNotNull())
            {
                LastProccess();
            }
            else
            {
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
                    }
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }

        private void Payment(string number, string month, string year, string cvv)
        {
            try
            {
                //poseidon.FindElement(By.Id("radio-stripe")).JsClick();
                //Thread.Sleep(3000);
                poseidon.FindElement(By.Id("ccNumber")).Clear();
                poseidon.FindElement(By.Id("ccNumber")).SendKeys(number);
                poseidon.FindElement(By.Id("ccExpiry")).Clear();
                poseidon.FindElement(By.Id("ccExpiry")).SendKeys(month + year.Substring(year.Length - 2));
                poseidon.FindElement(By.Id("ccName")).Clear();
                poseidon.FindElement(By.Id("ccName")).SendKeys(FirstName + " " + LastName);
                poseidon.FindElement(By.Id("ccCvv")).Clear();
                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        poseidon.FindElement(By.Id("ccCvv")).SendKeys("000");
                        break;
                    case 4:
                        poseidon.FindElement(By.Id("ccCvv")).SendKeys("0000");
                        break;
                }
                Thread.Sleep(5000);
                poseidon.FindElement(By.Id("checkout-payment-continue")).JsClick();
            }
            catch (NoSuchElementException) { throw; }
            catch (StaleElementReferenceException) { throw; }
            catch (WebDriverException) { throw; }
            catch (NullReferenceException) { throw; }
        }

        private bool Confirmation()
        {
            //div.modal.optimizedCheckout-contentPrimary.modal--error.modal--afterOpen > div.modal-body > p
            IWebElement element = poseidon.ElementTextContains(By.CssSelector("div.modal.optimizedCheckout-contentPrimary.modal--error > div.modal-header > h2"), "wrong", "Die", 20);
            if (element.IsNotNull())
            {
                poseidon.FindElement(By.CssSelector("div.modal.optimizedCheckout-contentPrimary.modal--error > div.modal-footer > button")).Click();
                return false;
            }

            IWebElement element2 = poseidon.ElementTextContains(By.CssSelector("#checkout-app > div > div > div > h1"), "Thank", "Live");
            if (element2.IsNotNull())
            {
                return true;
            }
            Restart();
            return false;
        }
        private void Restart()
        {
            poseidon.Kill();
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

        public void KillThread()
        {
            try
            {
                VerifyCardsThread.Abort();
                VerifyCardsThread.Join();
                poseidon.Kill();
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
    }
}
