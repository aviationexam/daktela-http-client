using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Xunit.Sdk;

namespace Daktela.HttpClient.Tests.Infrastructure.Attributes;

public sealed class ManualInlineDataAttribute : DataAttribute
{
    private readonly object[] _data;

    public ManualInlineDataAttribute(params object[] data)
    {
        if (!Debugger.IsAttached)
        {
            Skip = "Only running in interactive mode.";
        }

        _data = data;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod) =>
    [
        _data,
    ];
}
