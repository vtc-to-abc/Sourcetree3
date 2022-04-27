using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BlazorServer.Data.Models;
using BlazorServer.Data.Services;
using NUnit.Framework;
using BlazorServer.Data.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace BlazorServerTests.ControllersTest
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookData> _mockData;
        private Mock<ILogger<BookController>> _mockLogger;
        private BookController _controller;
        private List<BookModel> _testDatas;

        [SetUp]
        public void Setup()
        {
            _mockData = new Mock<IBookData>();
            _mockLogger = new Mock<ILogger<BookController>>();
            _controller = new BookController(_mockData.Object, _mockLogger.Object);

            _testDatas = new()
            {
                new() { book_id = 1, book_title = "rgergertg", current_rent = 1, stored_copies = 2 },
                new() { book_id = 2, book_title = "rgergertg", current_rent = 1, stored_copies = 2 },
                new() { book_id = 3, book_title = "rgergertg", current_rent = 1, stored_copies = 2 },
            };
        }

        [Test]
        public async Task BookController_GetBook_Successfully()
        {
            _mockData.Setup(md => md.GetBook()).ReturnsAsync(_testDatas);

            var response = await _controller.Index();
            var responseResult = response as ObjectResult;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(responseResult.StatusCode, 200);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var responseResultValue = responseResult.Value as IEnumerable<BookModel>;

#pragma warning disable CS8604 // Possible null reference argument.
            Assert.AreEqual(responseResultValue.First().book_id, _testDatas.First().book_id);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        [Test]
        public async Task BookController_InsertBook_Successfully()
        {
            _mockData.Setup(md => md.InsertBook(_testDatas.First())).ReturnsAsync(_testDatas.First());

            var response = await _controller.InsertBook(_testDatas.First());
            var responseResult = response as ObjectResult;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(responseResult.StatusCode, 200);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            var responseResultValue = responseResult.Value as BookModel;

#pragma warning disable CS8604 // Possible null reference argument.
            Assert.AreEqual(responseResultValue.book_id, _testDatas.First().book_id);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
