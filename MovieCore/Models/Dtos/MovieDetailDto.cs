namespace MovieCore.Models.Dtos
{
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public decimal? Budget { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
        public ICollection<ActorDto> Actors { get; set; } = new List<ActorDto>();
    }
}
