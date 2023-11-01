using MinimalJwt.Models;

namespace MinimalJwt.Repositories
{
    public interface IMovie
    {
        IList<Movie> GetMovies();
        Movie GetMovieById(int id);
        Movie CreateNewMovie(Movie movie);
        Movie EditMovie(int id, Movie oldMovie);
        bool DeleteMovie(int id);

    }
}
