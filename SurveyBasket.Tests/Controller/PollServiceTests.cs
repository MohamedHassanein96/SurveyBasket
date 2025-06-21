using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Survey_Basket.Contracts.Poll;
using Survey_Basket.Entities;
using Survey_Basket.Persistence;

using SurveyBasket.Services.PollService;


namespace SurveyBasket.Tests.Controller
{
    public class PollServiceTests
    {
        private readonly InMemoryDbContext _context;
        private readonly PollService _pollService;
        public PollServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .Options;


            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            _context = new InMemoryDbContext(options, httpContextAccessor);
            _pollService = new PollService(_context,null);
          
        }

        [Fact]
        public async Task GetAllAsync_WhenThereArePolls_ReturnsIEnumerableOfPollResponse()
        {
            //Arrange 

            var polls = new List<Poll>
            {
                new Poll
                {
                 Title = "poll1",
                Summary = "ss",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
                },
                new Poll
                {
                Title = "poll2",
                Summary = "ss",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)

                }

            };
         
          
            _context.Polls.AddRange(polls);
            await _context.SaveChangesAsync();



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
            var poll = new Poll
            {
                Title = "poll1",
                Summary = "existing",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
            };
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();



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
            int id = 1;

            //Act 
            var result = await _pollService.GetAsync(id, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);

        }

        [Fact]
        public async Task AddAsync_WhenPollTitleIsExisted_RetunsPollDuplicatedTitle()
        {
            // Arrange
            var poll = new Poll
            {
                Title = "poll1",
                Summary = "existing",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
            };
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            var request = new PollRequest(
                "poll1",
                "jdksajks",
                DateOnly.FromDateTime(DateTime.UtcNow), 
                DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
          

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
            var request = new PollRequest(
                "poll1", "jdksajks",
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));


            //Act
            var result = await _pollService.AddAsync(request, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("poll1", result.Value.Title);
            Assert.Single(_context.Polls);


        }

        [Fact]
        public async Task DeleteAsync_WhenPollIdIsNotExisted_ReturnsPollNotFound()
        {
            //Arrange
            int id = 1;
            var pollInDb = await _context.Polls.FindAsync(id);
            Assert.Null(pollInDb);
            //Act
            var result = await _pollService.DeleteAsync(id, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(result.Error.Code, "Poll.NotFound");
        }
        [Fact]
        public async Task DeleteAsync_WhenPollIdIsExisted_ReturnsSuccess()
        {
            //Arrange
            int id = 1;
            var poll = new Poll
            {
                Title = "poll1",
                Summary = "existing",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
            };
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

          

            //Act
            var result = await _pollService.DeleteAsync(id, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);

        }

        [Fact]
        public async Task UpdateAsync_WhenTitleAlreadyExistsForAnotherPoll_ReturnsDuplicatedPollTitleError()
        {
            //Arrange
            var poll = new Poll
            {
                Title = "poll1",
                Summary = "existing",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
            };
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            var pollRequest = new PollRequest("poll1", "shjhjj", DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
            
         

            //Act 
            var result = await _pollService.UpdateAsync(2, pollRequest);


            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(result.Error.Code, "Poll.DuplicatedTitle");
        }

        [Fact]
        public async Task UpdateAsync_WhenPollIdIsNotExisted_ReturnsPollNotFound()
        {
            //Arrange
            var pollRequest = new PollRequest("po", "shjhjj", DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
          

            //Act
            var result = await _pollService.UpdateAsync(1, pollRequest, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(result.Error.Code, "Poll.NotFound");
        }


        [Fact]
        public async Task UpdateAsync_WhenTitleDoesNotConflictAnotherPoll_ReturnsSuccess()
        {
            //Arrange
            var poll = new Poll
            {
                Title = "poll1",
                Summary = "existing",
                StartsAt = DateOnly.FromDateTime(DateTime.UtcNow),
                EndsAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1)
            };
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            var pollRequest = new PollRequest("poll1",
                "shjhjj",
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
           

            //Act 
            var result = await _pollService.UpdateAsync(1, pollRequest);


            //Assert
            Assert.True(result.IsSuccess);
          
        }

        [Fact]
        public void ThisShouldThrowException()
        {
            throw new Exception("This is a test exception to fail the pipeline.");
        }
    }

}
