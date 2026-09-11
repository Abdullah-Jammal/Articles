namespace Article.Security;

public record JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SecretKey { get; init; }
    public int ValidForMinutes { get; set; }
    public DateTime IssueAt { get; set; } = DateTime.UtcNow;
    public TimeSpan ValidFor => TimeSpan.FromMinutes(ValidForMinutes);
    public DateTime Expiration => IssueAt.Add(ValidFor);
}
