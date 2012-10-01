using System;
using System.Web;
using NHibernate;
using NHibernate.Context;

namespace TankTempWeb.Data
{
    public class NHibernateSessionModule : IHttpModule
    {
 
        private Func<ISessionFactory> _sessionFactoryProvider;
        public const string NHibernateISessionFactoryKey = "NHibernateISessionFactory";
        private HttpApplication _app;

        public void Init(HttpApplication context)
        {
            _sessionFactoryProvider = () => {return (ISessionFactory)context.Application[NHibernateISessionFactoryKey];};
            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
        }

        void ContextBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var session = !ManagedWebSessionContext.HasBind(app.Context, _sessionFactoryProvider()) ?
                _sessionFactoryProvider().OpenSession() : _sessionFactoryProvider().GetCurrentSession();

            ManagedWebSessionContext.Bind(app.Context, session);
        }

        void ContextEndRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            ISession session = null;
            if(ManagedWebSessionContext.HasBind(app.Context, _sessionFactoryProvider()))
            {
                session = _sessionFactoryProvider().GetCurrentSession();
                ManagedWebSessionContext.Unbind(app.Context, _sessionFactoryProvider());
                if (session.IsOpen)
                {
                    session.Dispose();
                }
            }


        }

        public void Dispose()
        {
            _app.BeginRequest -= ContextBeginRequest;
            _app.EndRequest -= ContextEndRequest;
            _app = null;
        }
    }
}