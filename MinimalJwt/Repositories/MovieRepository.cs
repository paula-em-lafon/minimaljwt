using MinimalJwt.Models;
using System.Configuration;
using System.Data.SqlClient;
using System;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using System.Data;

namespace MinimalJwt.Repositories
{
    public class MovieRepository : IMovie
    {
        private readonly string CS = ConfigurationManager.ConnectionStrings["MoviesContextDb"].ConnectionString;


        public IList<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetAllMovies", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var movie = new Movie()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Title = rdr["Title"].ToString(),
                        Description = rdr["Description"].ToString(),
                        Rating = Convert.ToDouble(rdr["Rating"])
                    };
                    movies.Add(movie);
                }
                return (movies);
            }
        }

        public Movie CreateNewMovie(Movie movie)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spAddNewMovie", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", movie.Title);
                cmd.Parameters.AddWithValue("@Description", movie.Description);
                cmd.Parameters.AddWithValue("@Rating", movie.Rating);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    movie.Id = Convert.ToInt32(rdr["Id"]);
                    movie.Title = rdr["Title"].ToString();
                    movie.Description = rdr["Description"].ToString();
                    movie.Rating = Convert.ToDouble(rdr["Rating"]);
                }
                return movie;
            }
        }

        public Movie GetMovieById(int id) 
        {
            Movie movie = new Movie();
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spGetSingleMovie", con);
                con.Open();
                cmd.CommandType= CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    movie.Id = Convert.ToInt32(rdr["Id"]);
                    movie.Title = rdr["Title"].ToString();
                    movie.Description = rdr["Description"].ToString();
                    movie.Rating = Convert.ToDouble(rdr["Rating"]);
                }
                return movie;
            }
        }
        public Movie EditMovie(int id, Movie oldMovie)
        {
            Movie newMovie = new Movie();
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spUpdateMovie", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Title", oldMovie.Title);
                cmd.Parameters.AddWithValue("@Description", oldMovie.Description);
                cmd.Parameters.AddWithValue("@Rating", oldMovie.Rating);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    newMovie.Id = Convert.ToInt32(rdr["Id"]);
                    newMovie.Title = rdr["Title"].ToString();
                    newMovie.Description = rdr["Description"].ToString();
                    newMovie.Rating = Convert.ToDouble(rdr["Rating"]);
                }
                return newMovie;
            }
        }

        public bool DeleteMovie(int id)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spDeleteMovie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static List<Movie> Movies = new()
        {
            new() { Id = 1, Title = "Eternals", Description="This is a movie description we'ree entering movie description territory this is a movie description",Rating = 6.8},
            new() { Id = 2, Title = "Titanic", Description="A love story that takes place in the famously wrecked ship",Rating = 7.9},
            new() { Id = 3, Title = "Como Agua Para Chocolate", Description="I don't know what this movie is about I'm just making stuff up", Rating = 8.5},
            new() { Id = 4, Title = "Begin again", Description="This one isn't even a movie it's a song and I'm giving it the highest rating", Rating = 9.3},
            new() { Id = 5, Title = "Some abismal movie", Description="This movie is really really bad it gets a bad score from me", Rating = 3.2},
        };
    }
}
