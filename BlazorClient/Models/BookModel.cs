namespace BlazorClient.Models
{
    public class BookModel
    {
        public int book_id { get; set; }
        public string book_title { get; set; }
        public int stored_copies { get; set; }
        public int current_rent { get; set; }
    }
}
