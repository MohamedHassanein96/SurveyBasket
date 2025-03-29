namespace SurveyBasket.Authentication
{
    public record ConfirmEmailRequest(string UserId, string Code);
    
}
