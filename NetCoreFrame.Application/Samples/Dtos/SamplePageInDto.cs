using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Application.Samples.Dtos
{
    /// <summary>
    /// 示例分页输出实体
    /// </summary>
    public class SamplePageInDto : PageInDtoBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplePageInDto()
        {
            this.SortField = "Name";
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}