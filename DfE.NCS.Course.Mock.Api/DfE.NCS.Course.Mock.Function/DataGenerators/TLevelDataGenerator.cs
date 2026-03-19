using TLevelModel = DfE.NCS.Course.Mock.Function.Models.TLevel;
using TLevelUpdateModel = DfE.NCS.Course.Mock.Function.Models.TLevelUpdate;

namespace DfE.NCS.Course.Mock.Function.DataGenerators
{
    internal static class TLevelDataGenerator
    {
        private static readonly Random _random = new();

        // T-Levels are always Level 3 qualifications
        private const int TLevelQualificationLevel = 3;

        // Update types: 1=NewlyAdded, 2=Updated, 3=Deleted
        private static readonly int[] UpdateTypes = [1, 2, 3];

        private static readonly string[] CourseNames =
        [
            "T Level in Digital Production, Design and Development",
            "T Level in Digital Business Services",
            "T Level in Digital Support Services",
            "T Level in Digital Infrastructure",
            "T Level in Health",
            "T Level in Science",
            "T Level in Healthcare Science",
            "T Level in Education and Childcare",
            "T Level in Building Services Engineering",
            "T Level in Design, Surveying and Planning for Construction",
            "T Level in Onsite Construction",
            "T Level in Engineering, Manufacturing, Processing and Control",
            "T Level in Maintenance, Installation and Repair",
            "T Level in Agriculture, Land Management and Production",
            "T Level in Animal Care and Management",
            "T Level in Craft and Design",
            "T Level in Cultural Heritage and Visitor Attractions",
            "T Level in Legal Services",
            "T Level in Accounting",
            "T Level in Finance"
        ];

        private static readonly string[] Providers =
        [
            "City College Bristol",
            "Midlands Learning Hub",
            "Northern Skills Academy",
            "South East Training Centre",
            "West London Institute",
            "Yorkshire Professional College",
            "Greater Manchester Skills",
            "Thames Valley Learning",
            "East Anglia College",
            "Merseyside Training Solutions"
        ];

        private static readonly string[] VenueNames =
        [
            "Main Campus",
            "City Centre Hub",
            "Digital Learning Suite",
            "Skills and Innovation Centre",
            "Professional Development Centre",
            "Community Learning Centre"
        ];

        private static readonly (string Town, string County, string Postcode, decimal Lat, decimal Lng)[] Locations =
        [
            ("Bristol", "Avon", "BS1 4DJ", 51.4545m, -2.5879m),
            ("Birmingham", "West Midlands", "B1 1BB", 52.4862m, -1.8904m),
            ("Manchester", "Greater Manchester", "M1 1AE", 53.4808m, -2.2426m),
            ("Leeds", "West Yorkshire", "LS1 1BA", 53.8008m, -1.5491m),
            ("London", "Greater London", "EC1A 1BB", 51.5074m, -0.1278m),
            ("Sheffield", "South Yorkshire", "S1 1AA", 53.3811m, -1.4701m),
            ("Liverpool", "Merseyside", "L1 1JF", 53.4084m, -2.9916m),
            ("Norwich", "Norfolk", "NR1 1AA", 52.6309m, 1.2974m),
            ("Cambridge", "Cambridgeshire", "CB1 1AA", 52.2053m, 0.1218m),
            ("Reading", "Berkshire", "RG1 1AA", 51.4543m, -0.9781m)
        ];

        private static readonly string[] AddressLine1Templates =
        [
            "{0} High Street",
            "{0} Park Road",
            "{0} College Lane",
            "{0} Learning Way",
            "{0} Innovation Drive"
        ];

        public static List<TLevelModel> Generate(int count = 100)
        {
            var tLevels = new List<TLevelModel>(count);

            for (int i = 0; i < count; i++)
            {
                tLevels.Add(GenerateTLevel());
            }

            return tLevels;
        }

