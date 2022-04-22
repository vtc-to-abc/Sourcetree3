using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
using Microsoft.JSInterop;

namespace BlazorClient.Pages.Book
{
    public partial class BookInsertModal
    {
        [Inject] private IBookService BookService { get; set; }
        [Inject] private IAuthorService AuthorService { get; set; }
        [Inject] private IAuthorBookService AuthorBookService { get; set; }
        [Inject] IJSRuntime JS { get; set; }

        private BookModel newBook = new();
        private IEnumerable<AuthorModel> ExistedAuthors = new List<AuthorModel>();
        private List<AuthorModel> SelectedAuthors = new List<AuthorModel>();

        private IEnumerable<BookModel> ExistedBooks = new List<BookModel>();
        private bool OpenModal = false;

        protected override async Task OnInitializedAsync()
        {
            ExistedAuthors = await AuthorService.GetAuthors();
            ExistedBooks = await BookService.GetBooks();
            await base.OnInitializedAsync();
        }

        public void ExistedAuthorCheckBoxClicked(AuthorModel author, object checkedValue)
        {
            if ((bool)checkedValue)
                if (!SelectedAuthors.Contains(author))
                    SelectedAuthors.Add(author);
            else
                if (SelectedAuthors.Contains(author))
                    SelectedAuthors.Remove(author);
        }
        public async Task InsertNewBook ()
        { 
            await BookService.InsertBook(newBook);

            StateHasChanged();

        }


        //protected override async Task OnAfterRenderAsync(bool first)
        //{
        //    if (first)
        //    {
        //        var interopResult = await JS.InvokeAsync<IJSObjectReference>("import","./scripts/testinterop.js");
        //        await JS.InvokeVoidAsync("testjquerychosen.testchosen");
        //    }
        //    await base.OnAfterRenderAsync(first);
        //}

        public async Task CreateRelational()
        {

            await InsertNewBook();
            var LastestBook = ExistedBooks.LastOrDefault();
            if (SelectedAuthors.Count > 0)
            {
                foreach (AuthorModel author in SelectedAuthors)
                {
                    AuthorBookModel authbook = new AuthorBookModel()
                    {
                        book_id = LastestBook.book_id,
                        author_id = author.author_id
                    };

                    await AuthorBookService.InsertAuthorBook(authbook);
                }
            }
            StateHasChanged();


        }


        public async Task SetVisible(bool visible)
        {
             OpenModal =  visible;
            StateHasChanged();
        }

    }
}
