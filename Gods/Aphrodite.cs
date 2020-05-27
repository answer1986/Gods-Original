using Bogus;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace _0lymp.us.Gods
{
    public partial class Aphrodite : Component
    {
        private static IWebDriver aphrodite;
        public List<string> CardsList { get; set; }
        public string Url;
        private Faker faker;
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }
        private string Address { get; set; }
        private string City { get; set; }
        private string Postcode { get; set; }
        private string Phone { get; set; }

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


        public Aphrodite()
        {
            InitializeComponent();
            faker = new Faker();
        }

        public Aphrodite(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public void VerifyCards()
        {
            Random random = new Random();
            FirstName = faker.Name.FirstName();
            LastName = faker.Name.LastName();
            UserName = faker.Internet.UserName();
            Email = faker.Internet.Email();
            Password = faker.Internet.Password();
            Address = faker.Address.StreetAddress();
            City = faker.Address.City();
            Postcode = faker.Address.ZipCode("??##??");
            Phone = faker.Phone.PhoneNumber();
            Load();
            ExecuteRefreshProgressThread(0, "", "Success");
            aphrodite.Kill();
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            try
            {
                ExecuteRefreshProgressThread(1, "Cargando AFRODITA...", "Success");
                aphrodite = Driver.ChromeNoImage();
                aphrodite.GoUrl(Url);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!aphrodite.WaitForLoadElement(By.Id("plan-2-btn"), "ExceptionLoad"))
            {
                ExecuteRefreshProgressThread(1, "ERROR AFRODITA, Reiniciando proceso", "Success");
                Restart();
            }
            else
            {
                ContinuePlan();
            }
        }

        private void ContinuePlan()
        {
            try
            {
                ExecuteRefreshProgressThread(10, "AFRODITA Cargado...", "Success");
                Thread.Sleep(2000);
                ExecuteRefreshProgressThread(15, "Seleccionando plan.", "Success");
                aphrodite.FindElement(By.Id("plan-2-btn")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!aphrodite.WaitForLoadElement(By.Id("first-name"), "ExceptionContinuePlan"))
            {
                ExecuteRefreshProgressThread(10, "ERROR seleccionando plan. Reiniciando Proceso...", "Success");
                Restart();
            }
            else
            {
                CheckoutRegistration();
            }
        }

        private void CheckoutRegistration(bool usernameError = false, bool passwordError = false)
        {
            try
            {
                ExecuteRefreshProgressThread(20, "Ingresando datos usuario fake.", "Success");
                if (usernameError | passwordError)
                {
                    if (usernameError)
                    {
                        aphrodite.FindElement(By.Id("username")).Clear();
                        aphrodite.FindElement(By.Id("username")).SendKeys(faker.Internet.UserName());
                    }
                    if (passwordError)
                    {
                        aphrodite.FindElement(By.Id("password")).Clear();
                        aphrodite.FindElement(By.Id("password")).SendKeys(faker.Internet.Password());
                    }
                }
                else
                {
                    aphrodite.FindElement(By.Id("first-name")).SendKeys(FirstName);
                    aphrodite.FindElement(By.Id("last-name")).SendKeys(LastName);
                    aphrodite.FindElement(By.Id("username")).SendKeys(UserName);
                    aphrodite.FindElement(By.Id("email")).SendKeys(Email);
                    aphrodite.FindElement(By.Id("password")).SendKeys(Password);
                    aphrodite.FindElement(By.Id("newsletter-option")).JsClick();
                    aphrodite.FindElement(By.Id("is-acknowledged")).JsClick();
                }
                Thread.Sleep(3000);
                aphrodite.FindElement(By.Id("next-step-btn")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!aphrodite.WaitForLoadElement(By.Id("credit-card-number"), "ExceptionCheckoutRegistration"))
            {
                usernameError = aphrodite.WaitForLoadElement(By.CssSelector("#username-errors > span.off-screen"), "ExceptionCheckoutRegistrationUsername");
                passwordError = aphrodite.WaitForLoadElement(By.CssSelector("#password-errors > span.off-screen"), "ExceptionCheckoutRegistrationPassword");
                if (usernameError | passwordError)
                {
                    CheckoutRegistration(usernameError, passwordError);
                }
                else
                {
                    ExecuteRefreshProgressThread(20, "ERROR ingresando datos usuario fake. Reiniciando proceso...", "Success");
                    Restart();
                }
            }
            else
            {
                LastProccess();
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
                    CheckoutPayment(list[0], list[1], list[2], list[3]);
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
                        if(limitDie < 5)
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
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void CheckoutPayment(string number, string month, string year, string cvv)
        {
            try
            {
                ExecuteRefreshProgressThread(50, "Añadiendo tarjeta: " + number, "Success");
                aphrodite.FindElement(By.Id("credit-card-number")).SendKeys(number);
                aphrodite.FindElement(By.Id("cc-expiration-date")).SendKeys(month + "/" + year.Substring(year.Length - 2));
                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        aphrodite.FindElement(By.Id("cvv")).SendKeys("000");
                        break;
                    case 4:
                        aphrodite.FindElement(By.Id("cvv")).SendKeys("0000");
                        break;
                }
                if (aphrodite.FindElement(By.Id("billing-address-line-1")).GetAttribute("value") == "")
                {
                    aphrodite.FindElement(By.Id("billing-address-line-1")).SendKeys(Address);
                    aphrodite.FindElement(By.Id("billing-city")).SendKeys(City);
                    aphrodite.FindElement(By.CssSelector("#payment-address > div:nth-child(4) > div.col-6.left-input > div > button")).JsClick();
                    (from x in aphrodite.FindElements(By.CssSelector("#-menu > li")).ToList()
                     orderby Guid.NewGuid()
                     select x).FirstOrDefault().JsClick();
                    Thread.Sleep(3000);
                    aphrodite.FindElement(By.Id("billing-postal-code")).SendKeys(Postcode);
                    aphrodite.FindElement(By.Id("phone-number")).SendKeys(Phone);
                }
                aphrodite.FindElement(By.Id("next-step-btn")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!aphrodite.WaitForLoadElement(By.CssSelector("#review > div.container.outer-container > div > div > div.disclosure-message > form > div > div > label"), "ExceptionCheckoutPayment"))
            {
                ExecuteRefreshProgressThread(50, "ERROR añadiendo tarjeta, Reiniciando Proceso...", "Success");
                Restart();
            }
            else
            {
                CheckoutReview();
            }
        }

        private void CheckoutReview()
        {
            try
            {
                ExecuteRefreshProgressThread(70, "Finalizando...", "Success");
                aphrodite.FindElement(By.CssSelector("#review > div.container.outer-container > div > div > div.disclosure-message > form > div > div > label")).JsClick();
                aphrodite.FindElement(By.Id("next-step-btn")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
        }

        private bool Confirmation()
        {
            ExecuteRefreshProgressThread(90, "Estado de la tarjeta...", "Success");

            if (!aphrodite.WaitForTextContainsByElement(By.CssSelector("#card-failure-modal > div.modal-header > span"), "Sorry", "ExceptionConfirmationDie"))
            {
                if (!aphrodite.WaitForTextContainsByElement(By.CssSelector("#confirmation > div.container.outer-container > div.conf-form-container.form-content.receipt > div.row > div > p"), "confirmation", "ExceptionConfirmationLive"))
                {
                    ExecuteRefreshProgressThread(90, "ERROR estado de la tarjeta. Reiniciando proceso...", "Success");
                    Thread.Sleep(2000);
                    Restart();
                    return false;
                }
                ExecuteRefreshProgressThread(100, "LIVE...", "Success");
                Thread.Sleep(2000);
                return true;
            }
            ReturnCheckoutPayment();
            ExecuteRefreshProgressThread(100, "DIE...", "Success");
            Thread.Sleep(2000);
            return false;
        }

        private void ReturnCheckoutPayment()
        {
            try
            {
                aphrodite.FindElement(By.Id("modalBtnId")).JsClick();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
        }

        private void Restart()
        {
            aphrodite.Kill();
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
                VerifyCardsThread.Abort();
                VerifyCardsThread.Join();
                aphrodite.Kill();
            }
            catch (NullReferenceException)
            {
            }
        }

        private void ExecuteRefreshProgressThread(int percent, string message, string typeMessage)
        {
            RefreshProgressThread = new Thread((ThreadStart)delegate
            {
                RefreshProgress(percent, message, typeMessage);
            });
            RefreshProgressThread.Start();
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

