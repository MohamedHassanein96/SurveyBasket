
namespace SurveyBasket.Entities.Erorrs
{
    public  static class PollErrors
    {
        public static readonly Error PollNotFound = new Error("Poll.NotFound", "No Poll was found with this given Id");
    }
}
