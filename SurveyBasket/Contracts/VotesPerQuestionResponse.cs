﻿using SurveyBasket.Contracts.Result;

namespace SurveyBasket.Contracts
{
    public record VotesPerQuestionResponse(string Content , IEnumerable<VotesPerAnswerResponse> VotesPerAnswerResponses);
    
}
