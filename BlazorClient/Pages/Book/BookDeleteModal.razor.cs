using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
namespace BlazorClient.Pages.Book
{
    public partial class BookDeleteModal
    {
        [Inject] private IBookService BookService { get; set; }

        [Parameter] public BookModel Book { get; set; }

        private bool OpenModal = false;

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        public async Task DeleteCurrentData()
        {
            await BookService.DeleteBook(Book);
            StateHasChanged();

        }

        public async Task SetVisible(bool visible)
        {
            OpenModal = visible;
            StateHasChanged();
        }
    }
}
