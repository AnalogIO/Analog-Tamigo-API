using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TamigoServices.Models.Requests;
using TamigoServices.Models.Responses;

namespace TamigoServices
{
    public class TamigoUserClient : ITamigoUserClient
    {
        private readonly HttpClient _client;
        private Task<Guid?> _userLoginTask;
        private Guid? _userToken;
        private readonly Action _userRelogin;

        public TamigoUserClient(Uri apiUri, string email, string password)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _client.BaseAddress = apiUri;

            _userRelogin = () =>
            {
                _userLoginTask = UserLogin(email, password);
            };

            _userRelogin();
        }

        private async Task<Guid?> UserLogin(string email, string password)
        {
            var credentials = new UserLoginRequest { Email = email, Password = password };
            var response = await _client.PostAsJsonAsync("login", credentials);

            if (response.IsSuccessStatusCode)
            {
                var loginresult = await response.Content.ReadAsAsync<UserLoginResponse>();
                return loginresult.SessionToken;
            }

            return null;
        }

        public async Task<IEnumerable<Shift>> GetShifts()
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }
            // Get future
            return await GetShifts(DateTime.Today, DateTime.Today.AddDays(7));
        }

        public async Task<IEnumerable<Shift>> GetShifts(DateTime date)
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

        public async Task<IEnumerable<Shift>> GetShifts(DateTime from, DateTime to)
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }

            var result = new List<Shift>();

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
            return (await GetShifts(DateTime.Today)).Any(shift => shift.StartTime <= DateTime.Now && shift.EndTime >= DateTime.Now);
        }

        public async Task<IEnumerable<Contact>> GetContacts()
        {
            if (_userToken == null)
            {
                _userToken = await _userLoginTask;
                if (_userToken == null) throw new InvalidOperationException("Wrong username or password");
            }

            using (var result = await _client.GetAsync($"contacts/?securitytoken={_userToken}"))
            {
                return await RetrieveContactsFromResponse(result);
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

        private async Task<IEnumerable<Shift>> RetrieveShiftsFromResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var shifts = await response.Content.ReadAsAsync<IEnumerable<Shift>>();

                return shifts.OrderBy(shift => shift.StartTime);
            }
            
            // The api probably returned unauthorized, because the token needs to be refreshed.
            _userToken = null;
            _userRelogin();

            return new Shift[0];
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}