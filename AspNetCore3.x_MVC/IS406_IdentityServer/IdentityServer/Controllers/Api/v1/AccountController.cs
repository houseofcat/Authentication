using AutoMapper;
using IdentityServer.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IMapper Mapper { get; }
        private UserManager<IdentityUser> UserManager { get; }

        #region Constants
        private const string UserCreatedTemplate = "Sucessfully created user {0}.";
        private const string UserCreatedFailedTemplate = "Failed creating user {0}.";
        private const string UserCreatedAdditionalErrorTemplate = "\r\nError {0}: {1}";
        #endregion

        public AccountController(
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            Mapper = mapper;
            UserManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        //TODO: [Authorize(Roles = "Administrator")]
        [Route("create")]
        public async Task<IActionResult> CreateAsync(CreateUserRequest request)
        {
            if (request == null) { return BadRequest(); }

            // AutoMap the request into a new IdentityUser
            var identityUser = Mapper.Map<IdentityUser>(request);

            IdentityResult identityResult;
            try
            { identityResult = await UserManager.CreateAsync(identityUser, request.Password); }
            catch (Exception ex)
            {
                var errorMessage = Utils.Write(UserCreatedFailedTemplate, identityUser.NormalizedUserName);
                Log.Logger.Error(ex, errorMessage);
                return BadRequest(errorMessage);
            }

            if (identityResult.Succeeded)
            {
                var logMessage = Utils.Write(UserCreatedTemplate, identityUser.NormalizedUserName);
                Log.Logger.Information(logMessage);
                return Ok(logMessage);
            }
            else
            {
                var errorMessage = Utils.Write(UserCreatedFailedTemplate, identityUser.NormalizedUserName);
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