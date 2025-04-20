namespace SurveyBasket.Abstractions.Consts
{
    public static class DefaultRoles
    {
        public partial class Admin
        {
            public const string Name = nameof(Admin);
            public const string Id = "0196023e-edf2-7363-8c14-5091696ceb85";
            public const string ConcurrencyStamp = "0196025c-6426-7421-b4e4-5ae55a02eb1b";
        }

        public partial class Member
        {
            public const string Name = nameof(Member);
            public const string Id = "0196023e-edf2-7363-8c14-5090d303b628";
            public const string ConcurrencyStamp = "0196025c-6431-76bf-aca6-0b477b80d455";
        }




    }
}
