using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Issues_Tracker;
using Issues_Tracker.Controllers;
using Issues_Tracker.BL;
using Moq;
using Issues_Tracker.Models;
using System.Xml.Linq;

namespace IssuesTracker.Tests.Controllers
{
    [TestClass]
    public class IssueControllerTest
    {
        private MyMock _myMock;

        private IssueController _issueController;

        [TestInitialize]
        public void Initialize()
        {
            _myMock = new MyMock();
            _issueController = new IssueController(_myMock.UnitOfWorkMock.Object);
        }

        [TestMethod]
        public void IssueController_Index()
        {
            //Arrange
            _myMock.IssueRepositoryMock.Setup(x => x.GetAll("Project", "High")).Returns(_myMock.IssueList);
            

            //Act
            IssuePrioritiesList result = (_issueController.Index("Project", "High", 1) as ViewResult).Model as IssuePrioritiesList;
            List<int> expected = result.Issues.Select(x => x.Number).ToList();
            List<int> real = _myMock.IssueList.Select(x => x.Number).ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(real.ToString(), expected.ToString());
        }

        [TestMethod]
        public void IssueController_AddEditIssue_Update()
        {
            //Arrange
            _myMock.IssueRepositoryMock.Setup(x => x.AddOrUpdateIssue(new Issue())).Verifiable();

            //Act
            var result = _issueController.Index(new Issue() {Id = 4 }) as RedirectToRouteResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IssueController_AddEditIssue_ReturnView()
        {
            //Arrange
            _myMock.IssueRepositoryMock.Setup(x => x.Get(1)).Returns(_myMock.IssueList.FirstOrDefault(x => x.Id == 1));
            _myMock.PriorityRepositoryMock.Setup(x => x.GetAll()).Returns(_myMock.PriorityList);
            _myMock.StateRepositoryMock.Setup(x => x.GetAll()).Returns(_myMock.StateList);
            _myMock.UserRepositoryMock.Setup(x => x.GetAll()).Returns(_myMock.UserList);
            _myMock.ProjectRepositoryMock.Setup(x => x.GetAll()).Returns(_myMock.ProjectList);

            //Act
            var result = (_issueController.AddEditIssue(1) as PartialViewResult).Model as IssueView;
            
            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Issue);
            Assert.AreEqual(result.Issue, _myMock.IssueList.FirstOrDefault(x => x.Id == 1));
        }
    }
}
