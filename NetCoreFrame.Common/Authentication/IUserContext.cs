namespace NetCoreFrame.Common.Authentication
{
    /// <summary>
    /// 用户上下文管理
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 用户被授权的角色名称
        /// </summary>
        string RoleName { get; }
    }
}