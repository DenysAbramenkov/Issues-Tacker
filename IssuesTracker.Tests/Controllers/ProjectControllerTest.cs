using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Issues_Tracker.Controllers;
using Issues_Tracker;
using System.Web.Mvc;

namespace IssuesTracker.Tests.Controllers
{
    [TestClass]
    public class ProjectControllerTest
    {
        private MyMock _myMock;

        private ProjectController _projectController;

        [TestInitialize]
        public void Initialize()
        {
            _myMock = new MyMock();
            _projectController = new ProjectController(_myMock.UnitOfWorkMock.Object);
        }

        [TestMethod]
        public void ProjectController_CreateProject()
        {
            //Arrange
            _myMock.ProjectRepositoryMock.Setup(x => x.Create(new Project())).Verifiable();

            //Act
            var result = _projectController.CreateProject("Project") as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
