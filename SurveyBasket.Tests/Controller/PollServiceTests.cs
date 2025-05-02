using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Survey_Basket.Abstractions;
using Survey_Basket.Contracts.Poll;
using Survey_Basket.Entities;
using Survey_Basket.Persistence;
using SurveyBasket.Contracts.Roles;
using SurveyBasket.Services.PollService;
using System;
using System.Security.Cryptography;
using System.Threading;

namespace SurveyBasket.Tests.Controller
{
    public class PollServiceTests
    {

        public PollServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;

            using var context = new InMemoryDbContext(options,null);
        }

        [Fact]
        public async Task GetAllAsync_WhenThereArePolls_ReturnsIEnumerableOfPollResponse()
        {
            //Arrange 

            var _pollService = A.Fake<IPollService>();
           

            A.CallTo(() => _pollService.GetAllAsync(A<CancellationToken>.Ignored))
              .Returns(new List<PollResponse> {
                 new PollResponse (1, "s", "sjkas", true,  DateOnly.FromDateTime(DateTime.UtcNow),  DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1))
              });

            //Act
            var result = await _pollService.GetAllAsync(CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task GetAllAsync_WhenThereAreNoPolls_ReturnsEmptyList()
        {
            //Arrange 
            var _pollService = A.Fake<IPollService>();


            A.CallTo(() => _pollService.GetAllAsync(A<CancellationToken>.Ignored))
              .Returns(new List<PollResponse>()); 
              

            //Act
            var result = await _pollService.GetAllAsync(CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAsync_WhenIdIsExisted_RetunsTheSpecificPoll()
        {
            //Arrange 
            var _pollService = A.Fake<IPollService>();
            var pollResponse = new PollResponse(
             1, "Title", "Description", true,
             DateOnly.FromDateTime(DateTime.UtcNow),
             DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));

            A.CallTo(() => _pollService.GetAsync(1, A<CancellationToken>.Ignored))
           .Returns(Task.FromResult(Result.Success(pollResponse)));


            //Act 
            var result = await _pollService.GetAsync(1, CancellationToken.None);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1,result.Value.Id);
        }
        [Fact]
        public async Task GetAsync_WhenIdIsNotExisted_RetunsPollNotFound()
        {
            //Arrange 
            var _pollService = A.Fake<IPollService>();
            var error = new Error("Poll.NotFound", "PollNotFound", 404);
          

            A.CallTo(() => _pollService.GetAsync(1, A<CancellationToken>.Ignored))
           .Returns(Task.FromResult(Result.Failure<PollResponse>(error)));


            //Act 
            var result = await _pollService.GetAsync(1, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);
           

        }

        [Fact]
        public async Task AddAsync_WhenPollTitleIsExisted_RetunsPollDuplicatedTitle()
        {
            // Arrange
            var _pollService = A.Fake<IPollService>();
            var request = new PollRequest("poll1", "jdksajks", DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
            var error = new Error("Poll.DuplicatedTitle", "DuplicatedTitle", 409);

            A.CallTo(() => _pollService.AddAsync(request, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(Result.Failure<PollResponse>(error)));

            //Act
            var result = await _pollService.AddAsync(request, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.Error.Code, "Poll.DuplicatedTitle");

        }
        [Fact]
        public async Task AddAsync_WhenPollTitleIsNotExisted_RetunsTheAddedPoll()
        {
            // Arrange
            var _pollService = A.Fake<IPollService>();
            var request = new PollRequest("poll1", "jdksajks", DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
            var pollResponse = new PollResponse(
             1, "Title", "Description", true,
             DateOnly.FromDateTime(DateTime.UtcNow),
             DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));

            A.CallTo(() => _pollService.AddAsync(request, A<CancellationToken>.Ignored))
                .Returns(Task.FromResult(Result.Success(pollResponse)));

            //Act
            var result = await _pollService.AddAsync(request, CancellationToken.None);

            //Assert
            //Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Id);
         

        }
    }

}
