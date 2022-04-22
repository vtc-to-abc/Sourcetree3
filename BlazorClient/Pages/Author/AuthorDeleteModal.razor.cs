using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
namespace BlazorClient.Pages.Author
{
    public partial class AuthorDeleteModal
    {
        [Inject] private IAuthorService AuthorService { get; set; }

        [Parameter] public AuthorModel Auth { get; set; }

        private bool OpenModal = false;

        protected override async Task OnInitializedAsync()
        {

            await base.OnInitializedAsync();
        }

        public async Task DeleteCurrentData()
        {
            await AuthorService.DeleteAuthor(Auth);
            StateHasChanged();

        }

        public async Task SetVisible(bool visible)
        {
            OpenModal = visible;
            StateHasChanged();
        }
    }
}
