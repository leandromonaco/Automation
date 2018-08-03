using System;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Automation.UI
{
    public abstract class BaseWebAutomation
    {
        public RemoteWebDriver Driver { get; set; }
        public DriverOptions Options { get; set; }
        public string TestPath { get; set; }

        public void RunProcess()
        {
            try
            {
                int timeOutInSeconds = 3600; //1 hour

                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutInSeconds);
                Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeOutInSeconds);
                Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(timeOutInSeconds);

                var xml = new XmlDocument();

                xml.Load(TestPath);

                var actions = xml.SelectNodes("//Action");


                foreach (XmlNode action in actions)
                {
                    Thread.Sleep(2000);
                    FillControl(action, Driver);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Clean(string processName)
        {
            foreach (Process proc in Process.GetProcessesByName(processName))
            {
                proc.Kill();
            }
        }

        private void FillControl(XmlNode node, RemoteWebDriver driver)
        {
            try
            {
                if (node.Attributes["type"].Value.Equals("Navigation"))
                {
                    var url = node.Attributes["value"].Value;
                    driver.Navigate().GoToUrl(url);
                }
                else
                {
                    By control = GetControl(node);
                    IWebElement element = driver.FindElement(control);

                    try
                    {
                        var submit = node.Attributes["submit"] != null;
                        var value = node.Attributes["value"] != null ? node.Attributes["value"].Value : string.Empty;

                        switch (node.Attributes["type"].Value)
                        {
                            case "Checkbox":
                                element.Click();
                                break;
                            case "Button":
                            case "Link":
                            case "RadioButton":
                                element.Click();
                                break;
                            case "Combobox":
                                element.SendKeys(value);
                                break;
                            case "Textbox":
                                element.Clear();
                                element.SendKeys(value);
                                if (submit)
                                {
                                    element.SendKeys(Keys.Return);
                                }
                                break;
                            case "ScrollTo":
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView();", element);
                                break;
                                //case "Screenshot":
                                //    TakeScreenshot(id, chromeDriver);
                                //    break;
                        }
                    }
                    catch (Exception)
                    {
                        //Fix: element not visible
                        //((IJavaScriptExecutor)chromeDriver).ExecuteScript("arguments[0].checked = true;", element);
                        //MessageBox.Show(ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private By GetControl(XmlNode node)
        {
            By control = null;
            string id;

            if (node.Attributes["id"] != null)
            {
                id = node.Attributes["id"].Value;
                control = By.Id(id);
            }

            if (node.Attributes["name"] != null)
            {
                id = node.Attributes["name"].Value;
                control = By.Name(id);
            }

            if (node.Attributes["LinkText"] != null)
            {
                id = node.Attributes["LinkText"].Value;
                control = By.LinkText(id);
            }

            if (node.Attributes["xpath"] != null)
            {
                id = node.Attributes["xpath"].Value;
                control = By.XPath(id);
            }

            return control;
        }
    }
}
