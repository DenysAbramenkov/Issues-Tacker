using Issues_Tracker.BL.Classes;
using Issues_Tracker.BL.Interfaces;
using System;

namespace Issues_Tracker.BL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IssueTrackerEntities context = new IssueTrackerEntities();

        private IIssueRepository issues;

        private IRepository<Priority> priorityes;

        private IRepository<User> users;

        private IRepository<State> states;

        private IRepository<Project> projects;


        public IIssueRepository Issues
        {
            get
            {
                if (issues == null)
                {
                    issues = new IssueRepository(context);
                }
                return issues;
            }
            set
            {
                issues = value;
            }
        }

        public IRepository<Priority> Priorityes
        {
            get
            {
                if (priorityes == null)
                {
                    priorityes = new Repository<Priority>(context);
                }
                return priorityes;
            }
        }

        public IRepository<Project> Projects
        {
            get
            {
                if (projects == null)
                {
                    projects = new Repository<Project>(context);
                }
                return projects;
            }
        }

        public IRepository<State> States
        {
            get
            {
                if (states == null)
                {
                    states = new Repository<State>(context);
                }
                return states;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (users == null)
                {
                    users = new Repository<User>(context);
                }
                return users;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}