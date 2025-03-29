﻿namespace SurveyBasket.Authentication
{
    public class ResendConfirmationEmailRequestValidator:AbstractValidator<ResendConfirmationEmailRequest>
    {
        public ResendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
