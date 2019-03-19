using System;
using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Application.Samples.Dtos
{
    /// <summary>
    /// 示例输出DTO
    /// </summary>
    public class SampleOutDto : EntityDto<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}