namespace KnorrNews.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Source { get; set; }

        public DateTime PublishDate { get; set; }

        public string Link { get; set; }
    }
}
