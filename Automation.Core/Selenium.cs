using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Xml;

namespace Automation.Core
{
    public class Selenium
    {
        public void RunProcess(string filenamePath, string driverPath)
        {
            try
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("--start-maximized");
                var chromeDriver = new ChromeDriver(driverPath, chromeOptions);

                int timeOutInSeconds = 3600; //1 hour

                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeOutInSeconds);
                chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(timeOutInSeconds);
                chromeDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(timeOutInSeconds);

                var xml = new XmlDocument();

                xml.Load(filenamePath);

                var actions = xml.SelectNodes("//Action");


                foreach (XmlNode action in actions)
                {
                    FillControl(action, chromeDriver);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CleanProcesses()
        {
            foreach (Process proc in Process.GetProcessesByName("chromedriver"))
            {
                proc.Kill();
            }
        }

        private void FillControl(XmlNode node, ChromeDriver chromeDriver)
        {
            try
            {
                if (node.Attributes["type"].Value.Equals("Navigation"))
                {
                    var url = node.Attributes["value"].Value;
                    chromeDriver.Navigate().GoToUrl(url);
                }
                else
                {
                    By control = GetControl(node);
                    IWebElement element = chromeDriver.FindElement(control);

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
                                ((IJavaScriptExecutor)chromeDriver).ExecuteScript("arguments[0].scrollIntoView();", element);
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

            if (node.Attributes["xpath"] != null)
            {
                id = node.Attributes["xpath"].Value;
                control = By.XPath(id);
            }

            return control;
        }

    }
}
