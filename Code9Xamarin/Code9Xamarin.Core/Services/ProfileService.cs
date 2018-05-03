using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;

        public ProfileService(IRequestService requestService, IAuthenticationService authenticationService)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
        }

        public async Task<GetProfileDto> GetProfile(string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/profile"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            var profile = await _requestService.GetAsync<GetProfileDto>(builder.Uri, token);

            AppSettings.UserId = profile.UserId;

            return profile;
        }

        public async Task<bool> CreateProfile(CreateProfileDto profile)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/profile"
            };

            var message = await _requestService.PostAsync<CreateProfileDto, string>(builder.Uri, profile);

            return await Task.FromResult(true);
        }
    }
}
