namespace KnorrNews.Models
{
    public class NewsViewModel
    {
        public required string Title { get; set; }

        public required string Summary { get; set; }

        public required string Source { get; set; }

        public DateTime PublishDate { get; set; }


        public required string Link { get; set; }
    }
}
