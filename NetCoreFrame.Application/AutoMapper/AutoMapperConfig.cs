using AutoMapper;

namespace NetCoreFrame.Application.AutoMapper
{
    /// <summary>
    /// AutoMapper映射配置
    /// </summary>
    public class AutoMapperConfig
    {
        /// <summary>
        /// 注册映射方法
        /// </summary>
        /// <returns>映射配置</returns>
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToDtoMappingProfile());
                cfg.AddProfile(new DtoToEntityMappingProfile());
            });
        }
    }
}