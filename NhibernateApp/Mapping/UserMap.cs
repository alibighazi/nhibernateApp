using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NhibernateApp.Models;
using System;

namespace NhibernateApp.Mapping
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {

            Table("Users");

            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Guid);
                x.Type(NHibernateUtil.Guid);
                x.Column("Id");
                x.UnsavedValue(Guid.Empty);
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

            Property(b => b.Email, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
                x.Column("Email");
            });

            Property(b => b.Fullname, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
                x.Column("Fullname");
            });

        }
    }
}
