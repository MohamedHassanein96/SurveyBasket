namespace SurveyBasket.Erorrs
{
    public class QuestionErrors
    {
        public static readonly Error QuestionNotFound = new (Code: "Question.NotFound", Description: "there is no question with the given Id",StatusCodes.Status404NotFound);
        public static readonly Error DuplicatedQuestionContent = new(Code: "Question. DuplicatedContent", Description: "another question with the same content was found in the same poll", StatusCodes.Status409Conflict);

    }
}
