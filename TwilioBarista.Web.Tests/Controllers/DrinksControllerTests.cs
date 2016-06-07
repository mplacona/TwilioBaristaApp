using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using TwilioBarista.Web.Controllers;
using TwilioBarista.Web.DAL;
using TwilioBarista.Web.Models;
using TwilioBarista.Web.Repository;

namespace TwilioBarista.Web.Tests.Controllers
{
    public class DrinksControllerTests
    {
        private Mock<TwilioBaristaContext> _twilioBaristaContextMock;
        private Mock<DrinkRepository> _repositoryMock;
        private DrinksController _controller;

        [SetUp]
        public void Setup()
        {   
            _twilioBaristaContextMock = new Mock<TwilioBaristaContext>();
            _repositoryMock = new Mock<DrinkRepository>(_twilioBaristaContextMock.Object);
            _controller = new DrinksController(_repositoryMock.Object);
        }


        [Test]
        public void Test_When_List_Drinks_Then_Load_From_Database_And_Return_View()
        {
            // Arrange
            var drinks = new List<Drink>
            {
                new Drink {Name = "Expresso"},
                new Drink {Name = "Late"},
                new Drink {Name = "Cappucino"}
            };

            _repositoryMock.Setup(x => x.SelectAll()).Returns(drinks);

            // Act
            //var result = _controller.Index() as ViewResult;
            var returnedDrinks = _controller.Index() as ViewResult;
            var model = returnedDrinks.ViewData.Model as List<Drink>;

            //Assert
            Assert.IsNotNull(returnedDrinks);
            Assert.AreEqual(3, model.Count);
        }

        [Test]
        public void Test_When_Save_Then_Persists_In_Database()
        {
            // Arrange
            var drink = new Drink {Name = "Expresso"};

            // Act
            var result = _controller.Create(drink);

            // Assert
            _repositoryMock.Verify(r => r.Insert(It.IsAny<Drink>()), Times.Once);

        }
    }
}