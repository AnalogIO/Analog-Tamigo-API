using Analog_Tamigo_API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Analog_Tamigo_API.Logic
{
    public interface ITamigoClient : IDisposable
    {
        Task<bool> IsOpen();
        Task<IEnumerable<ShiftDTO>> GetShifts();
        Task<IEnumerable<ShiftDTO>> GetShifts(DateTime date);
        Task<IEnumerable<ShiftDTO>> GetShifts(DateTime from, DateTime to);
    }
}