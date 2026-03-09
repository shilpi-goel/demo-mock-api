using CourseModel = DfE.NCS.Course.Mock.Function.Models.Course;
using CourseUpdateModel = DfE.NCS.Course.Mock.Function.Models.CourseUpdate;

namespace DfE.NCS.Course.Mock.Function.DataGenerators
{
    internal static class CourseDataGenerator
    {
        private static readonly Random _random = new();

        // Delivery mode: 1=ClassroomBased, 2=Online, 3=WorkBased, 4=BlendedLearning
        private static readonly int[] DeliveryModes = [1, 2, 3, 4];

        // Duration unit: 1=Days, 2=Weeks, 3=Months, 4=Years, 5=Hours
        private static readonly int[] DurationUnits = [1, 2, 3, 4, 5];

        // Study mode: 1=FullTime, 2=PartTime, 3=Flexible
        private static readonly int[] StudyModes = [1, 2, 3];

        // Attendance pattern: 1=Daytime, 2=Evening, 3=Weekend, 4=DayOrBlockRelease
        private static readonly int[] AttendancePatterns = [1, 2, 3, 4];

        // Education level: 1=EntryLevel, 2=Level1, ..., 7=Level6
        private static readonly int[] EducationLevels = [1, 2, 3, 4, 5, 6, 7];

        // Update types: 1=NewlyAdded, 2=Updated, 3=Deleted
        private static readonly int[] UpdateTypes = [1, 2, 3];


        private static readonly string[] CourseNames =
        [
            "Introduction to Digital Marketing",
            "Data Science Fundamentals",
            "Project Management Essentials",
            "Full Stack Web Development",
            "Accounting and Finance Principles",
            "Health and Social Care",
            "Electrical Installation",
            "Business Administration",
            "Early Years Education",
            "Cyber Security Awareness",
            "Hospitality and Catering",
            "Construction Management",
            "Graphic Design for Beginners",
            "Leadership and Management",
            "Mental Health First Aid",
            "Environmental Sustainability",
            "Retail and Customer Service",
            "Supply Chain and Logistics",
            "Photography and Media",
            "Engineering Principles"
        ];

