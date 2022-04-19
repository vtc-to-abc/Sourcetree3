using Microsoft.AspNetCore.Components;
using BlazorClient;
using BlazorClient.Services;
namespace BlazorServer.Pages
{
    public partial class AuthorIndex: ComponentBase
    {
        [Inject] private IAuthorService AuthorService { get; set; }
        [Parameter] public int TestComponentParemeter { get; set; }
        protected IEnumerable<Author> authors { get; set; } = new List<Author>();

        protected override async Task OnInitializedAsync()
        {
            authors = await GetData();
            await base.OnInitializedAsync();
        }

        public async Task<IEnumerable<Author>> GetData()
        {
            return await AuthorService.GetAuthors();
        }
        /*public async Task InsertNewAuthor()
        {
            Author insertest = new Author
            {
                pseudonym = "test insert"
            };
            await AuthorDB.InsertAuthor(insertest);
        }

        public async Task EditAuthorInformation()
        {
            Author edittest = new Author
            {
                author_id = 1,
                pseudonym = "test edit"
            };
            await AuthorDB.EditAuthor(edittest);
        }
        public async Task DeleteAuthor(Author author )
        {
            await AuthorDB.Delete(author);
        }*/
    }
}
