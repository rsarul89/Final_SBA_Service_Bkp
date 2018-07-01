using Autofac;
using System.Linq;
using System.Reflection;

namespace SkillTracker.WebApi
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("SkillTracker.Services"))
                  .Where(t => t.Name.EndsWith("Service"))
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();
        }
    }
}