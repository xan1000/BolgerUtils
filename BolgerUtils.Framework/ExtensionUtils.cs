using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using RazorEngine.Templating;

namespace BolgerUtils.Framework
{
    public static class ExtensionUtils
    {
        #region DbContext

        // Note this extension method is usually used in conjunction with EF.
        public static void LogSql(this DbContext context) => context.Database.Log = s => Debug.WriteLine(s);

        // Note this extension method is usually used in conjunction with EF.
        public static int PropertyMaximumLength(this DbContext context, string entitySet, string property)
        {
            var objectContext = ((IObjectContextAdapter) context).ObjectContext;
            var container = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName,
                DataSpace.CSpace);

            // ReSharper disable once PossibleInvalidOperationException
            return container.EntitySets[entitySet].ElementType.Properties[property].MaxLength.Value;
        }

        #endregion

        #region IRazorEngineService

        public static string Parse<T>(this IRazorEngineService service, FileInfo templateFileInfo, T model = null,
            DynamicViewBag viewBag = null) where T : class =>
            service.Parse(templateFileInfo, model.GetType(), model, viewBag);

        public static string Parse(this IRazorEngineService service, FileInfo templateFileInfo,
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

            return service.IsTemplateCached(templateKey, modelType) ?
                service.Run(templateKey, modelType, model, viewBag) :
                service.RunCompile(File.ReadAllText(templateFileInfo.FullName), templateKey, modelType, model, viewBag);
        }

        #endregion

        #region String

        public static SqlConnectionStringBuilder SqlConnectionStringBuilder(this string connectionString) =>
            new SqlConnectionStringBuilder(connectionString);

        #endregion
    }
}
