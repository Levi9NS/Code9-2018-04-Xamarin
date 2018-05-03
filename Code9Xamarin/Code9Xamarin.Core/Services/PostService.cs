using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;

        public PostService(IRequestService requestService, IAuthenticationService authenticationService)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
        }

        public async Task<IEnumerable<PostDto>> GetAllPosts(string searchString, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/posts/all",
                Query = $"searchString={Uri.EscapeDataString(searchString)}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            return await _requestService.GetAsync<IEnumerable<PostDto>>(builder.Uri, token);
        }

        public async Task<PostDto> GetPost(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/posts/{id}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            return await _requestService.GetAsync<PostDto>(builder.Uri, token);
        }

        public async Task<bool> CreatePost(CreatePostDto post, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/posts"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.PostAsync<CreatePostDto, string>(builder.Uri, post, token);

            return await Task.FromResult(true);
        }

        public async Task<bool> EditPost(EditPostDto post, Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/posts/{id}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.PutAsync<EditPostDto, string>(builder.Uri, post, token);
            return await Task.FromResult(true);
        }

        public async Task<bool> LikePost(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/posts/{id}/reactToPost"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.PutAsync<EditPostDto, string>(builder.Uri, null, token);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeletePost(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/posts/{id}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.DeleteAsync<string, string>(builder.Uri, null, token);

            return await Task.FromResult(true);
        }
    }
}
