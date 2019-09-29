namespace Tests.BolgerUtils.Framework.Razor.Models
{
    public class TestModel
    {
        public TestModel(int loopCount = 0, string text = null, bool isRawString = false)
        {
            IsRawString = isRawString;
            LoopCount = loopCount;
            Text = text;
        }

        public bool IsDisplayed => LoopCount > 0;
        public int LoopCount { get; }
        public string Text { get; }
        public bool IsRawString { get; }
    }
}
