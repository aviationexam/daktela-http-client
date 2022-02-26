using System.Runtime.Serialization;

namespace Daktela.HttpClient.Api.Tickets
{
    public enum EQueueDirection
    {
        /// <summary>
        /// Incoming
        /// </summary>
        [EnumMember(Value = "in")]
        Incoming,

        /// <summary>
        /// Outgoing
        /// </summary>
        [EnumMember(Value = "out")]
        Outgoing,
    }
}
