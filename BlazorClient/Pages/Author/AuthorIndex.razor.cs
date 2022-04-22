using Microsoft.AspNetCore.Components;
using BlazorClient.Models;
using BlazorClient.Services;
namespace BlazorClient.Pages.Author
{
    public partial class AuthorIndex: ComponentBase
    {
        [Inject] private IAuthorService AuthorService { get; set; }
        [Inject] private NavigationManager navigationManager { get; set; }
        [Parameter] public int TestComponentParemeter { get; set; }
        protected IEnumerable<AuthorModel> authors { get; set; } = new List<AuthorModel>();

        private AuthorInsertModal _refAuthorInsertModal = new();
        private AuthorEditModal _refAuthorEditModal = new();
        private AuthorDeleteModal _refAuthorDeleteModal = new();

        private AuthorModel CurrentAuthor;

        public async void GetCurrrentAuthor(AuthorModel auth)
        {
             CurrentAuthor = auth;
        }

        protected override async Task OnInitializedAsync()
        {
            authors = await GetData();
            await base.OnInitializedAsync();
        }

        public async Task<IEnumerable<AuthorModel>> GetData()
        {
            return await AuthorService.GetAuthors();
            StateHasChanged();

        }
        public async Task InsertNewAuthor()
        {
            await _refAuthorInsertModal.SetVisible(true);
            StateHasChanged();
        }

        public async Task EditAuthorInformation()
        {
            await _refAuthorEditModal.SetVisible(true);
            StateHasChanged();

        }
        public async Task DeleteAuthor()
        {
            await _refAuthorDeleteModal.SetVisible(true);
            StateHasChanged();

        }
    }
}
