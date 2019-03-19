using System;
using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Entities.Models
{
    /// <summary>
    /// 示例
    /// </summary>
    public class Sample : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}