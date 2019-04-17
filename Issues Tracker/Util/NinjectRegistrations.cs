using Issues_Tracker.BL;
using Issues_Tracker.BL.Classes;
using Issues_Tracker.BL.Interfaces;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Issues_Tracker.Util
{
    public class NinjectRegistrations: NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IIssueRepository>().To<IssueRepository>();
            Bind<IRepository<State>>().To<Repository<State>>();
            Bind<IRepository<User>>().To<Repository<User>>();
            Bind<IRepository<Project>>().To<Repository<Project>>();
            Bind<IRepository<Priority>>().To<Repository<Priority>>();
        }
    }
}