using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using Analog_Tamigo_API.Models;
using Analog_Tamigo_API.Models.Responses;

namespace Analog_Tamigo_API.Logic
{
    public class CachedTamigoClient : ITamigoClient
    {
        private static readonly TimeSpan FiveMinutes = TimeSpan.FromMinutes(5);
        private readonly ITamigoClient _client;
        private List<ShiftDTO> _cache;
        private DateTime _lastRefresh;

        public CachedTamigoClient(ITamigoClient client)
        {
            _client = client;
            _cache = new List<ShiftDTO>();
            FillCacheBackground();
        }

        public void Dispose()
        {
            _client.Dispose();
            _cache.Clear();
        }

        public void FillCacheBackground()
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct => FillCache());
        }

        public void FillCacheBackground(DateTime from, DateTime to)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct => FillCache(from,to));
        }

        public async Task FillCache()
        {
            if (DateTime.Now - _lastRefresh > FiveMinutes
                || !_cache.Any()
                || (_cache.Any(shift => shift.Open < DateTime.Now) && DateTime.Now.Subtract(_cache.Where(shift => shift.Open < DateTime.Now).Max(shift => shift.Open)) < DateTime.Now.Subtract(_lastRefresh)))
            {
                _lastRefresh = DateTime.Now;
                var newCache = new List<ShiftDTO>();
                newCache.AddRange(await _client.GetShifts());
                _cache = newCache;
            }
        }

        public async Task FillCache(DateTime from, DateTime to)
        {
            if (DateTime.Now - _lastRefresh > FiveMinutes
                || !_cache.Any()
                || (_cache.Any(shift => shift.Open < DateTime.Now) && DateTime.Now.Subtract(_cache.Where(shift => shift.Open < DateTime.Now).Max(shift => shift.Open)) < DateTime.Now.Subtract(_lastRefresh)))
            {
                _lastRefresh = DateTime.Now;
                var newCache = new List<ShiftDTO>();
                newCache.AddRange(await _client.GetShifts(from,to));
                _cache = newCache;
            }
        }

        public Task<bool> IsOpen()
        {
            FillCacheBackground();
            return Task.FromResult(_cache.Any(shift => shift.Open < DateTime.Now && DateTime.Now < shift.Close));
        }

        public Task<IEnumerable<ShiftDTO>> GetShifts()
        {
            FillCacheBackground();
            return Task.FromResult(_cache.AsEnumerable());
        }

        public async Task<IEnumerable<ShiftDTO>> GetShifts(DateTime date)
        {
            FillCacheBackground();
            if (_cache.Exists(shift => shift.Open.Date == date.Date))
                return _cache.Where(d => d.Open.Date == date.Date);
            return await _client.GetShifts(date);
        }

        public async Task<IEnumerable<ShiftDTO>> GetShifts(DateTime @from, DateTime to)
        {
            FillCacheBackground(from,to);
            if (_cache.Exists(shift => shift.Open.Date == from.Date) &&
                _cache.Exists(shift => shift.Open.Date == to.Date))
                return _cache.Where(shift => shift.Open > from && shift.Close < to);
            return await _client.GetShifts(from, to);
        }

        public Task<IEnumerable<VolunteerDto>> GetEmployees()
        {
            // TODO: Maybe cache output.
            return _client.GetEmployees();
        }
    }
}