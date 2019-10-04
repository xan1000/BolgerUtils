using System.IO;
using RazorEngine;
using RazorEngine.Templating;

namespace BolgerUtils.Framework.Razor
{
    public static class Utils
    {
        // https://stackoverflow.com/questions/26862336/how-to-make-intellisense-works-with-razorengine
        // https://stackoverflow.com/a/27671174/9798310
        public static string Parse<T>(FileInfo fileInfo, T model = null, DynamicViewBag viewBag = null)
            where T : class => Engine.Razor.Parse(fileInfo, model, viewBag);
    }
}
