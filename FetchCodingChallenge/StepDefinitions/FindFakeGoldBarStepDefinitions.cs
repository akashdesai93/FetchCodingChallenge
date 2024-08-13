using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FetchCodingChallenge.StepDefinitions;
[Binding]
public class FindFakeGoldBarStepDefinitions
{
    private IWebDriver _driver;

    [Given(@"I have navigated to the gold bar weighing website")]
    public void GivenIHaveNavigatedToTheGoldBarWeighingWebsite()
    {
        _driver = new ChromeDriver();
        _driver.Navigate().GoToUrl("http://sdetchallenge.fetch.com/");
    }

    [When(@"I find the fake gold bar")]
    public void WhenIFindTheFakeGoldBar()
    {
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        // Step 1: Weigh group 1 (0, 1, 2) against group 2 (3, 4, 5)
        _driver.FindElement(By.XPath("//input[@id='left_0']")).SendKeys("0");
        _driver.FindElement(By.XPath("//input[@id='left_1']")).SendKeys("1");
        _driver.FindElement(By.XPath("//input[@id='left_2']")).SendKeys("2");

        _driver.FindElement(By.XPath("//input[@id='right_0']")).SendKeys("3");
        _driver.FindElement(By.XPath("//input[@id='right_1']")).SendKeys("4");
        _driver.FindElement(By.XPath("//input[@id='right_2']")).SendKeys("5");

        _driver.FindElement(By.XPath("//button[@id='weigh']")).Click();
        Thread.Sleep(200); // Wait for the result to appear
        var resultText = wait.Until(driver => driver.FindElement(By.XPath("//ol/li[1]"))).Text;
        int[] groupToWeigh;

        // Step 2: Determine which group contains the fake bar
        if (resultText.Contains(">")) // Left side heavier, fake bar is in group 2 (3, 4, 5)
        {
            groupToWeigh = new int[] { 3, 4, 5 };
        }
        else if (resultText.Contains("<")) // Right side heavier, fake bar is in group 1 (0, 1, 2)
        {
            groupToWeigh = new int[] { 0, 1, 2 };
        }
        else // Both sides equal, fake bar is in group 3 (6, 7, 8)
        {
            groupToWeigh = new int[] { 6, 7, 8 };
        }

        // Step 3: Reset the scale
        ResetScale();

        // Step 4: Weigh two bars from the suspected group
        _driver.FindElement(By.XPath("//input[@id='left_0']")).SendKeys(groupToWeigh[0].ToString());
        _driver.FindElement(By.XPath("//input[@id='right_0']")).SendKeys(groupToWeigh[1].ToString());

        _driver.FindElement(By.XPath("//button[@id='weigh']")).Click();
        Thread.Sleep(3000); // Wait for the result to appear
        resultText = _driver.FindElement(By.XPath("//ol/li[2]")).Text;

        int fakeBar;

        if (resultText.Contains(">")) // Left side heavier, fake bar is the second one
        {
            fakeBar = groupToWeigh[1];
        }
        else if (resultText.Contains("<")) // Right side heavier, fake bar is the first one
        {
            fakeBar = groupToWeigh[0];
        }
        else // Both sides equal, fake bar is the third one
        {
            fakeBar = groupToWeigh[2];
        }

        // Store the identified fake bar index for the next step
        ScenarioContext.Current["FakeBarIndex"] = fakeBar;
    }



    [When(@"I click on the correct gold bar button")]
    public void WhenIClickOnTheCorrectGoldBarButton()
    {
        int fakeBarIndex = (int)ScenarioContext.Current["FakeBarIndex"];
        Console.WriteLine("Fake Bar Index: " + fakeBarIndex);
        _driver.FindElement(By.XPath($"//button[@id='coin_{fakeBarIndex}']")).Click();
    }

    [Then(@"I should see the message ""(.*)""")]
    public void ThenIShouldSeeTheMessage(string expectedMessage)
    {
        IAlert alert = _driver.SwitchTo().Alert();
        string alertText = alert.Text;
        alert.Accept();

        if (alertText.Equals(expectedMessage))
        {
            Console.WriteLine("Correct message displayed: " + expectedMessage);
        }
        else
        {
            throw new Exception($"Expected message '{expectedMessage}' but got '{alertText}'");
        }
    }
    private void ResetScale()
    {
        // Attempt to reset the scale, ensuring the correct reset button is clicked
        IWebElement resetButton = _driver.FindElement(By.XPath("//div/button[@id='reset' and text()='Reset']"));

        // Ensure the button is clickable before clicking
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => resetButton.Enabled);

        resetButton.Click();
        Console.WriteLine("Reset Button Clicked");
    }

    //[AfterScenario]
    //public void CleanUp()
    //{
    //    _driver.Quit();
    //}
}
