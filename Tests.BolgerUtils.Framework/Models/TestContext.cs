using System.Data.Entity;

namespace Tests.BolgerUtils.Framework.Models
{
    public class TestContext: DbContext
    {
        public TestContext() : base(@"Server=.\SQLEXPRESS;Database=Test;Trusted_Connection=True")
        { }

        // ReSharper disable once MemberCanBePrivate.Global
        public DbSet<Person> People { get; set; }
    }
}
