using RazorEngine.Text;

namespace BolgerUtils.Framework
{
    public static class Utils
    {
        // http://stackoverflow.com/a/16533897
        // https://github.com/Antaris/RazorEngine/pull/105
        // https://github.com/todthomson/RazorEngine/commit/ac66247674ac6405feac421ee972bb96a574ff19
        public static class Html
        {
            /// <summary>The same Raw method that is in TemplateBase.</summary>
            public static IEncodedString Raw(string rawString) => new RawString(rawString);
        }
    }
}
