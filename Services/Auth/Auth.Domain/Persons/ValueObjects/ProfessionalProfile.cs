using Blocks.Domain.ValueObjects;

namespace Auth.Domain.Persons.ValueObjects;

public class ProfessionalProfile : ValueObject
{
    public string? Position { get; set; }
    public string? CompanyName { get; set; }
    public string? Affiliation { get; set; }

    private ProfessionalProfile() { } // For EF Core

    public static ProfessionalProfile Create(string? position, string? companyName, string? affiliation)
    {
        return new ProfessionalProfile
        {
            Position = string.IsNullOrWhiteSpace(position) ? null : position.Trim(),
            CompanyName = string.IsNullOrWhiteSpace(companyName) ? null : companyName.Trim(),
            Affiliation = string.IsNullOrWhiteSpace(affiliation) ? null : affiliation.Trim()
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Position ?? string.Empty;
        yield return CompanyName ?? string.Empty;
        yield return Affiliation ?? string.Empty;
    }

    public override string ToString() =>
        $"Position: {Position ?? "N/A"}, Company: {CompanyName ?? "N/A"}, Affiliation: {Affiliation ?? "N/A"}";
}
