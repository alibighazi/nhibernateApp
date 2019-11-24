using NHibernate;
using NhibernateApp.DbContext;
using NhibernateApp.Models;
using System;

namespace NhibernateApp.Business
{
    public class BaseBusiness<T> : NHibernateMapperSession<T> where T : class
    {
        public BaseBusiness(ISession session) : base(session)
        {
        }

        

    }
}
