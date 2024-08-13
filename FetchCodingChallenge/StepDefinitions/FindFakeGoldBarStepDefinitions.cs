using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static FetchCodingChallenge.Constants.Constants;

namespace FetchCodingChallenge.StepDefinitions
{
    [Binding]
    public class FindFakeGoldBarStepDefinitions
    {
        private IWebDriver? _driver;
        private WebDriverWait? _wait;

        [Given(@"I have navigated to the gold bar weighing website")]
        public void GivenIHaveNavigatedToTheGoldBarWeighingWebsite()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(GoldBarWeighingWebsiteUrl);
        }

        [When(@"I find the fake gold bar")]
        public void WhenIFindTheFakeGoldBar()
        {
            if (_driver == null || _wait == null)
            {
                throw new NullReferenceException("Driver or Wait is null");
            }
            // Step 1: Weigh group 1 (0, 1, 2) against group 2 (3, 4, 5)
            int[] firstGroup = { 0, 1, 2 };
            int[] secondGroup = { 3, 4, 5 };
            int[] suspectedGroup = WeighAndDetermineGroup(firstGroup, secondGroup);

            // Step 2: Reset the scale
            IWebElement resetButton = _driver.FindElement(By.XPath(XPathConstants.ResetButton));
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
            if (_driver == null)
            {
                throw new NullReferenceException("Driver is null");
            }
            int fakeBarIndex = (int)ScenarioContext.Current["FakeBarIndex"];
            Console.WriteLine("Fake Bar Index: " + fakeBarIndex);
            _driver.FindElement(By.XPath($"//button[@id='coin_{fakeBarIndex}']")).Click();
        }

        [Then(@"I should see the message ""(.*)""")]
        public void ThenIShouldSeeTheMessage(string expectedMessage)
        {
            if (_driver == null)
            {
                throw new NullReferenceException("Driver is null");
            }
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
            _driver?.Quit();
        }

        private int[] WeighAndDetermineGroup(int[] leftGroup, int[] rightGroup)
        {
            if (_driver == null)
            {
                throw new NullReferenceException("Driver is null");
            }

            // Use the combined method to enter values into the left and right bowls
            EnterValuesIntoBowl(leftGroup, "left");
            EnterValuesIntoBowl(rightGroup, "right");

            _driver.FindElement(By.XPath(XPathConstants.WeightButton)).Click();

            string weighingResultText = GetWeighingResultText(1);

            if (weighingResultText.Contains(SymbolConstants.GreaterThan))
                return rightGroup; // Left side is heavier, so the fake bar is in the right group
            else if (weighingResultText.Contains(SymbolConstants.LessThan))
                return leftGroup; // Right side is heavier, so the fake bar is in the left group
            else if (weighingResultText.Contains(SymbolConstants.Equal))
                return new int[] { 6, 7, 8 }; // Both sides are equal, so the fake bar is in the third group
            else
                throw new Exception($"Invalid weighing result: {weighingResultText}");
        }

        private void EnterValuesIntoBowl(int[] bars, string side)
        {
            if (bars == null || bars.Length == 0)
                throw new NullReferenceException("Bars array is null or empty");

            for (int i = 0; i < bars.Length; i++)
            {
                if (_driver == null)
                    throw new NullReferenceException("Driver is null");

                _driver.FindElement(By.XPath($"//input[@id='{side}_{i}']")).SendKeys(bars[i].ToString());
            }
        }
        private int FindFakeBarWithinGroup(int[] group)
        {
            if (_driver == null)
                throw new NullReferenceException("Driver is null");

            EnterValuesIntoBowl(new int[] { group[0] }, "left");
            EnterValuesIntoBowl(new int[] { group[1] }, "right");
            _driver.FindElement(By.XPath(XPathConstants.WeightButton)).Click();

            string weighingResultText = GetWeighingResultText(2);

            if (weighingResultText.Contains(SymbolConstants.GreaterThan))
                return group[1]; // Left side is heavier, so the fake bar is the second one
            else if (weighingResultText.Contains(SymbolConstants.LessThan))
                return group[0]; // Right side is heavier, so the fake bar is the first one
            else
                return group[2]; // Both sides are equal, so the fake bar is the third one
        }

        private string GetWeighingResultText(int resultIndex)
        {
            if (_wait == null)
            {
                throw new NullReferenceException("Driver is null");
            }
            return _wait.Until(driver => driver.FindElement(By.XPath($"//ol/li[{resultIndex}]"))).Text;
        }
    }
}