        private static readonly string[] SectorDescriptions =
        [
            "Business, Administration and Law",
            "Computing and ICT",
            "Construction, Planning and the Built Environment",
            "Education and Training",
            "Engineering and Manufacturing",
            "Health, Public Services and Care",
            "Hospitality, Leisure, Travel and Tourism",
            "Retail and Commercial Enterprise",
            "Science and Mathematics",
            "Arts, Media and Publishing"
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
            "Online Campus",
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

        private static readonly string[] QualificationLevels =
        [
            "Entry Level",
            "Level 1",
            "Level 2",
            "Level 3",
            "Level 4",
            "Level 5",
            "Level 6"
        ];

        private static readonly string[] AwardingOrganisations =
        [
            "City & Guilds",
            "BTEC",
            "OCR",
            "AQA",
            "Pearson",
            "NCFE",
            "CACHE",
            "IMI",
            "ILM",
            "CMI"
        ];

        public static List<CourseModel> Generate(int count = 100)
        {
            var courses = new List<CourseModel>(count);

            for (int i = 0; i < count; i++)
            {
                courses.Add(GenerateCourse());
            }

            return courses;
        }

        public static List<CourseUpdateModel> GenerateUpdates(int count = 100)
        {
            var courses = new List<CourseUpdateModel>(count);

            for (int i = 0; i < count; i++)
            {
                var course = GenerateCourse();
                courses.Add(new CourseUpdateModel
                {
                    Id = course.Id,
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    CourseType = course.CourseType,
                    SectorDescription = course.SectorDescription,
                    EducationLevel = course.EducationLevel,
                    AwardingBody = course.AwardingBody,
                    DeliveryMode = course.DeliveryMode,
                    FlexibleStartDate = course.FlexibleStartDate,
                    StartDate = course.StartDate,
                    CourseWebsite = course.CourseWebsite,
                    Cost = course.Cost,
                    CostDescription = course.CostDescription,
                    DurationUnit = course.DurationUnit,
                    DurationValue = course.DurationValue,
                    StudyMode = course.StudyMode,
                    AttendancePattern = course.AttendancePattern,
                    National = course.National,
                    Region = course.Region,
                    ParentRegion = course.ParentRegion,
                    WhoTheCourseIsFor = course.WhoTheCourseIsFor,
                    EntryRequirements = course.EntryRequirements,
                    WhatYoullLearn = course.WhatYoullLearn,
                    HowYoullLearn = course.HowYoullLearn,
                    WhatYoullNeed = course.WhatYoullNeed,
                    HowYoullBeAssessed = course.HowYoullBeAssessed,
                    WhatYouCanDoNext = course.WhatYouCanDoNext,
                    ProviderName = course.ProviderName,
                    ProviderWebsite = course.ProviderWebsite,
                    ProviderEmail = course.ProviderEmail,
                    ProviderPhoneNumber = course.ProviderPhoneNumber,
                    VenueName = course.VenueName,
                    Postcode = course.Postcode,
                    AddressLine1 = course.AddressLine1,
                    AddressLine2 = course.AddressLine2,
                    Town = course.Town,
                    County = course.County,
                    Latitude = course.Latitude,
                    Longitude = course.Longitude,
                    LearnAimRefTitle = course.LearnAimRefTitle,
                    QualificationLevel = course.QualificationLevel,
                    AwardingOrganisation = course.AwardingOrganisation,
                    UpdateType = PickRandom(UpdateTypes)
                });
            }

            return courses;
        }

        private static CourseModel GenerateCourse()
        {
            var location = PickRandom(Locations);
            var provider = PickRandom(Providers);
            var courseName = PickRandom(CourseNames);
            var awardingOrg = PickRandom(AwardingOrganisations);
            var qualLevel = PickRandom(QualificationLevels);
            var addressNumber = _random.Next(1, 200);
            var addressTemplate = PickRandom(AddressLine1Templates);
            var startDate = DateTime.UtcNow.AddDays(_random.Next(-180, 365));

            var providerSlug = provider.ToLower().Replace(" ", "");

            return new CourseModel
            {
                Id = Guid.NewGuid().ToString(),
                CourseId = Guid.NewGuid().ToString(),
                CourseName = courseName,
                CourseType = MaybeNull(_random.Next(1, 4)),
                SectorDescription = MaybeNull(PickRandom(SectorDescriptions)),
                EducationLevel = MaybeNull(PickRandom(EducationLevels)),
                AwardingBody = MaybeNull(awardingOrg),
                DeliveryMode = PickRandom(DeliveryModes),
                FlexibleStartDate = _random.Next(0, 2) == 1 ? "true" : "false",
                StartDate = startDate,
                CourseWebsite = $"https://www.{providerSlug}.ac.uk/courses/{courseName.ToLower().Replace(" ", "-")}",
                Cost = MaybeNull((decimal)_random.Next(0, 5000)),
                CostDescription = "Please contact the provider for cost information.",
                DurationUnit = PickRandom(DurationUnits),
                DurationValue = _random.Next(1, 24),
                StudyMode = PickRandom(StudyModes),
                AttendancePattern = PickRandom(AttendancePatterns),
                National = MaybeNull(_random.Next(0, 2)),
                Region = MaybeNull(location.County),
                ParentRegion = MaybeNull(location.County),
                WhoTheCourseIsFor = $"This course is designed for individuals looking to develop skills in {courseName.ToLower()}.",
                EntryRequirements = "No formal qualifications required. Applicants should be motivated and enthusiastic.",
                WhatYoullLearn = $"You will gain a solid understanding of the core concepts and practical skills in {courseName.ToLower()}.",
                HowYoullLearn = "Through a blend of classroom-based sessions, practical workshops, and self-directed study.",
                WhatYoullNeed = "A notepad and pen. Any specific materials will be provided by the provider.",
                HowYoullBeAssessed = "You will be assessed through a combination of assignments, practical tasks, and observations.",
                WhatYouCanDoNext = $"Following this course, you may progress to higher-level qualifications or seek employment in the sector.",
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
                LearnAimRefTitle = $"{qualLevel} Certificate in {courseName}",
                QualificationLevel = qualLevel,
                AwardingOrganisation = awardingOrg,
            };
        }


        private static T PickRandom<T>(T[] source)
            => source[_random.Next(source.Length)];

        private static T? MaybeNull<T>(T value, int nullChancePercent = 30) where T : struct
            => _random.Next(100) < nullChancePercent ? null : value;

        private static string MaybeNull(string value, int nullChancePercent = 30)
            => _random.Next(100) < nullChancePercent ? null! : value;
    }
}
