namespace SurveyBasket.Contracts.Users
{
    public class UpdateUserRequestValidator :AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Email).NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Roles)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("you cannot add duplicated role for the same user")
                .When(x => x.Roles != null);
        }
    }
}
