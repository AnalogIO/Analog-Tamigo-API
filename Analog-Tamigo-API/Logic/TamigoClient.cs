using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Analog_Tamigo_API.Models.Responses;
using System.Net.Http;
using Analog_Tamigo_API.Models.Requests;
using Analog_Tamigo_API.Models;

namespace Analog_Tamigo_API.Logic
{
    public class TamigoClient : ITamigoClient
    {
        private readonly HttpClient _client;
        private Task<string> _userLoginTask;
        private string _userToken;
        private readonly Action _userRelogin;

        public TamigoClient(string email, string password)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _client.BaseAddress = new Uri("https://api.tamigo.com/");

            _userRelogin = () =>
            {
                _userLoginTask = UserLogin(email, password);
            };

            _userRelogin();
        }

        private async Task<string> UserLogin(string email, string password)
        {
            var credentials = new UserLoginRequest { Email = email, Password = password };
            var response = await _client.PostAsJsonAsync("login", credentials);

            if (response.IsSuccessStatusCode)
            {
                var loginresult = await response.Content.ReadAsAsync<LoginResponse>();
                return loginresult.SessionToken;
            }

            return null;
        }

        public async Task<IEnumerable<ShiftDTO>> GetShifts()
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }
            // Get future
            return await GetShifts(DateTime.Today, DateTime.Today.AddDays(7));
        }

        public async Task<IEnumerable<ShiftDTO>> GetShifts(DateTime date)
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }
            using (var result = await _client.GetAsync($"shifts/day/{date.ToString("yyyy-MM-dd")}/?securitytoken={_userToken}"))
            {
                return await RetrieveShiftsFromResponse(result);
            }
        }

        public async Task<IEnumerable<ShiftDTO>> GetShifts(DateTime from, DateTime to)
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }

            var result = new List<ShiftDTO>();

            var tasks = Enumerable
                .Range(0, (to - from).Days + 1)
                .Select(offset => from.AddDays(offset))
                .Select(async date => await GetShifts(date));

            foreach (var task in tasks)
            {
                result.AddRange(await task);
            }

            return result;
        }

        public async Task<bool> IsOpen()
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }
            return (await GetShifts(DateTime.Today)).Any(shift => shift.Open <= DateTime.Now && shift.Close >= DateTime.Now);
        }

        public async Task<IEnumerable<VolunteerDto>> GetEmployees()
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }

            using (var result = await _client.GetAsync($"contacts/?securitytoken={_userToken}"))
            {
                var contacts = await RetrieveContactsFromResponse(result);

                return contacts
                    .Where(contact => contact.Email != "analogen@cafeanalog.dk")
                    .Select(contact => new VolunteerDto
                {
                    Name = contact.FirstName,

                    // TODO: Define default
                    Photo = string.IsNullOrWhiteSpace(contact.ImageUrl) ? null : $"https://app.tamigo.com/Public/Photos/{Path.GetFileName(contact.ImageUrl)}",

                    Study = null, // TODO: See if we can get Analog to enter this data.

                    // TODO: Role (President, Board member, barista and so on).
                });
            }
        }

        private async Task<IEnumerable<Contact>> RetrieveContactsFromResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Contact>>();
            }

            // The api probably returned unauthorized, because the token needs to be refreshed.
            _userToken = null;
            _userRelogin();

            return new Contact[0];
        }

        private async Task<IEnumerable<ShiftDTO>> RetrieveShiftsFromResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var shifts = await response.Content.ReadAsAsync<IEnumerable<TamigoShift>>();

                return shifts.GroupBy(shift => shift.StartTime)
                    .Select(grouping => new ShiftDTO
                    {
                        Open = grouping.Key,
                        Close = grouping.First().EndTime,
                        Employees = grouping.Select(shift => shift.EmployeeName.Split(' ').First())
                    })
                    .OrderBy(shift => shift.Open)
                    .Where(shift => shift.Employees.Any(employee => employee != "Vacant"));
            }
            
            // The api probably returned unauthorized, because the token needs to be refreshed.
            _userToken = null;
            _userRelogin();

            return new ShiftDTO[0];
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}