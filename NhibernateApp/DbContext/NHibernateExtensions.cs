using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Type;
using System;

namespace NhibernateApp.DbContext
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateExtensions).Assembly.ExportedTypes);
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<NHibernate.Dialect.MySQL5Dialect>();
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
            //var exporter = new SchemaExport(configuration);
            //exporter.Execute(true, true, false);

            var sessionFactory = configuration.BuildSessionFactory().WithOptions();

            sessionFactory.Interceptor(new AuditInterceptor());

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());

            return services;
        }
    }









    public interface IAuditable { }


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
