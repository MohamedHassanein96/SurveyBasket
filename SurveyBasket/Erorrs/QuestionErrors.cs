namespace SurveyBasket.Erorrs
{
    public class QuestionErrors
    {
        public static readonly Error QuestionNotFound = new (Code: "Question.NotFound", Description: "there is no question with the given Id");
        public static readonly Error DuplicatedQuestionContent = new(Code: "Question. DuplicatedContent", Description: "another question with the same content was found in the same poll");

    }
}
