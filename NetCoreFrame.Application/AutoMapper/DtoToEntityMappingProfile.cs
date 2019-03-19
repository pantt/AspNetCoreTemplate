using AutoMapper;
using NetCoreFrame.Application.Samples.Dtos;
using NetCoreFrame.Application.Users.Dtos;
using NetCoreFrame.Entities.Models;

namespace NetCoreFrame.Application.AutoMapper
{
    /// <summary>
    /// Dto映射为实体
    /// </summary>
    public class DtoToEntityMappingProfile : Profile
    {
        /// <summary>
        /// 映射配置
        /// </summary>
        public DtoToEntityMappingProfile()
        {
            CreateMap<UserCreateDto, User>();
            CreateMap<UserOutDto, User>();
            CreateMap<SampleCreateDto, Sample>();
            CreateMap<SamplePatchUpdateDto, Sample>();
            CreateMap<SampleUpdateDto, Sample>();
        }
    }
}