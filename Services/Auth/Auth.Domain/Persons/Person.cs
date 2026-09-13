using Articles.Abstractions.Enums;
using Auth.Domain.Persons.ValueObjects;

namespace Auth.Domain.Persons;

public partial class Person
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public required Gender Gender { get; set; }
    public HonorificTile? Honorific { get; set; }
    public ProfessionalProfile? ProfessionalProfile { get; set; }
    public string? PictureUrl { get; set; }
}
