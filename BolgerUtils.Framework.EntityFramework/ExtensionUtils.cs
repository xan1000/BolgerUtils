using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BolgerUtils.Framework.EntityFramework
{
    public static class ExtensionUtils
    {
        #region DbContext

        // Note this extension method is usually used in conjunction with EF.
        public static void LogSql(this DbContext item) => item.Database.Log = s => Debug.WriteLine(s);

        // Note this extension method is usually used in conjunction with EF.
        public static int PropertyMaximumLength(this DbContext item, string entitySet, string property)
        {
            var objectContext = ((IObjectContextAdapter) item).ObjectContext;
            var container = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName,
                DataSpace.CSpace);

            // ReSharper disable once PossibleInvalidOperationException
            return container.EntitySets[entitySet].ElementType.Properties[property].MaxLength.Value;
        }

        #endregion

        #region String

        public static SqlConnectionStringBuilder ToSqlConnectionStringBuilder(this string item) =>
            new SqlConnectionStringBuilder(item);

        #endregion
    }
}
