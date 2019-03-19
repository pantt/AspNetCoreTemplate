using AutoMapper;
using NetCoreFrame.Application.Samples.Dtos;
using NetCoreFrame.Application.Users.Dtos;
using NetCoreFrame.Entities.Models;

namespace NetCoreFrame.Application.AutoMapper
{
    /// <summary>
    /// 实体映射为DTO配置
    /// </summary>
    public class EntityToDtoMappingProfile : Profile
    {
        /// <summary>
        /// 映射配置
        /// </summary>
        public EntityToDtoMappingProfile()
        {
            CreateMap<User, UserOutDto>();
            CreateMap<User, UserOutDto>();
            CreateMap<Sample, SampleOutDto>();
            CreateMap<Sample, SampleUpdateDto>();
        }
    }
}