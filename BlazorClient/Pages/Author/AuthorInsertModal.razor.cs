using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
namespace BlazorClient.Pages.Author
{
    public partial class AuthorInsertModal
    {
        [Inject] private IAuthorService AuthorService { get; set; }

        private AuthorModel newAuthor = new();

        private bool OpenModal = false;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        public async Task InsertNewData()
        {
            await AuthorService.InsertAuthor(newAuthor);
            StateHasChanged();

        }

        public async Task SetVisible(bool visible)
        {
             OpenModal =  visible;
            StateHasChanged();
        }

    }
}
