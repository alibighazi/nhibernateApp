using Alibi.Framework.Interfaces;
using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using System;
using System.Linq;

namespace Alibi.Framework.DbContext
{
    public static class NHibernateExtensions
    {
        public static void AddNHibernate(this ContainerBuilder builder, string connectionString)
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(typeof(NhibernateExtension).Assembly.ExportedTypes);

            // TODO: change
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.ToLower().Contains("modules")).ToList())
            {
                mapper.AddMappings(item.ExportedTypes);
            }

            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
                c.ConnectionString = connectionString;
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Validate;
#if DEBUG
                c.LogSqlInConsole = true;
                c.LogFormattedSql = true;
                c.AutoCommentSql = true;
#endif
            });

            configuration.AddMapping(domainMapping);
            var exporter = new SchemaExport(configuration);
            exporter.Execute(true, true, false);


            var sessionFactory = configuration.BuildSessionFactory().WithOptions();

            //sessionFactory.Interceptor(new AuditInterceptor());

            builder.Register(c => sessionFactory)
                .As<ISessionBuilder>()
                .Keyed<ISessionBuilder>("default")
                .SingleInstance();

            builder.Register(c => c.ResolveKeyed<ISessionBuilder>("default").OpenSession()).As<ISession>().InstancePerLifetimeScope();


            builder.RegisterInstance(sessionFactory);
        }
    }


    public class AuditInterceptor : EmptyInterceptor
    {
        private int _updates;
        private int _creates;
        private int _loads;

        public override void OnDelete(object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            // do nothing
        }

        public override bool OnFlushDirty(object entity,
            object id,
            object[] currentState,
            object[] previousState,
            string[] propertyNames,
            IType[] types)
        {
            if (!(entity is IAuditable)) return false;
            _updates++;
            for (var i = 0; i < propertyNames.Length; i++)
            {
                if (!"lastUpdateTimestamp".Equals(propertyNames[i])) continue;
                currentState[i] = new DateTime();
                return true;
            }

            return false;
        }

        public override bool OnLoad(object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            if (entity is IAuditable)
            {
                _loads++;
            }

            return false;
        }

        public override bool OnSave(object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            if (entity is IAuditable)
            {
                _creates++;
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if ("createTimestamp".Equals(propertyNames[i]))
                    {
                        state[i] = new DateTime();
                        return true;
                    }
                }
            }

            return false;
        }

        public override void AfterTransactionCompletion(ITransaction tx)
        {
            if (tx.WasCommitted)
            {
                System.Console.WriteLine(
                    "Creations: " + _creates +
                    ", Updates: " + _updates +
                    ", Loads: " + _loads);
            }

            _updates = 0;
            _creates = 0;
            _loads = 0;
        }
    }
}