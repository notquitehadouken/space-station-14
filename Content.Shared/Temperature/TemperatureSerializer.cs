using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Shared.Temperature;

/// <summary>
/// Makes temperatures.
/// Default units are kelvin.
/// </summary>
public sealed class TemperatureSerializer : ITypeSerializer<float, ValueDataNode>, ITypeSerializer<double, ValueDataNode>
{
    private static readonly Regex MatchNumber = new Regex("([0-9]+)");
    private static readonly Regex MatchLetter = new Regex("([a-z])", RegexOptions.IgnoreCase);

    public static bool ConvertToKelvin(string toConvert, [NotNullWhen(true)] out float? inKelvin)
    {
        inKelvin = null;
        if (toConvert.Length == 0)
        {
            return false;
        }

        var firstNumber = MatchNumber.Match(toConvert);
        var firstLetter = MatchLetter.Match(toConvert);
        if (!float.TryParse(firstNumber.Value, out var result))
        {
            return false;
        }

        if (firstLetter.Length == 0) // No letters found
        {
            inKelvin = result;
            return true;
        }
    }

    public ValidationNode Validate(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
    }

    public float Read(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<float>? instanceProvider = null)
    {
        throw new NotImplementedException();
    }

    public DataNode Write(ISerializationManager serializationManager,
        float value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
    }

    public double Read(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<double>? instanceProvider = null)
    {
        throw new NotImplementedException();
    }

    public DataNode Write(ISerializationManager serializationManager,
        double value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
    }
}
