using System.IO;
using RazorEngine;
using RazorEngine.Templating;

namespace BolgerUtils.Framework.Razor
{
    public static class Utils
    {
        public static string Parse<T>(FileInfo fileInfo, T model = null, DynamicViewBag viewBag = null)
            where T : class => Engine.Razor.Parse(fileInfo, model, viewBag);
    }
}
