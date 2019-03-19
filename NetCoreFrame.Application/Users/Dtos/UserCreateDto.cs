namespace NetCoreFrame.Application.Users.Dtos
{
    /// <summary>
    /// 用户新增实体
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}