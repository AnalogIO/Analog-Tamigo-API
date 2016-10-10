using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TamigoServices.Models.Responses;

namespace TamigoServices
{
    public interface ITamigoUserClient : IDisposable
    {
        Task<bool> IsOpen();
        Task<IEnumerable<Shift>> GetShifts();
        Task<IEnumerable<Shift>> GetShifts(DateTime date);
        Task<IEnumerable<Shift>> GetShifts(DateTime from, DateTime to);
        Task<IEnumerable<Contact>> GetContacts();
    }
}