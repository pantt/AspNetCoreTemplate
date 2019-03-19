using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using iCTR.TB.Framework.Core.Extensions;
using iCTR.TB.Framework.Domain.Entities;
using NetCoreFrame.Application.Samples.Dtos;
using NetCoreFrame.Entities;
using NetCoreFrame.Entities.Models;
using NetCoreFrame.Entities.Repositories;

namespace NetCoreFrame.Application.Samples
{
    /// <summary>
    /// 示例服务实现
    /// </summary>
    public class SampleService : ISampleService
    {
        /// <summary>
        /// 示例仓储
        /// </summary>
        private readonly ISampleRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        public SampleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.SampleRepository;
            _userRepository = unitOfWork.UserRepository;
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="samplePageInDto">分页查询实体</param>
        /// <returns>分页查询结果</returns>
        public PageOutDtoBase<SampleOutDto> GetSamplesByPage(SamplePageInDto samplePageInDto)
        {
            var result = new PageOutDtoBase<SampleOutDto>();
            Expression<Func<Sample, bool>> predicate =
                d => d.Name.Contains(samplePageInDto.Name.IsNullOrEmpty() ? "" : samplePageInDto.Name) || d.Remark.Contains(samplePageInDto.Remark.IsNullOrEmpty() ? "" : samplePageInDto.Remark);
            var list = _repository.GetPaged(predicate, samplePageInDto.CurrentPageIndex, samplePageInDto.PageSize,
                samplePageInDto.SortField, samplePageInDto.SortBy);
            result.Data = Mapper.Map<List<SampleOutDto>>(list);
            result.Total = _repository.Count(predicate);
            return result;
        }

        /// <summary>
        /// 新增一条示例数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        public SampleUpdateDto CreateSample(SampleCreateDto dto)
        {
            using (_unitOfWork.BeginTransaction())
            {
                var entity = Mapper.Map<Sample>(dto);
                var entity2 = new User()
                {
                    Id = "123123122211111",
                    Age = 11,
                    Name = "测试"
                };
                entity.Id = Guid.NewGuid();
                var insertEntity = _repository.Insert(entity);
                _userRepository.Insert(entity2);
                _unitOfWork.Save();
                return Mapper.Map<SampleUpdateDto>(insertEntity);
            }
        }

        /// <summary>
        /// 根据主键值获取一条示例数据
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>实体</returns>
        public SampleOutDto GetSample(Guid id)
        {
            var entity = _repository.Get(id);
            return Mapper.Map<SampleOutDto>(entity);
        }

        /// <summary>
        /// 更新一条示例数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        public SampleUpdateDto UpdateSample(SampleUpdateDto dto)
        {
            var entity = Mapper.Map<Sample>(dto);
            _repository.Update(entity);
            return dto;
        }

        /// <summary>
        /// 部分更新一条示例数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        public SampleUpdateDto PatchSample(Guid id, SamplePatchUpdateDto dto)
        {
            //事务测试
            var entity = _repository.Update(id, dto);
            return Mapper.Map<SampleUpdateDto>(entity);
        }

        /// <summary>
        /// 删除一条示例数据
        /// </summary>
        /// <param name="id">主键值</param>
        public void DeleteSample(Guid id)
        {
            _repository.Delete(id);
        }
    }
}