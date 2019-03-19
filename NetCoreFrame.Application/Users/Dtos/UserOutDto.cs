using iCTR.TB.Framework.Domain.Entities;

namespace NetCoreFrame.Application.Users.Dtos
{
    /// <summary>
    /// 用户输出实体
    /// </summary>
    public class UserOutDto : EntityDto<string>
    {
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
    }
}