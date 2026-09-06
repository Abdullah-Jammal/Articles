namespace Auth.Domain.Users;

public class RefreshToken
{
    public int UserId { get; set; }
    public required string Token { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ExpireOn { get; set; }
    public DateTime? RevokeOn { get; set; }
    public string? CreatedByIp { get; set; }
}
