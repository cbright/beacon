using System;
using System.Web;
using NHibernate;
using NHibernate.Context;

namespace TankTempWeb.Data
{
    public class NHibernateSessionModule : IHttpModule
    {

        private readonly ISessionFactory _sessionFactory;
        public const string NHibernateISessionFactoryKey = "NHibernateISessionFactory";
        private HttpApplication _app;

        public NHibernateSessionModule(ISessionFactory sessionFactorye)
        {
            _sessionFactory = sessionFactorye;
        }

        public void Init(HttpApplication context)
        {
            _app = context;
            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
        }

        void ContextBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var session = !ManagedWebSessionContext.HasBind(app.Context, _sessionFactory) ?
                _sessionFactory.OpenSession() : _sessionFactory.GetCurrentSession();

            ManagedWebSessionContext.Bind(app.Context, session);
        }

        void ContextEndRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            ISession session = null;
            if(ManagedWebSessionContext.HasBind(app.Context, _sessionFactory))
            {
                session = _sessionFactory.GetCurrentSession();
                ManagedWebSessionContext.Unbind(app.Context, _sessionFactory);
                if (session.IsOpen)
                {
                    session.Dispose();
                }
            }


        }

        public void Dispose()
        {
            if (_app == null) return;
            _app = null;
        }
    }
}