
namespace SurveyBasket.Contracts.Question
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().Length(3, 1000);
            RuleFor(x => x.Answers).NotNull();

            RuleFor(x => x.Answers).Must(x => x.Count > 1)
                .WithMessage("Question should be at leats 2 answers")
                .When(x => x.Answers != null);


            RuleFor(x => x.Answers).Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("you cannot add duplicated answers for the same questions")
                .When(x => x.Answers != null);
        }
    }
}
