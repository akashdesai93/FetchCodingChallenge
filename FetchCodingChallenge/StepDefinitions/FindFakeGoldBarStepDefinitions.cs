using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FetchCodingChallenge.StepDefinitions
{
    [Binding]
    public class FindFakeGoldBarStepDefinitions
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [Given(@"I have navigated to the gold bar weighing website")]
        public void GivenIHaveNavigatedToTheGoldBarWeighingWebsite()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl("http://sdetchallenge.fetch.com/");
        }

        [When(@"I find the fake gold bar")]
        public void WhenIFindTheFakeGoldBar()
        {
            // Step 1: Weigh group 1 (0, 1, 2) against group 2 (3, 4, 5)
            int[] firstGroup = { 0, 1, 2 };
            int[] secondGroup = { 3, 4, 5 };
            int[] suspectedGroup = WeighAndDetermineGroup(firstGroup, secondGroup);

            // Step 2: Reset the scale
            IWebElement resetButton = _driver.FindElement(By.XPath("//div/button[@id='reset' and text()='Reset']"));
            _wait.Until(driver => resetButton.Enabled);
            resetButton.Click();

            // Step 3: Weigh two bars from the suspected group
            int fakeBarIndex = FindFakeBarWithinGroup(suspectedGroup);

            // Store the identified fake bar index for the next step
            ScenarioContext.Current["FakeBarIndex"] = fakeBarIndex;
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

        [AfterScenario]
        public void CleanUp()
        {
            _driver.Quit();
        }
        private int[] WeighAndDetermineGroup(int[] leftGroup, int[] rightGroup)
        {
            EnterValuesIntoLeftBowl(leftGroup);
            EnterValuesIntoRightBowl(rightGroup);
            _driver.FindElement(By.XPath("//button[@id='weigh']")).Click();

            string weighingResultText = GetWeighingResultText(1);

            if (weighingResultText.Contains(">"))
                return rightGroup; // Left side is heavier, so the fake bar is in the right group
            else if (weighingResultText.Contains("<"))
                return leftGroup; // Right side is heavier, so the fake bar is in the left group
            else
                return new int[] { 6, 7, 8 }; // Both sides are equal, so the fake bar is in the third group
        }
        private void EnterValuesIntoLeftBowl(int[] bars)
        {
            for (int i = 0; i < bars.Length; i++)
            {
                _driver.FindElement(By.XPath($"//input[@id='left_{i}']")).SendKeys(bars[i].ToString());
            }
        }

        private void EnterValuesIntoRightBowl(int[] bars)
        {
            for (int i = 0; i < bars.Length; i++)
            {
                _driver.FindElement(By.XPath($"//input[@id='right_{i}']")).SendKeys(bars[i].ToString());
            }
        }
        private int FindFakeBarWithinGroup(int[] group)
        {
            EnterValuesIntoLeftBowl(new int[] { group[0] });
            EnterValuesIntoRightBowl(new int[] { group[1] });
            _driver.FindElement(By.XPath("//button[@id='weigh']")).Click();

            string weighingResultText = GetWeighingResultText(2);

            if (weighingResultText.Contains(">"))
                return group[1]; // Left side is heavier, so the fake bar is the second one
            else if (weighingResultText.Contains("<"))
                return group[0]; // Right side is heavier, so the fake bar is the first one
            else
                return group[2]; // Both sides are equal, so the fake bar is the third one
        }

        private string GetWeighingResultText(int resultIndex)
        {
            return _wait.Until(driver => driver.FindElement(By.XPath($"//ol/li[{resultIndex}]"))).Text;
        }
    }
}
