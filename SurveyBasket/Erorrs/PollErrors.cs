namespace SurveyBasket.Erorrs
{
    public  static class PollErrors
    {
        public static readonly Error PollNotFound = new Error("Poll.NotFound", "No Poll was found with this given Id");
        public static readonly Error DuplicatedPollTitle = new Error("Poll.DuplicatedTitle", "another poll with the same title is already existed");
    }
}
