using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Daktela.HttpClient.Tests.Infrastructure.Attributes;

public sealed class ManualInlineDataAttribute : DataAttribute
{
    private readonly object[] _data;

    public override bool SupportsDiscoveryEnumeration() => true;

    public ManualInlineDataAttribute(params object[] data)
    {
        if (!Debugger.IsAttached)
        {
            Skip = "Only running in interactive mode.";
        }

        _data = data;
    }

    /// <inheritdoc/>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker
    )
    {
        var traits = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        TestIntrospectionHelper.MergeTraitsInto(traits, Traits);

        return ValueTask.FromResult<IReadOnlyCollection<ITheoryDataRow>>([
            new TheoryDataRow(_data)
            {
                Explicit = ExplicitAsNullable,
                Skip = Skip,
                TestDisplayName = TestDisplayName,
                Timeout = TimeoutAsNullable,
                Traits = traits,
            },
        ]);
    }
}
