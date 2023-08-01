using MinimalJwt.Models;

namespace MinimalJwt.Repositories
{
    public class MovieRepository
    {
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
