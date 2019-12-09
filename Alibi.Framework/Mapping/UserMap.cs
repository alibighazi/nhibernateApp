using Alibi.Framework.Models;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Alibi.Framework.mapping
{
    public class UserMap : ClassMapping<UserIdentityModel>
    {
        public UserMap()
        {

            Table("Users");

            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Increment);
                x.Column("id");
            });


            Property(b => b.Username, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
                x.Column("Username");
            });

            Property(b => b.Password, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
                x.Column("Password");
            });

      

            Property(b => b.Token, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(false);
                x.Column("Token");
            });

        }
    }
}
