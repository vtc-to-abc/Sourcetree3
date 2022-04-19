
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Pages
{
    public partial class BookIndex : ComponentBase
    {
        /*[Inject] private IBookData BookDB { get; set; }
        [Parameter] public int TestComponentParemeter { get; set; }
        private List<BookModel> Books = new List<BookModel>();

        protected override async Task OnInitializedAsync()
        {
            Books = await GetData();
            await base.OnInitializedAsync();
        }

        public async Task<List<BookModel>> GetData()
        {

           return await BookDB.GetBook();
        }
        public async Task InsertNewBook()
        {
            BookModel insertest = new BookModel
            {
                book_title = "test insert",
                current_rent = 0,
                stored_copies = 10
            };
            await BookDB.InsertBook(insertest);
        }

        public async Task EditBookInformation(BookModel Book)
        {
            BookModel edittest = new BookModel
            {
                book_id = Book.book_id,
                book_title = "test edit",
                current_rent = Book.current_rent,
                stored_copies = Book.stored_copies
            };
            await BookDB.EditBook(edittest);
        }
        public async Task DeleteBook(BookModel Book)
        {
            await BookDB.Delete(Book);
        }*/
    }
}
