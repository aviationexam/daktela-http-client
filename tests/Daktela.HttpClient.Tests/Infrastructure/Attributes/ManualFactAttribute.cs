using System.Diagnostics;
using Xunit;

namespace Daktela.HttpClient.Tests.Infrastructure.Attributes;

public class ManualFactAttribute : FactAttribute
{
    public string Reason { get; set; } = "Only running in interactive mode.";

    public ManualFactAttribute()
    {
        if (!Debugger.IsAttached)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Skip = Reason;
        }
    }
}
