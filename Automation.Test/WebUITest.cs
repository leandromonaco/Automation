using Automation.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace Automation.Test
{
    public class WebUITest
    {
        [Fact]
        public void SearchOnGoogle()
        {
            FirefoxWebAutomation firefoxWebAutomation = new FirefoxWebAutomation(@"C:\GitHub\Automation\Automation\Automation.Test\Source\GoogleSearch.xml", 
                                                                                 @"C:\GitHub\Automation\Automation\Automation.Test\Drivers\");
            firefoxWebAutomation.RunProcess();
        }
    }
}
