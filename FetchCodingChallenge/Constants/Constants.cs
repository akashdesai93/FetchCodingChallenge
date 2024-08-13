namespace FetchCodingChallenge.Constants
{
    public static class Constants
    {
        public const string GoldBarWeighingWebsiteUrl = "http://sdetchallenge.fetch.com/";
        public static class XPathConstants
        {
            public const string ResetButton = "//div/button[@id='reset' and text()='Reset']";
            public const string WeightButton = "//div/button[@id='weigh']";
        }
        public static class SymbolConstants
        {
            public const string GreaterThan = ">";
            public const string LessThan = "<";
            public const string Equal = "=";
        }
    }
}
