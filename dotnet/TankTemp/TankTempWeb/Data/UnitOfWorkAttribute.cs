using System;
using System.Web.Mvc;
using NHibernate;

namespace TankTempWeb.Data
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UnitOfWorkAttribute : ActionFilterAttribute 
    {
        public Func<ISessionFactory> SessionFactoryFinder { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = SessionFactoryFinder().GetCurrentSession();
            session.BeginTransaction();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var session = SessionFactoryFinder().GetCurrentSession();

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