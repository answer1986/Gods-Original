using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

internal static class Driver
{
    public static IWebDriver Chrome()
    {
        try
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>
        {
            "--headless",
            "--incognito",
            "--test-type",
            "--window-size=1920,1080",
            "--no-sandbox",
            "--disable-plugins",
            "--disable-cache",
            "--disable-keep-alive",
            "--disable-hang-monitor",
            "--disable-web-security",
            "--disable-extensions",
            "--disable-application-cache",
            "--disable-popup-blocking",
            "--disable-default-apps",
            "--disable-dev-shm-usage",
            "--disable-gpu",
            "--ignore-certificate-errors"
        });
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;
            return new ChromeDriver(chromeDriverService, chromeOptions);
        }
        catch (NoSuchElementException) { }
        catch (StaleElementReferenceException) { }
        catch (WebDriverException) { }
        catch (NullReferenceException) { }
        return null;
    }

    public static IWebDriver ChromeNoImage()
    {
        try
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>
        {
            "--headless",
            "--incognito",
            "--blink-settings=imagesEnabled=false",
            "--test-type",
            "--no-sandbox",
            "--disable-plugins",
            "--disable-cache",
            "--disable-keep-alive",
            "--disable-hang-monitor",
            "--disable-web-security",
            "--disable-extensions",
            "--disable-application-cache",
            "--disable-popup-blocking",
            "--disable-default-apps",
            "--disable-dev-shm-usage",
            "--disable-gpu",
            "--ignore-certificate-errors"
        });
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;
            return new ChromeDriver(chromeDriverService, chromeOptions);
        }
        catch (NoSuchElementException) { }
        catch (StaleElementReferenceException) { }
        catch (WebDriverException) { }
        catch (NullReferenceException) { }
        return null;
    }

    public static IWebDriver Gecko()
    {
        FirefoxOptions geckoOptions = new FirefoxOptions();
        geckoOptions.AddArguments(new List<string> {
                        "-headless",
            "-private",
            //"--blink-settings=imagesEnabled=false",
            //"-test-type",
            //"-window-size=1920,1080",
            //"-no-sandbox",
            //"-disable-plugins",
            //"-disable-cache",
            //"-disable-keep-alive",
            //"-disable-hang-monitor",
            //"-disable-web-security",
            //"-disable-extensions",
            //"-disable-application-cache",
            //"-disable-popup-blocking",
            //"-disable-default-apps",
            //"-disable-dev-shm-usage",
            //"-disable-gpu",
            //"-ignore-certificate-errors"
        });

        FirefoxDriverService geckoDriverService = FirefoxDriverService.CreateDefaultService();
        geckoDriverService.SuppressInitialDiagnosticInformation = true;
        geckoDriverService.HideCommandPromptWindow = true;
        return new FirefoxDriver(geckoDriverService, geckoOptions);
    }

    public static void GoUrl(this IWebDriver browser, string url)
    {
        browser.Navigate().GoToUrl(url);
    }

    public static IWebElement RemoveElement(this IWebElement element)
    {
        IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)wrappedDriver;
        javaScriptExecutor.ExecuteScript("arguments[0].remove()", element);
        return element;
    }

    public static IWebElement SetAtrribute(this IWebElement element, string attribute, string value)
    {
        IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)wrappedDriver;
        javaScriptExecutor.ExecuteScript("arguments[0].setAttribute(arguments[1],arguments[2])", element, attribute, value);
        return element;
    }

    public static IWebElement RemoveAtrribute(this IWebElement element, string attribute)
    {
        IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)wrappedDriver;
        javaScriptExecutor.ExecuteScript("arguments[0].removeAttribute(arguments[1])", element, attribute);
        return element;
    }

    public static IWebElement InnerHtml(this IWebElement element, string text)
    {
        IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)wrappedDriver;
        javaScriptExecutor.ExecuteScript("arguments[0].innerHTML=arguments[1]", element, text);
        return element;
    }

    public static IWebElement InnerText(this IWebElement element, string text)
    {
        IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)wrappedDriver;
        javaScriptExecutor.ExecuteScript("arguments[0].innerText=arguments[1]", element, text);
        return element;
    }

    public static IWebElement JsClick(this IWebElement element)
    {
        try
        {
            IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
            IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)wrappedDriver;
            javaScriptExecutor.ExecuteScript("arguments[0].click()", element);
            return element;
        }
        catch (NullReferenceException)
        {
            return element;
        }
    }

    public static IWebElement Hover(this IWebElement element)
    {
        IWebDriver wrappedDriver = ((IWrapsDriver)element).WrappedDriver;
        Actions actions = new Actions(wrappedDriver);
        actions.MoveToElement(element).Perform();
        return element;
    }

    public static void SelectBySubText(this SelectElement element, string subText)
    {
        try
        {
            using (IEnumerator<IWebElement> enumerator = element.Options.Where((IWebElement option) => option.Text.Contains(subText)).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    IWebElement current = enumerator.Current;
                    current.Click();
                    return;
                }
            }
            element.SelectByText(subText);
        }
        catch (NullReferenceException) { }
    }

    public static IWebElement FindElementBySubText(this List<IWebElement> elements, string subText)
    {
        try
        {
            foreach (IWebElement element in elements)
            {
                if (element.Text.Contains(subText))
                {
                    return element;
                }
            }
        }
        catch (NullReferenceException)
        {
        }
        return null;
    }

    public static IWebElement FindElementByText(this List<IWebElement> elements, string text)
    {
        try
        {
            foreach (IWebElement element in elements)
            {
                if (element.Text == text)
                {
                    return element;
                }
            }
        }
        catch (NullReferenceException) { }
        return null;
    }

    public static void WaitForAjax(this IWebDriver driver, int timeoutSecs = 10, bool throwException = false)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            if ((bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"))
            {
                return;
            }
            Thread.Sleep(1000);
        }
        if (throwException)
        {
            throw new Exception("WebDriver timed out waiting for AJAX call to complete");
        }
    }

    public static void WaitForLoad(this IWebDriver browser, int timeoutSecs = 10, bool throwException = false)
    {
        Thread.Sleep(5000);
        for (int i = 0; i < timeoutSecs; i++)
        {
            string a = (string)((IJavaScriptExecutor)browser).ExecuteScript("return document.readyState");
            if (a == "complete")
            {
                return;
            }
            Thread.Sleep(1000);
        }
        if (throwException)
        {
            throw new Exception("WebDriver timed out waiting for Load to complete");
        }
    }

    public static bool WaitForLoadElement(this IWebDriver browser, By by, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                if (browser.FindElement(by).Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static bool WaitForDisappearElement(this IWebDriver browser, By by, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                if (!browser.FindElement(by).Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static bool WaitForAlertPresent(this IWebDriver browser, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                browser.SwitchTo().Alert();
                return true;
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static bool WaitForGetAttributeByElement(this IWebDriver browser, By by, string attribute, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                if (browser.FindElement(by).GetAttribute(attribute).Length > 0)
                {
                    return true;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static bool WaitForTextContainsByElement(this IWebDriver browser, By by, string text, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                if (browser.FindElement(by).Text.Contains(text))
                {
                    return true;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static bool WaitForTextEqualsByElement(this IWebDriver browser, By by, string text, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                if (browser.FindElement(by).Text.Equals(text))
                {
                    return true;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static void Screenshot(this IWebDriver browser, string image)
    {
        try
        {
            Screenshot screenshot = ((ITakesScreenshot)browser).GetScreenshot();
            screenshot.SaveAsFile(image + ".jpg", ScreenshotImageFormat.Jpeg);
        }
        catch (NoSuchElementException) { }
        catch (StaleElementReferenceException) { }
        catch (WebDriverException) { }
        catch (NullReferenceException) { }
    }

    public static void Refresh(this IWebDriver browser)
    {
        try
        {
            browser.Navigate().Refresh();
        }
        catch (NoSuchElementException) { }
        catch (StaleElementReferenceException) { }
        catch (WebDriverException) { }
        catch (NullReferenceException) { }
    }

    public static void Back(this IWebDriver browser)
    {
        try
        {
            browser.Navigate().Back();
        }
        catch (NoSuchElementException) { }
        catch (StaleElementReferenceException) { }
        catch (WebDriverException) { }
        catch (NullReferenceException) { }
    }

    public static void Kill(this IWebDriver browser)
    {
        try
        {
            browser.Close();
            browser.Quit();
            browser.Dispose();
        }
        catch (NoSuchElementException) { }
        catch (StaleElementReferenceException) { }
        catch (WebDriverException) { }
        catch (NullReferenceException) { }
    }

    public static bool ElementIsVisible(this IWebDriver browser, By by, string method = "ThrowException", int timeoutSecs = 10)
    {
        WebDriverWait webDriverWait = new WebDriverWait(browser, TimeSpan.FromSeconds(timeoutSecs));
        return webDriverWait.Until(delegate
        {
            try
            {
                return browser.FindElement(by).Displayed;
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            //Screenshot(browser, method);
            return false;
        });
    }

    public static IWebElement ElementVisible(this IWebDriver browser, By by, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                IWebElement webElement = browser.FindElement(by);
                if (webElement.Displayed)
                {
                    return webElement;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return null;
    }

    public static IWebElement ElementRandomVisible(this IWebDriver browser, By by, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                IWebElement webElement = (from x in browser.FindElements(@by)
                                          orderby Guid.NewGuid()
                                          select x).FirstOrDefault();
                if (webElement.Displayed)
                {
                    return webElement;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return null;
    }

    public static IWebElement ElementRandomTextContains(this IWebDriver browser, By by, string textContains, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                IWebElement webElement = (from x in browser.FindElements(@by)
                                          orderby Guid.NewGuid()
                                          select x).FirstOrDefault();
                if (webElement.Text.Contains(textContains))
                {
                    return webElement;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return null;
    }



    public static IWebElement ElementTextContains(this IWebDriver browser, By by, string textContains, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                IWebElement webElement = browser.FindElement(by);
                if (webElement.Text.Contains(textContains))
                {
                    return webElement;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }

            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return null;
    }

    public static IWebElement ElementTextEquals(this IWebDriver browser, By by, string textEquals, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                IWebElement webElement = browser.FindElement(by);
                if (webElement.Text.Equals(textEquals))
                {
                    return webElement;
                }
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return null;
    }

    public static bool ElementDisappear(this IWebDriver browser, By by, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                if (!browser.FindElement(by).Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException) { return true; }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return false;
    }

    public static IAlert AlertPresent(this IWebDriver browser, string method = "ThrowException", int timeoutSecs = 10)
    {
        for (int i = 0; i < timeoutSecs; i++)
        {
            try
            {
                return browser.SwitchTo().Alert();
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverException) { }
            catch (NullReferenceException) { }
            Thread.Sleep(1000);
        }
        //Screenshot(browser, method);
        return null;
    }

    public static bool IsNotNull(this IWebElement element)
    {
        if (element == null)
        {
            return false;
        }
        return true;
    }
}
