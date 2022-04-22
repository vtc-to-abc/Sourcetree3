using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BlazorServer.Data.Models;
using BlazorServer.Data;
using NUnit.Framework;

namespace BlazorServerTests.DataAccessTests
{
    [TestFixture]
    public class BookDataTests
    {
        private Mock<ISqlDataAccess>? _mock;
        private ISqlDataAccess? Testdb { get; }
        private IBookData? _service;
        private BookModel? _testData;
        // setup fake database with fake data
        [SetUp]
        public void SetUp()
        {
            //_testdb = new SqlDataAccess();
            _mock = new Mock<ISqlDataAccess>();
            //_service = new BookData(Testdb);
            _service = new BookData(_mock.Object);
            _testData = new BookModel(){ book_id = 6, book_title ="rgergertg", current_rent = 1, stored_copies = 2};

        }

        public async Task<BookModel> ConvertToTaskBookModel(BookModel book)
        {
            return  book;
        }
        [Test]
        public async Task BookData_GetBook_Successfully()
        {

            //using (var mock = AutoMock.GetLoose)
            _mock.Setup(m => m.LoadDataList<BookModel, dynamic>("select * from dbo.book", null)).ReturnsAsync(new List<BookModel> { _testData });
            var actualResult =  await _service.GetBook();
            Assert.IsNotNull(actualResult);
            Assert.AreSame(_testData, actualResult.FirstOrDefault());
        }

        [Test]
        public async Task BookData_InsertBook_Successfully()
        {
            int timesCalled = 0;
            var testSqlQuery = @"insert into dbo.book(book_title, stored_copies, current_rent) 
                                values(@book_title, @stored_copies, @current_rent)";
            var expectedMockResult = new BookModel() { book_id = 1, book_title = "insert test by direct", current_rent = 0, stored_copies = 1 };

            //_mock.Setup(m => m.LoadData<BookModel, dynamic>("select * from dbo.book", null)).ReturnsAsync(new List<BookModel> { });
            //var AfterInsertedDatabase = await _service.GetBook();
           var a = _mock.Setup(m => m.SaveData(testSqlQuery, expectedMockResult)).Returns(ConvertToTaskBookModel(expectedMockResult)).Callback(() => ++timesCalled);

            var result = _service.InsertBook(expectedMockResult);

            Assert.AreEqual(1, timesCalled);
        }

    }
}
