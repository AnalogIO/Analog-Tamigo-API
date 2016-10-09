using Analog_Tamigo_API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Analog_Tamigo_API.Models;

namespace Analog_Tamigo_API.Logic
{
    public interface ITamigoClient : IDisposable
    {
        Task<bool> IsOpen();
        Task<IEnumerable<ShiftDTO>> GetShifts();
        Task<IEnumerable<ShiftDTO>> GetShifts(DateTime date);
        Task<IEnumerable<ShiftDTO>> GetShifts(DateTime from, DateTime to);
        Task<IEnumerable<VolunteerDto>> GetEmployees();
    }
}