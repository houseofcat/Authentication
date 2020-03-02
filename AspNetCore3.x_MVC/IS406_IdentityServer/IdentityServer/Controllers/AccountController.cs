using AutoMapper;
using IdentityServer.Requests;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMapper Mapper { get; }
        private IUserService UserService { get; }

        #region Constants
        private const string UserCreatedTemplate = "Sucessfully created user {0}.";
        private const string UserCreatedFailedTemplate = "Failed creating user {0}.";
        private const string UserCreatedAdditionalErrorTemplate = "\r\nError {0}: {1}";
        #endregion

        public AccountController(
            IMapper mapper,
            IUserService userService)
        {
            Mapper = mapper;
            UserService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(CreateUserRequest request)
        {
            if (request == null) { return BadRequest(); }

            // AutoMap the request into a new IdentityUser
            var identityUser = Mapper.Map<IdentityUser>(request);

            IdentityResult identityResult;
            try
            { identityResult = await UserService.CreateUserAsync(identityUser, request.Password); }
            catch (Exception ex)
            {
                var errorMessage = ServiceUtils.Write(UserCreatedFailedTemplate, identityUser.Email);
                Log.Logger.Error(ex, errorMessage);
                return BadRequest(errorMessage);
            }

            if (identityResult.Succeeded)
            {
                var logMessage = ServiceUtils.Write(UserCreatedTemplate, identityUser.Email);
                Log.Logger.Information(logMessage);
                return Ok(logMessage);
            }
            else
            {
                var errorMessage = ServiceUtils.Write(UserCreatedFailedTemplate, identityUser.Email);
                var badRequest = BadRequest(errorMessage);

                // Add additional details to internal logging - but not to the BadRequest message.
                for (int i = 0; i < identityResult.Errors.Count(); i++)
                { errorMessage += string.Format(UserCreatedAdditionalErrorTemplate, i, identityResult.Errors.ElementAt(i).Description); }

                Log.Logger.Error(errorMessage);
                return badRequest;
            }
        }
    }
}