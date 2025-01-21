using EmaptaLoginAutomation.Enums;
using OpenQA.Selenium;

namespace EmaptaLoginAutomation.Interfaces
{
    public interface IComponentService
    {
        IWebElement? GetComponent(string elementName, GetBy by, int timeOut = 3_000);
    }
}
