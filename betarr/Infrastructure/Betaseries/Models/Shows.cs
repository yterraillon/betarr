namespace Infrastructure.Betaseries.Models;

public record Shows(IEnumerable<Show> shows);

public record Show(string id,string title, string original_title, string thetvdb_id, string imdb_id, string themoviedb_id);