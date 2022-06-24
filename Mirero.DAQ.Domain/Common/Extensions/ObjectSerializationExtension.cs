using System.Text;
using System.Text.Json;

namespace Mirero.DAQ.Domain.Common.Extensions;

public static class ObjectSerializationExtension
{
    public static string ToBase64(this object @object)
    {
        return Convert.ToBase64String(
            Encoding.Default.GetBytes(
                JsonSerializer.Serialize(@object)));
    }
}