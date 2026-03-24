using DfE.NCS.Course.Mock.Function.Models;

namespace DfE.NCS.Course.Mock.Function.Storage
{
    public interface ICourseStore
    {
        IReadOnlyList<CourseUpdate> Courses { get; }
    }
}
