namespace Infrastructure.Betaseries.Models;

public record OauthParameters(string client_id, string client_secret, string redirect_uri, string code);
public record AccessToken(string access_token);