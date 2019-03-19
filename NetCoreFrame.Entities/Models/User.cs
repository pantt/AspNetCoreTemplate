using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Entities.Models
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : Entity<string>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }
}