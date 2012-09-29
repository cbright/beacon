using System;
using System.Web.Mvc;
using NHibernate;

namespace TankTempWeb.Data
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UnitOfWorkAttribute : ActionFilterAttribute 
    {
        public Func<ISession> SessionFinder { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = SessionFinder();
            session.BeginTransaction();
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var session = SessionFinder();

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