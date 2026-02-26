namespace DfE.NCS.Course.Mock.Function.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public int? CourseType { get; set; }
        public string SectorDescription { get; set; }
        public string EducationLevel { get; set; }
        public string AwardingBody { get; set; }
        public int DeliveryMode { get; set; }
        public string FlexibleStartDate { get; set; }
        public DateTime StartDate { get; set; }
        public string CourseWebsite { get; set; }
        public decimal? Cost { get; set; }
        public string CostDescription { get; set; }
        public int DurationUnit { get; set; }
        public int DurationValue { get; set; }
        public int StudyMode { get; set; }
        public int AttendancePattern { get; set; }
        public int? National { get; set; }
        public string Region { get; set; }
        public string ParentRegion { get; set; }
        public string WhoTheCourseIsFor { get; set; }
        public string EntryRequirements { get; set; }
        public string WhatYoullLearn { get; set; }
        public string HowYoullLearn { get; set; }
        public string WhatYoullNeed { get; set; }
        public string HowYoullBeAssessed { get; set; }
        public string WhatYouCanDoNext { get; set; }
        public string ProviderName { get; set; }
        public string ProviderWebsite { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderPhoneNumber { get; set; }
        public string VenueName { get; set; }
        public string Postcode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LearnAimRefTitle { get; set; }
        public string QualificationLevel { get; set; }
        public string AwardingOrganisation { get; set; }
        public UpdateType? UpdateType { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
