using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.ComponentModel;
using System.Threading;

namespace _0lymp.us.Gods
{
    public partial class Perses : Component
    {

        private IWebDriver perses;

        public string bin;
        public string month;
        public string year;
        public string cvv;
        public string quantity;
        private string Result{get;set;}

        public Thread GenerateCardsThread;
        public event GenerateCardsCompleteHandler GenerateCardsComplete;
        public delegate void GenerateCardsCompleteHandler(string CreditCards);

        public Perses()
        {
            InitializeComponent();
        }

        public Perses(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void GenerateCards()
        {
            Load();
            perses.Kill();
            this.GenerateCardsComplete(Result.Trim());
        }

        private void Load()
        {
            try
            {
                perses = Driver.ChromeNoImage();
                perses.GoUrl("https://ccgen.co/");
            }
            catch (NoSuchElementException){}
            catch (StaleElementReferenceException){}
            catch (WebDriverException){}
            catch (NullReferenceException){}

            if (!perses.WaitForLoadElement(By.Id("gerar"), "ExceptionLoad"))
            {
                Restart();
            }
            else
            {
                FillForm();
            }
        }

        private void FillForm()
        {
            try
            {
                perses.FindElement(By.Id("ccpN")).SendKeys(bin);
                if (month != "Random")
                {
                     new SelectElement(perses.FindElement(By.Name("emeses"))).SelectByText(month);
                }
                if (year != "Random")
                {
                    new SelectElement(perses.FindElement(By.Name("eyear"))).SelectByText(year);
                }
                if (cvv != "Random" && cvv != "rnd")
                {
                    perses.FindElement(By.Id("eccv")).Clear();
                    perses.FindElement(By.Id("eccv")).SendKeys(cvv);
                }
                perses.FindElement(By.Name("ccghm")).Clear();
                perses.FindElement(By.Name("ccghm")).SendKeys(quantity);
                perses.FindElement(By.Id("gerar")).Click();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (!perses.WaitForGetAttributeByElement(By.Id("output2"), "value", "ExceptionFillForm"))
            {
                Restart();
            }
            else
            {
                GetResult();
            }
        }

        private void GetResult()
        {
            try
            {
                Result = perses.FindElement(By.Id("output2")).GetAttribute("value");
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            if (Result.Length <= 0)
            {
                Restart();
            }
        }

        private void Restart()
        {
            perses.Kill();
            GenerateCards();
        }

        public void ExecuteThread()
        {
            GenerateCardsThread = new Thread(GenerateCards);
            GenerateCardsThread.Start();
        }

        public void KillThread()
        {
            try
            {
                GenerateCardsThread.Abort();
                GenerateCardsThread.Join();
                perses.Kill();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
        }
    }
}
