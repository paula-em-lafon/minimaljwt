using MinimalJwt.Models;
using MinimalJwt.Repositories;
using System.Reflection;

namespace MinimalJwt.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieRepository movieRepository = new MovieRepository();
        public Movie Create(Movie movie)
        {
            movie = movieRepository.CreateNewMovie(movie);

            return movie;
        }

        public Movie Get(int id)
        {
            
            var movie = movieRepository.GetMovieById(id);
            
            if (movie is null) return null;
            
            return movie;
        }

        public List<Movie> List() 
        {
            var movies = movieRepository.GetMovies();
            List<Movie> moviesList = new List<Movie>(movies);

            return moviesList;
        }

        public Movie Update(int id, Movie oldMovie) 
        {
            var newMovie = movieRepository.EditMovie(id, oldMovie);

            return newMovie;
        }

        public bool Delete(int id) 
        {
            var newMovie = movieRepository.DeleteMovie(id);

            return true;
        }
    }
}
