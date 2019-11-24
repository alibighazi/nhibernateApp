using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NhibernateApp.Models;

namespace NhibernateApp.Mapping
{
    public class BookMap : ClassMapping<Book>//AuditableBaseMap<Book>
    {
        public BookMap()
        {
            Table("Books");
            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Increment);
                //x.Type(NHibernateUtil.i);
                x.Column("Id");
                //x.UnsavedValue(Guid.Empty);
            });

            Property(b => b.Title, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
                x.Column("Title");
            });



            //Property(b => b.CreatedBy, x =>
            //{
            //    x.Column("CreatedBy");
            //});
            //Property(b => b.CreatedOn, x =>
            //{
            //    x.Column("CreatedOn");
            //});
            //Property(b => b.UpdatedBy, x =>
            //{
            //    x.Column("UpdatedBy");
            //});
            //Property(b => b.UpdatedOn, x =>
            //{
            //    x.Column("UpdatedOn");
            //});



        }
    }
}
