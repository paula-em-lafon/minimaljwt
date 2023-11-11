using MinimalJwt.Models;
using MinimalJwt.Repositories;
using System.Reflection;

namespace MinimalJwt.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovie _movieRepository;

        public MovieService(IMovie movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public Movie Create(Movie movie)
        {
            movie = _movieRepository.CreateNewMovie(movie);

            return movie;
        }

        public Movie Get(int id)
        {
            
            var movie = _movieRepository.GetMovieById(id);
            
            if (movie is null) return null;
            
            return movie;
        }

        public List<Movie> List() 
        {
            var movies = _movieRepository.GetMovies();
            List<Movie> moviesList = new List<Movie>(movies);

            return moviesList;
        }

        public Movie Update(int id, Movie oldMovie) 
        {
            var newMovie = _movieRepository.EditMovie(id, oldMovie);

            return newMovie;
        }

        public bool Delete(int id) 
        {
            var newMovie = _movieRepository.DeleteMovie(id);

            return true;
        }
    }
}
