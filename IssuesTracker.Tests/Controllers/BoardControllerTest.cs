using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Issues_Tracker.Controllers;
using System.Collections.Generic;
using System.Linq;
using Issues_Tracker.Models;
using System.Web.Mvc;
using Issues_Tracker;
using Moq;
using System.Linq.Expressions;

namespace IssuesTracker.Tests.Controllers
{
    [TestClass]
    public class BoardControllerTest
    {
        private MyMock _myMock;

        private BoardController _boardController;

        [TestInitialize]
        public void Initialize()
        {
            _myMock = new MyMock();
            _boardController = new BoardController(_myMock.UnitOfWorkMock.Object);
        }

        [TestMethod]
        public void BoardController_Index()
        {
            //Arrange
            _myMock.ProjectRepositoryMock.Setup(x => x.Get(1)).Returns(_myMock.ProjectList.FirstOrDefault(x => x.Id ==1));
            _myMock.IssueRepositoryMock.Setup(x => x.Find(i => i.Project.Name == "Project")).Returns(_myMock.IssueList.Where(i => i.Project.Name == "Project"));
            _myMock.ProjectRepositoryMock.Setup(uow => uow.FirstOrDefault(It.IsAny<Expression<Func<Project, bool>>>())).Returns(new Project() {Name = "Project" });

            //Act
            var result = (_boardController.Index("Project") as ViewResult).Model as Project;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Issues);
        }

        [TestMethod]
        public void BoardController_UpdateState()
        {
            //Arrange
            _myMock.IssueRepositoryMock.Setup(x => x.Get(1)).Returns(new Issue() { Id = 1, StateId = 2 });
            _myMock.ProjectRepositoryMock.Setup(uow => uow.FirstOrDefault(It.IsAny<Expression<Func<Project, bool>>>())).Returns(new Project() { Name = "Project" });
          
            //Act
            var result = (_boardController.UpdateState(1, 1) as RedirectToRouteResult);

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
