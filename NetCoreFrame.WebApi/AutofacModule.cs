using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace NetCoreFrame.WebApi
{
    /// <summary>
    /// autofac注册管理
    /// </summary>
    public class AutofacModule : Module
    {
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="builder">autofac builder</param>
        protected override void Load(ContainerBuilder builder)
        {
            //dapper仓储接口注入
            //builder.RegisterAssemblyTypes(Assembly.Load("NetCoreFrame.Repository.Dapper"))
             //.Where(t => t.Name.EndsWith("Repository"))
             //   .AsImplementedInterfaces()
             //   .InstancePerLifetimeScope();
            //EF仓储接口注入
            builder.RegisterAssemblyTypes(Assembly.Load("NetCoreFrame.Repository.EF"))
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            //服务层接口注入
            builder.RegisterAssemblyTypes(Assembly.Load("NetCoreFrame.Application"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}