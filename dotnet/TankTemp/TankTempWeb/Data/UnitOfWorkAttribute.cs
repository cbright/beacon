using System;
using System.Web.Mvc;
using NHibernate;

namespace TankTempWeb.Data
{
    public class UnitOfWork : IActionFilter
    {
        private readonly ISessionFactory _sessionFactory;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = _sessionFactory.GetCurrentSession();
            session.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var session = _sessionFactory.GetCurrentSession();
            
            var txn = session.Transaction;

            if (txn == null || !txn.IsActive) return;

            if (filterContext.Exception == null || filterContext.ExceptionHandled){
                session.Transaction.Commit();
            }else{
                session.Transaction.Rollback();
                session.Clear();
            }
        }
    }
}