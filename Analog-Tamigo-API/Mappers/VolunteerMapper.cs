using System.Collections.Generic;
using System.IO;
using System.Linq;
using Analog_Tamigo_API.Models;
using TamigoServices.Models.Responses;

namespace Analog_Tamigo_API.Mappers
{
    internal class VolunteerMapper : IMapper<Contact, VolunteerDto>
    {
        public IEnumerable<VolunteerDto> Map(IEnumerable<Contact> t)
        {
            return t.Select(contact => new VolunteerDto
            {
                Name = contact.FirstName,

                // TODO: Define default
                Photo = string.IsNullOrWhiteSpace(contact.ImageUrl) ? null : $"https://app.tamigo.com/Public/Photos/{Path.GetFileName(contact.ImageUrl)}",

                Study = null, // TODO: See if we can get Analog to enter this data.

                // TODO: Role (President, Board member, barista and so on).
            });
        }
    }
}