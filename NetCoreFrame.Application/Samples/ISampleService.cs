using System;
using iCTR.TB.Framework.Application;
using iCTR.TB.Framework.Domain.Entities;
using NetCoreFrame.Application.Samples.Dtos;

namespace NetCoreFrame.Application.Samples
{
    /// <summary>
    /// 示例服务接口
    /// </summary>
    public interface ISampleService : IService
    {
        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="samplePageInDto">分页查询实体</param>
        /// <returns>分页示例数据列表</returns>
        PageOutDtoBase<SampleOutDto> GetSamplesByPage(SamplePageInDto samplePageInDto);

        /// <summary>
        /// 新增一条示例数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        SampleUpdateDto CreateSample(SampleCreateDto dto);

        /// <summary>
        /// 根据主键值获取一条示例数据
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>实体</returns>
        SampleOutDto GetSample(Guid id);

        /// <summary>
        /// 更新一条示例数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        SampleUpdateDto UpdateSample(SampleUpdateDto dto);

        /// <summary>
        /// 部分更新一条示例数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        SampleUpdateDto PatchSample(Guid id, SamplePatchUpdateDto dto);

        /// <summary>
        /// 删除一条示例数据
        /// </summary>
        /// <param name="id">主键值</param>
        void DeleteSample(Guid id);
    }
}