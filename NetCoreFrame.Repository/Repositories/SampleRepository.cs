using System;
using NetCoreFrame.Entities.Models;
using NetCoreFrame.Entities.Repositories;
using NetCoreFrame.Repository.EF.Base;

namespace NetCoreFrame.Repository.EF.Repositories
{
    public class SampleRepository : RepositoryBase<Sample, Guid>, ISampleRepository
    {
        public SampleRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}