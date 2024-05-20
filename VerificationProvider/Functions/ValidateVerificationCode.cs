using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationProvider.Data.Contexts;
using VerificationProvider.Data.Entities;
using VerificationProvider.Services;

namespace VerificationProvider.Functions;

public class ValidateVerificationCode(ILogger<ValidateVerificationCode> logger, IValidateVerificationCodeService validateVerificationCodeService, UserManager<UserEntity> userManager, DataContext context)
{
    private readonly ILogger<ValidateVerificationCode> _logger = logger;
    private readonly IValidateVerificationCodeService _validateVerificationCodeService = validateVerificationCodeService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly DataContext _context = context;

    [Function("ValidateVerificationCode")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        try
        {
            var validateRequest = await _validateVerificationCodeService.UnpackValidateRequestAsync(req);
            if (validateRequest != null)
            {
                var validateResult = await _validateVerificationCodeService.ValidateCodeAsync(validateRequest);

                if (validateResult)
                {
                    var updateEmailConfirmedResult = await _validateVerificationCodeService.UpdateEmailConfirmed(validateRequest);
                    if (updateEmailConfirmedResult)
                    {
                        return new OkResult();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : ValidateVerificationCode.Run :: {ex.Message}");
        }
        return new UnauthorizedResult();
    }


}
