﻿using SurveyBasket.Abstractions.Consts;

namespace SurveyBasket.Contracts.Users
{
    public class ChangePasswordRequestValidator :AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty()
                .Matches(RegexPatterns.Password)
                .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New Password cannot be the same with the current");
        }
    }
}
