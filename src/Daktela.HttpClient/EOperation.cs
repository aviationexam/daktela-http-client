using Aviationexam.GeneratedJsonConverters.Attributes;
using System;

namespace Daktela.HttpClient;

[Flags]
[EnumJsonConverter]
public enum EOperation : byte
{
    Read = 1 << 0,
    Create = 1 << 1,
    Update = 1 << 2,
    Delete = 1 << 3,
}
