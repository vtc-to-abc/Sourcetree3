using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
using System.Web.Mvc;

namespace BlazorClient.Pages.Book
{
    public partial class BookEditModal
    {
        [Inject] private IBookService BookService { get; set; }
        [Inject] private IAuthorService AuthorService { get; set; }
        [Inject] private IAuthorBookService AuthorBookService { get; set; }
        [Parameter] public BookModel Book { get; set; }
        [Parameter]public IEnumerable<AuthorModel> WritenByAuthors { get; set; }
        private bool OpenModal = false;
        public IEnumerable<BookModel> Books { get; set; } 
        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }


        public void EditCurrentData()
        {
            
             BookService.EditBook(Book);
            StateHasChanged();

        }


        public void SetVisible(bool visible)
        {
            OpenModal = visible;
            StateHasChanged();
        }

    }
}
