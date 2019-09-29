using System;
using System.IO;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace BolgerUtils.Framework.Razor
{
    public static class ExtensionUtils
    {
        #region IRazorEngineService

        public static string Parse<T>(this IRazorEngineService item, FileInfo templateFileInfo, T model = null,
            DynamicViewBag viewBag = null) where T : class =>
            item.Parse(templateFileInfo, model.GetType(), model, viewBag);

        public static string Parse(this IRazorEngineService item, FileInfo templateFileInfo,
            Type modelType = null, object model = null, DynamicViewBag viewBag = null)
        {
            // A bit of magic is required with how templates are complied and re-complied due to memory release issues
            // regarding assemblies - namely assemblies cannot be unloaded within an AppDomain.
            // 
            // There are a few solutions here but I have opted to use the template file name and last write time as the
            // template key which pretty much solves the issue for the most part here (there is still technically memory
            // leaks between template changes but its not significant and edits will only happen when actively developing).
            // 
            // Note that when the Application Pool naturally refreshes all memory leaks will then be taken care of.
            var templateKey = templateFileInfo.Name + templateFileInfo.LastWriteTimeUtc.ToBinary();

            return item.IsTemplateCached(templateKey, modelType) ?
                item.Run(templateKey, modelType, model, viewBag) :
                item.RunCompile(File.ReadAllText(templateFileInfo.FullName), templateKey, modelType, model, viewBag);
        }

        #endregion

        #region String

        // http://stackoverflow.com/a/16533897
        // https://github.com/Antaris/RazorEngine/pull/105
        // https://github.com/todthomson/RazorEngine/commit/ac66247674ac6405feac421ee972bb96a574ff19
        /// <summary>The same Raw method that is in TemplateBase.</summary>
        public static IEncodedString ToRawString(this string item) => new RawString(item);

        #endregion
    }
}
