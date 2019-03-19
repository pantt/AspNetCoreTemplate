using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Application.Users.Dtos
{
    /// <summary>
    /// 分页查询实体
    /// </summary>
    public class UserPageInDto : PageInDtoBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}