using System;
using NetCoreFrame.Entities.Models;

namespace NetCoreFrame.Entities.Repositories
{
    /// <summary>
    /// 示例仓储接口
    /// </summary>
    public interface ISampleRepository : IRepository<Sample, Guid>
    {
    }
}