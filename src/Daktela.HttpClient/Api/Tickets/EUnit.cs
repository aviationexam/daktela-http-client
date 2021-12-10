using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets
{
    public enum ETimeUnit : byte
    {
        /// <summary>
        /// Hours
        /// </summary>
        [EnumMember(Value = "HOURS")]
        Hours = 0,

        /// <summary>
        /// Minutes
        /// </summary>
        [EnumMember(Value = "MINUTES")]
        Minutes = 1,
    }
}
