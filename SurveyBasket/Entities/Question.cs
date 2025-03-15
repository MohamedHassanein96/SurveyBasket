namespace SurveyBasket.Entities
{
    public class Question :AuditableEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

       public int PollId { get; set; }
        public Poll Poll { get; set; } = default!;


        public bool IsActive { get; set; } = true;
        public ICollection<Answer> Answers { get; set; } = default!;
    }
}