        public static List<TLevelUpdateModel> GenerateUpdates(int count = 100)
        {
            var tLevels = new List<TLevelUpdateModel>(count);

            for (int i = 0; i < count; i++)
            {
                var tLevel = GenerateTLevel();
                tLevels.Add(new TLevelUpdateModel
                {
                    TLevelId = tLevel.TLevelId,
                    CourseName = tLevel.CourseName,
                    StartDate = tLevel.StartDate,
                    CourseWebsite = tLevel.CourseWebsite,
                    WhoTheCourseIsFor = tLevel.WhoTheCourseIsFor,
                    EntryRequirements = tLevel.EntryRequirements,
                    WhatYoullLearn = tLevel.WhatYoullLearn,
                    HowYoullLearn = tLevel.HowYoullLearn,
                    HowYoullBeAssessed = tLevel.HowYoullBeAssessed,
                    WhatYouCanDoNext = tLevel.WhatYouCanDoNext,
                    ProviderName = tLevel.ProviderName,
                    ProviderWebsite = tLevel.ProviderWebsite,
                    ProviderEmail = tLevel.ProviderEmail,
                    ProviderPhoneNumber = tLevel.ProviderPhoneNumber,
                    VenueName = tLevel.VenueName,
                    Postcode = tLevel.Postcode,
                    AddressLine1 = tLevel.AddressLine1,
                    AddressLine2 = tLevel.AddressLine2,
                    Town = tLevel.Town,
                    County = tLevel.County,
                    Latitude = tLevel.Latitude,
                    Longitude = tLevel.Longitude,
                    TLevelQualificationLevel = tLevel.TLevelQualificationLevel,
                    UpdateType = PickRandom(UpdateTypes)
                });
            }

            return tLevels;
        }

        private static TLevelModel GenerateTLevel()
        {
            var location = PickRandom(Locations);
            var provider = PickRandom(Providers);
            var courseName = PickRandom(CourseNames);
            var addressNumber = _random.Next(1, 200);
            var addressTemplate = PickRandom(AddressLine1Templates);
            var startDate = DateTime.UtcNow.AddDays(_random.Next(-180, 365));
            var providerSlug = provider.ToLower().Replace(" ", "");
            var courseSlug = courseName.ToLower().Replace(" ", "-");

            return new TLevelModel
            {
                TLevelId = Guid.NewGuid().ToString(),
                CourseName = courseName,
                StartDate = startDate,
                CourseWebsite = $"https://www.{providerSlug}.ac.uk/t-levels/{courseSlug}",
                WhoTheCourseIsFor = $"This T Level is designed for individuals aged 16-19 who want to develop specialist skills in {courseName.Replace("T Level in ", "").ToLower()}.",
                EntryRequirements = "Typically 5 GCSEs at grade 4 or above, including English and Maths. Applicants should demonstrate enthusiasm and commitment to their chosen sector.",
                WhatYoullLearn = $"You will gain in-depth knowledge and practical skills in {courseName.Replace("T Level in ", "").ToLower()}, preparing you for skilled employment or higher education.",
                HowYoullLearn = "Through a combination of classroom-based learning and a minimum 45-day industry placement with an employer.",
                HowYoullBeAssessed = "You will be assessed through written examinations, employer-set projects, and an occupational specialism assessment.",
                WhatYouCanDoNext = $"Following this T Level, you may progress to higher apprenticeships, degree-level study, or skilled employment in the {courseName.Replace("T Level in ", "").ToLower()} sector.",
                ProviderName = provider,
                ProviderWebsite = MaybeNull($"https://www.{providerSlug}.ac.uk"),
                ProviderEmail = MaybeNull($"info@{providerSlug}.ac.uk"),
                ProviderPhoneNumber = MaybeNull($"0117 {_random.Next(100, 999)} {_random.Next(1000, 9999)}"),
                VenueName = PickRandom(VenueNames),
                Postcode = location.Postcode,
                AddressLine1 = string.Format(addressTemplate, addressNumber),
                AddressLine2 = string.Empty,
                Town = location.Town,
                County = location.County,
                Latitude = location.Lat,
                Longitude = location.Lng,
                TLevelQualificationLevel = TLevelQualificationLevel
            };
        }

        private static T PickRandom<T>(T[] source)
            => source[_random.Next(source.Length)];

        private static string MaybeNull(string value, int nullChancePercent = 30)
            => _random.Next(100) < nullChancePercent ? null! : value;
    }
}
