using Autofac;
using System.Linq;
using System.Reflection;

namespace SkillTracker.WebApi
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("SkillTracker.Repositories"))
                  .Where(t => t.Name.EndsWith("Repository"))
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();
        }
    }
}