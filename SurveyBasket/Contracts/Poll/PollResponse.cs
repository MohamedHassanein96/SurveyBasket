namespace Survey_Basket.Contracts.Poll
{
    public record PollResponse(int Id, string Title, string Summary, bool IsPublished, DateOnly StartsAt, DateOnly EndsAt);

}
