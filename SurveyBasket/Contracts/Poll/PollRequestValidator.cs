
namespace Survey_Basket.Contracts.Poll
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().Length(3, 100).WithMessage("Please add at least 3 chars and the max is 100");
            RuleFor(x => x.StartsAt).NotEmpty().GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
            RuleFor(x => x.EndsAt).NotEmpty();
            RuleFor(x => x).Must(HasValidTime).WithName(nameof(PollRequest.EndsAt)).WithMessage("{PropertyName} must be greater than or equls start date");

        }
        public static bool HasValidTime(PollRequest request)
        {
            return request.EndsAt >= request.StartsAt;
        }
    }
}
