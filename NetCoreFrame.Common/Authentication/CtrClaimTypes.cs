namespace NetCoreFrame.Common.Authentication
{
    /// <summary>
    /// 自定义凭据标识
    /// </summary>
    public class CtrClaimTypes
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public const string UserId = "http://www.ctrchina.cn/ws/2017/identity/clamis/userid";

        /// <summary>
        /// 用户名称
        /// </summary>
        public const string UserName = "http://www.ctrchina.cn/ws/2017/identity/clamis/username";

        /// <summary>
        /// 用户区域Id
        /// </summary>
        public const string AreaBaseId = "http://www.ctrchina.cn/ws/2017/identity/clamis/areabaseId";

        /// <summary>
        /// 用户所属部门名称
        /// </summary>
        public const string DepName = "http://www.ctrchina.cn/ws/2017/identity/clamis/depname";

        /// <summary>
        /// 用户角色
        /// </summary>
        public const string RoleName = "http://www.ctrchina.cn/ws/2017/identity/clamis/rolename";

        /// <summary>
        /// 用户token
        /// </summary>
        public const string Token = "http://www.ctrchina.cn/ws/2017/identity/clamis/token";

        /// <summary>
        /// AES加密密钥
        /// </summary>
        public const string SignatureKey = "http://www.ctrchina.cn/ws/2017/identity/clamis/signatureKey";
    }
}