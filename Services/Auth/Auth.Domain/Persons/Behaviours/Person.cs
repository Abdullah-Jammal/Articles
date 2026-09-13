using Articles.Abstractions;
using Auth.Domain.Persons.ValueObjects;

namespace Auth.Domain.Persons;

public partial class Person
{
    public static Person Create(IPersonCreationInfo userInfo)
    {
        var person = new Person
        {
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Gender = userInfo.Gender,
            ProfessionalProfile = ProfessionalProfile.Create(userInfo.Position, userInfo.CompanyName, userInfo.Affiliation),
            PictureUrl = userInfo.PictureUrl,
            Honorific = HonorificTile.Create(userInfo.Honorific),
        };

        return person;
    }
}
