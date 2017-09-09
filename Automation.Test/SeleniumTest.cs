using Automation.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.Test
{
    [TestClass]
    public class SeleniumTest
    {
        Selenium _selenium = new Selenium();

        [TestInitialize]
        public void SetUp()
        {
            _selenium.CleanProcesses();
        }

        [TestMethod]
        public void SearchOnGoogle()
        {
            _selenium.RunProcess(@"C:\Source\Automation\Automation.Test\Source\GoogleSearch.xml", @"C:\Source\Automation\Automation.Test\Drivers\");
        }
    }
}
