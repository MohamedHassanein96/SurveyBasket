﻿namespace SurveyBasket.Persistence.EntitiesConfiguration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(x => new { x.PollId, x.Content });
            builder.Property(x => x.Content).HasMaxLength(1000);
        }
    }
}
