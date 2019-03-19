using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iCTR.TB.Framework.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace NetCoreFrame.Common.Authentication
{
    /// <summary>
    /// 权限管理类
    /// </summary>
    public class AuthFactory
    {
        /// <summary>
        /// 配置文件信息实体
        /// </summary>
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtOptions">JWT配置文件信息实体</param>
        public AuthFactory(IOptions<JwtOptions> jwtOptions)
        {

            _jwtOptions = jwtOptions.Value ?? new JwtOptions();
        }

        /// <summary>
        /// 用户登录方法，包括权限声明和token返回
        /// </summary>
        /// <param name="userInfo">登录用户信息</param>
        /// <returns>返回token字符串</returns>
        public string SignIn(UserInfo userInfo)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(CtrClaimTypes.RoleName, userInfo.RoleName ?? ""),
                new Claim(CtrClaimTypes.UserId, userInfo.UserId ?? ""),
                new Claim(CtrClaimTypes.UserName, userInfo.UserName ?? ""),
                new Claim(CtrClaimTypes.DepName, userInfo.DepName ?? ""),
                new Claim(CtrClaimTypes.RoleName, userInfo.RoleName ?? ""),
                new Claim(CtrClaimTypes.Token, userInfo.Token ?? ""),
                new Claim(CtrClaimTypes.SignatureKey, userInfo.SignatureKey ?? "")
            };
            return CreateToken(claims, _jwtOptions.ExpiresMinutes);
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="claims">权限声明</param>
        /// <param name="expiresMinutes">过期分钟数</param>
        /// <returns>返回token字符串</returns>
        private string CreateToken(Claim[] claims, double expiresMinutes)
        {
            var now = DateTime.UtcNow;
            var symmetricKeyAsBase64 = _jwtOptions.SymmetricKeyAsBase64;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                now,
                now.Add(TimeSpan.FromMinutes(expiresMinutes)),
                signingCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}