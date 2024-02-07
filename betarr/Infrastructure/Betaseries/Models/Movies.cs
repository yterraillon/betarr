namespace Infrastructure.Betaseries.Models;

public record Movies(IEnumerable<BsMovie> movies, int total, int totalMissingMovies);

public record BsMovie(string id, string title, string original_title, string tmdb_id, string imdb_id, User user);

public record User(bool in_account, string status);