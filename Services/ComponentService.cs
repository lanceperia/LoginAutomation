using EmaptaLoginAutomation.Enums;
using EmaptaLoginAutomation.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EmaptaLoginAutomation.Services
{
    public class ComponentService(ILoggerService logger, ChromeDriver driver) : IComponentService
    {
        public IWebElement? GetComponent(string elementName, GetBy by, int timeOut = 3_000)
        {
            try
            {
                By? byElement = null;

                switch (by)
                {
                    case GetBy.Id:
                        byElement = By.Id(elementName);
                        break;
                    case GetBy.Class:
                        byElement = By.ClassName(elementName);
                        break;
                    case GetBy.Tag:
                        byElement = By.TagName(elementName);
                        break;
                    case GetBy.XPath:
                        byElement = By.XPath(elementName);
                        break;
                    default:
                        break;
                }

                logger.Information($"Waiting for {elementName} to load...");

                var wait = new DefaultWait<IWebDriver>(driver)
                {
                    Timeout = TimeSpan.FromMilliseconds(timeOut),
                    PollingInterval = TimeSpan.FromMilliseconds(500),
                };
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));

                var element = wait.Until(drv =>
                {
                    var el = drv.FindElement(byElement);
                    return el.Displayed ? el : null;
                });

                logger.Information($"{elementName} found: {element is not null}");
                return element;
            }
            catch (Exception)
            {
                logger.Error($"{elementName} not found");
                return null;
            }
        }
    }
}
