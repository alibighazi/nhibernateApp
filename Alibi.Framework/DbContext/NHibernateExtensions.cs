using Alibi.Framework.Interfaces;
using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using System;

namespace Alibi.Framework.DbContext
{
    public static class NHibernateExtensions
    {
        public static ContainerBuilder AddNHibernate(this ContainerBuilder builder, string connectionString)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);

           
            
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

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
            //exporter.Execute(true, true, false);



            var sessionFactory = configuration.BuildSessionFactory().WithOptions();

            //sessionFactory.Interceptor(new AuditInterceptor());

            builder.Register(c => sessionFactory)
                .As<ISessionBuilder>()
                .SingleInstance();

            builder.Register(c => c.Resolve<ISessionBuilder>().OpenSession()).As<ISession>().InstancePerLifetimeScope();


            builder.RegisterInstance<ISessionBuilder>(sessionFactory);
            return builder;
        }
    }











    public class AuditInterceptor : EmptyInterceptor
    {

        private int updates;
        private int creates;
        private int loads;

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
            if (entity is IAuditable)
            {
                updates++;
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if ("lastUpdateTimestamp".Equals(propertyNames[i]))
                    {
                        currentState[i] = new DateTime();
                        return true;
                    }
                }
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
                loads++;
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
                creates++;
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
                    "Creations: " + creates +
                    ", Updates: " + updates +
                    ", Loads: " + loads);
            }
            updates = 0;
            creates = 0;
            loads = 0;
        }

    }
}
