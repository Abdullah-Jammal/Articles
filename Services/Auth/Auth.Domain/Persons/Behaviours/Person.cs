using Articles.Abstractions;
using Auth.Domain.Persons.ValueObjects;

namespace Auth.Domain.Persons;

public partial class Person
{
    public static Person Create(IPersonCreationInfo personInfo)
    {
        var person = new Person
        {
            Email = personInfo.Email,
            FirstName = personInfo.FirstName,
            LastName = personInfo.LastName,
            Gender = personInfo.Gender,
            ProfessionalProfile = ProfessionalProfile.Create(personInfo.Position, personInfo.CompanyName, personInfo.Affiliation),
            PictureUrl = personInfo.PictureUrl,
            Honorific = HonorificTile.Create(personInfo.Honorific),
        };

        return person;
    }
}
