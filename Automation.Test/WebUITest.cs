using Automation.UI;
using Xunit;

namespace Automation.Test
{
    public class WebUiTest
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
