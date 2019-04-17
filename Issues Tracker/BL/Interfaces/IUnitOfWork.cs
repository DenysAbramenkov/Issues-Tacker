using Issues_Tracker.BL.Interfaces;

namespace Issues_Tracker.BL
{
    public interface IUnitOfWork
    {
        IIssueRepository Issues { get; set; }

        IRepository<User> Users { get; }

        IRepository<State> States { get; }

        IRepository<Project> Projects { get; }

        IRepository<Priority> Priorityes { get; }

        void Save();

        void Dispose();
    }
}
