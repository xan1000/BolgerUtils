namespace Tests.BolgerUtils.Framework.Razor.Models
{
    public class TestModel
    {
        public TestModel(int loopCount = 0, string text = null, bool shouldOutputRawString = false)
        {
            LoopCount = loopCount;
            Text = text;
            ShouldOutputRawString = shouldOutputRawString;
        }

        public bool IsDisplayed => LoopCount > 0;
        public int LoopCount { get; }
        public string Text { get; }
        public bool ShouldOutputRawString { get; }
    }
}
