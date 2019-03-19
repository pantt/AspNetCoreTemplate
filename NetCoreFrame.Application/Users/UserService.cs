using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using iCTR.TB.Framework.Domain.Entities;
using NetCoreFrame.Application.Users.Dtos;
using NetCoreFrame.Entities;
using NetCoreFrame.Entities.Models;
using NetCoreFrame.Entities.Repositories;
using iCTR.TB.Framework.Core.Extensions;
using System;
using System.Transactions;

namespace NetCoreFrame.Application.Users
{
    /// <summary>
    /// 服务层：用户信息
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// automapper实例
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 用户数据仓储实例
        /// </summary>
        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">事务处理实例</param>
        /// <param name="mapper">automapper实例</param>
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = unitOfWork.UserRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 根据主键值获取一条用户数据
        /// </summary>
        /// <param name="id">主键值</param>
        /// <returns>实体</returns>
        public UserOutDto GetUser(string id)
        {
            var entity = _userRepository.Get(id);
            return _mapper.Map<UserOutDto>(entity);
        }

        /// <summary>
        /// 分页查询方法
        /// </summary>
        /// <param name="dto">分页查询实体</param>
        /// <returns>分页用户数据列表</returns>
        public PageOutDtoBase<UserOutDto> GetUsersByPage(UserPageInDto dto)
        {
            List<User> userlist = new List<User>();
            //获取分页list数据
            if (dto.Name.IsNullOrWhiteSpace())
            {
                userlist = _userRepository.GetPaged(null, dto.CurrentPageIndex, dto.PageSize,
                    dto.SortField, (SortBy)dto.SortBy).ToList();
            }
            else
            {
                userlist = _userRepository.GetPaged(w => w.Name.Contains(dto.Name), dto.CurrentPageIndex, dto.PageSize,
                    dto.SortField, (SortBy)dto.SortBy).ToList();
            }
            var list = _mapper.Map<List<UserOutDto>>(userlist);
            //获取总记录数
            var query = _userRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(dto.Name))
                query = query.Where(w => w.Name.Contains(dto.Name));
            var totalCount = query.Count();
            return new PageOutDtoBase<UserOutDto>() { Total = totalCount, Data = list };
        }

        /// <summary>
        /// 更新一条用户数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        public UserOutDto UpdateUser(string id, UserCreateDto dto)
        {
            //var entity = _mapper.Map<User>(dto);
            //entity.Id = id;
            var entity1 = _userRepository.Get(id);
            entity1.Name = dto.Name;
            entity1.Age = dto.Age;
            _userRepository.Update(entity1);
            return _mapper.Map<UserOutDto>(entity1);
        }

        /// <summary>
        /// 新增一条用户数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns>实体</returns>
        public UserOutDto CreateUser(UserCreateDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            entity.Id = Guid.NewGuid().ToString();
            _userRepository.Insert(entity);
            return _mapper.Map<UserOutDto>(entity);
        }

        /// <summary>
        /// 删除一条示例数据
        /// </summary>
        /// <param name="id">主键值</param>
        public void DeleteUser(string id)
        {
            var entity = _userRepository.Get(id);
            _userRepository.Delete(entity);
        }
        public void TransactionTest()
        {
            using (var scope = _unitOfWork.BeginTransaction())
            {
                _userRepository.Insert(new User() { Id = Guid.NewGuid().ToString(), Age = 1, Name = "aa" });
                _userRepository.Insert(new User() { Id = Guid.NewGuid().ToString(), Age = 1, Name = "aa" });
                _unitOfWork.Save();
                //_userRepository.Insert(new User() { Id = Guid.NewGuid().ToString(), Age = 1, Name = "aafdsgfdsgfsgfsgfdsgfsd" });
                //_unitOfWork.Save();
                _unitOfWork.Complete();
            }
            
        }
    }
}