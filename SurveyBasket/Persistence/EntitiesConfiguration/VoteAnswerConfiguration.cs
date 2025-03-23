﻿using SurveyBasket.Entities;

namespace SurveyBasket.Persistence.EntitiesConfiguration
{
    public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            builder.HasIndex(x => new { x.VoteId, x.QuestionId }).IsUnique();
        }
    }
}
