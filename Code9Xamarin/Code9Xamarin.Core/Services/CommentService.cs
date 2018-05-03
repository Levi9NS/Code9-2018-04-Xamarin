using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;

        public CommentService(IRequestService requestService, IAuthenticationService authenticationService)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
        }

        public async Task<IEnumerable<GetCommentDto>> GetPostComments(Guid postId, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/comments/GetPostComments",
                Query = $"postId={postId}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            return await _requestService.GetAsync<IEnumerable<GetCommentDto>>(builder.Uri, token);
        }

        public async Task<bool> CreateComment(Guid postId, string text, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"postId={postId}&text={text}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.PostAsync<string, string>(builder.Uri, null, token);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteComment(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"commentId={id}"
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
