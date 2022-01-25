namespace WarcraftGuild.Domain.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumCodeAttribute : Attribute
    {
        public EnumCodeAttribute(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}