﻿using Microsoft.AspNetCore.Http;
using VerificationProvider.Models;

namespace VerificationProvider.Services
{
    public interface IValidateVerificationCodeService
    {
        Task<ValidateRequest> UnpackValidateRequestAsync(HttpRequest req);
        Task<bool> UpdateEmailConfirmed(ValidateRequest request);
        Task<bool> ValidateCodeAsync(ValidateRequest validateRequest);
    }
}