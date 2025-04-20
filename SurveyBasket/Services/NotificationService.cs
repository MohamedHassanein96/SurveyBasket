namespace SurveyBasket.Services
{
    public class NotificationService(ApplicationDbContext context,
        UserManager<ApplicationUser> userManger,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSenderService) : INotificationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManger = userManger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSenderService = emailSenderService;

        public async Task SendNewPollsNotification(int? pollId = null)
        {
            IEnumerable<Poll> polls = [];

            if (pollId.HasValue)
            {
                var poll = await _context.Polls.SingleOrDefaultAsync(x => x.Id == pollId && x.IsPublished);
                polls = [poll!];
            }
            else
            {
                polls = await _context.Polls.
                    Where(x => x.IsPublished && x.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow)).
                    AsNoTracking().ToListAsync();
            }

            var users = await _userManger.GetUsersInRoleAsync(DefaultRoles.Member.Name);

            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            foreach (var poll in polls)
            {
                foreach (var user in users)
                {
                    var placegolders = new Dictionary<string, string>
                    {
                        {"{{name}}",user.FirstName },
                        {"{{pollTill}}",poll.Title },
                        {"{{endDate}}",poll.EndsAt.ToString()},
                        {"{{url}}",$"{origin}/polls/start/{poll.Id}" }
                    };
                    var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placegolders);
                    await _emailSenderService.SendEmailAsync(user.Email!, $"Survey Basket : New Poll {poll.Title}", body);
                }
            }
        }
    }
}
