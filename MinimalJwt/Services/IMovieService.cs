using MinimalJwt.Models;

namespace MinimalJwt.Services
{
    public interface IMovieService
    {
        public Movie Create(Movie movie);
        public Movie Get(int id);
        public List<Movie> List();
        public Movie Update(int id, Movie movie);
        public bool Delete(int id);
    }
}
