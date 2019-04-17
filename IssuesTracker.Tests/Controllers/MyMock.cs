using Issues_Tracker;
using Issues_Tracker.BL;
using Issues_Tracker.BL.Interfaces;
using Moq;
using System.Collections.Generic;

namespace IssuesTracker.Tests.Controllers
{
    class MyMock
    {
        internal Mock<IUnitOfWork> UnitOfWorkMock;

        internal Mock<IIssueRepository> IssueRepositoryMock;

        internal Mock<IRepository<Project>> ProjectRepositoryMock;

        internal Mock<IRepository<Priority>> PriorityRepositoryMock;

        internal Mock<IRepository<User>> UserRepositoryMock;

        internal Mock<IRepository<State>> StateRepositoryMock;

        internal List<Issue> IssueList;

        internal List<State> StateList;

        internal List<Project> ProjectList;

        internal List<User> UserList;

        internal List<Priority> PriorityList;

        public MyMock()
        {
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            IssueRepositoryMock = new Mock<IIssueRepository>();
            PriorityRepositoryMock = new Mock<IRepository<Priority>>();
            ProjectRepositoryMock = new Mock<IRepository<Project>>();
            UserRepositoryMock = new Mock<IRepository<User>>();
            StateRepositoryMock = new Mock<IRepository<State>>();
            IssueList = GetIssueList();
            StateList = GetStateList();
            UserList = GetUserList();
            PriorityList = GetPriorityList();
            ProjectList = GetProjectList();
            UnitOfWorkMock.Setup(x => x.Issues).Returns(IssueRepositoryMock.Object);
            UnitOfWorkMock.Setup(x => x.Projects).Returns(ProjectRepositoryMock.Object);
            UnitOfWorkMock.Setup(x => x.Priorityes).Returns(PriorityRepositoryMock.Object);
            UnitOfWorkMock.Setup(x => x.Users).Returns(UserRepositoryMock.Object);
            UnitOfWorkMock.Setup(x => x.States).Returns(StateRepositoryMock.Object);
        }

        private List<Issue> GetIssueList()
        {
           List<Issue> result =  new List<Issue>() {
           new Issue() { Id = 1, Number = 1, Descripton = "Descripton", Summary="Summary", Project = new Project() { Name = "Project" }, Priority = new Priority() {Name = "High" } },
           new Issue() { Id = 2, Number = 2, Descripton = "Descripton", Summary="Summary" , Project = new Project() { Name = "Project" }, Priority = new Priority() {Name = "High" } },
           new Issue() { Id = 3, Number = 3, Descripton = "Descripton", Summary="Summary" , Project = new Project() { Name = "Project" }, Priority = new Priority() {Name = "High" } }
          };
          return result;
        }

        private List<Project> GetProjectList()
        {
            return new List<Project>(){
                new Project() {Id = 1, Name ="Project" }
            };
        }

        private List<State> GetStateList()
        {
            return new List<State>(){
                new State() {Id = 1, Name ="State" }
            };
        }

        private List<Priority> GetPriorityList()
        {
            return new List<Priority>(){
                new Priority() {Id = 1, Name ="Priority" }
            };
        }

        private List<User> GetUserList()
        {
            return new List<User>(){
                new User() {Id = 1, Name ="User" }
            };
        }
    }
}
