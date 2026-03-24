using DfE.NCS.Course.Mock.Function.Configuration;
using DfE.NCS.Course.Mock.Function.DataGenerators;
using DfE.NCS.Course.Mock.Function.Models;

namespace DfE.NCS.Course.Mock.Function.Storage
{
    public class CourseStore : ICourseStore
    {
        public IReadOnlyList<CourseUpdate> Courses { get; }

        public CourseStore(IPaginationSettings paginationSettings)
        {
            Courses = CourseDataGenerator.GenerateUpdates(paginationSettings.DefaultTotalCount);
        }
    }
}
