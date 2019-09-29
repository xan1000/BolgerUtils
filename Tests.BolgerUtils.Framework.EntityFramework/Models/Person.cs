using System.ComponentModel.DataAnnotations;

namespace Tests.BolgerUtils.Framework.EntityFramework.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Person
    {
        // ReSharper disable once UnusedMember.Global
        public int PersonID { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        [MaxLength(60)]
        public string Name { get; set; }
    }
}
