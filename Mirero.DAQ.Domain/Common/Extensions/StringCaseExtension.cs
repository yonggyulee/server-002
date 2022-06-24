using System.Text.RegularExpressions;

namespace Mirero.DAQ.Domain.Common.Extensions;

public static class StringCaseExtension
{
    public static string ToUpperSnakeCase(this string? text)
    {
        var pattern = 
            new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");

        return text == null
            ? ""
            : string
                .Join("_", pattern.Matches(text).Cast<Match>().Select(m => m.Value))
                .ToUpper();
    }
}