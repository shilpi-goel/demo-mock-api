namespace DfE.NCS.Course.Mock.Function.Models
{
    public class EnumDescriptor
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public EnumDescriptor(int value, string description)
        {
            Value = value;
            Description = description;
        }
    }

    public class StringEnumDescriptor
    {
        public string Value { get; set; }
        public string Description { get; set; }

        public StringEnumDescriptor(string value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
