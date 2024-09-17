using System.Collections.Specialized;
using System.Text;

namespace Starfruit.RouterLib;

public record RouteSegment
{
    public string? SegmentName { get; set; }
    public NameValueCollection Parameters { get; set; } = new NameValueCollection();
}
public static class RoutePathParser
{
    private enum TokenType
    {
        Segment,
        Key,
        Value
    }

    public static QueueStack<RouteSegment> Parse(string routePath)
    {
        if (!routePath.EndsWith("/"))
        {
            routePath += '/';
        }
        int cursor = 0;
        var result = new QueueStack<RouteSegment>();
        var currentTokenType = TokenType.Segment;
        StringBuilder currentSegment = new();
        StringBuilder currentKey = new();
        StringBuilder currentValue = new();
        RouteSegment currentRouteSegment = new();
        var length = routePath.Length;

        while (cursor < length)
        {
            var currentChar = routePath[cursor];
            if (currentChar == '\\' && cursor + 1 < length)
            {//escape character
                currentChar = routePath[++cursor];
            }
            else
            {
                if (currentChar == '/')
                {
                    //close the previous Segment
                    if (currentTokenType == TokenType.Key)
                    {
                        throw new InvalidRouteException($"Unable to parse '{routePath}'. Unexpected '/' at postion {cursor}. Key: {currentKey} has no value.");
                    }
                    else if (currentTokenType == TokenType.Value)
                    {
                        currentRouteSegment.Parameters.Add(currentKey.ToString(), currentValue.ToString());
                    }
                    else // currentTokenType == TokenType.Segment
                    {
                        currentRouteSegment.SegmentName = currentSegment.ToString();
                    }
                    result.Enqueue(currentRouteSegment);

                    //prepare for the next Segment
                    currentRouteSegment = new RouteSegment();
                    currentTokenType = TokenType.Segment;
                    currentSegment = new StringBuilder();
                    cursor++;
                    continue;
                }
                else if (currentChar == ':')
                {
                    //close the previous Value
                    if (currentTokenType == TokenType.Value)
                    {
                        currentRouteSegment.Parameters.Add(currentKey.ToString(), currentValue.ToString());
                    }
                    else if (currentTokenType == TokenType.Segment)
                    {
                        currentRouteSegment.SegmentName = currentSegment.ToString();
                    }
                    else // currentTokenType == TokenType.Key
                    {
                        throw new InvalidRouteException($"Unable to parse '{routePath}'. Unexpected ':' at postion {cursor}. Key: {currentKey} has no value.");
                    }

                    //prepare for the next Key
                    currentTokenType = TokenType.Key;
                    currentKey = new StringBuilder();
                    cursor++;
                    continue;
                }
                else if (currentChar == '=')
                {
                    //close the previous token
                    if (currentTokenType == TokenType.Value)
                    {
                        throw new InvalidRouteException($"Unable to parse '{routePath}'. Unexpected '=' at postion {cursor}. Missing key for the value after '{currentValue}'.");
                    }
                    else if (currentTokenType == TokenType.Segment)
                    {
                        throw new InvalidRouteException($"Unable to parse '{routePath}'. Unexpected '=' at postion {cursor}. Cannot assign value to a segment '{currentSegment}'");
                    }

                    currentTokenType = TokenType.Value;
                    currentValue = new StringBuilder();
                    cursor++;
                    continue;
                }
            }

            switch (currentTokenType)
            {
                case TokenType.Key:
                    currentKey.Append(currentChar);
                    break;
                case TokenType.Value:
                    currentValue.Append(currentChar);
                    break;
                default:
                    currentSegment.Append(currentChar);
                    break;
            }

            cursor++;
        }

        return result;
    }

    public static QueueStack<RouteSegment> Combine(string basePath, string relativePath)
    {
        QueueStack<RouteSegment> parsedRelativePath = Parse(relativePath);
        QueueStack<RouteSegment> result = Parse(basePath);
        while (parsedRelativePath.TryDequeue(out var segment))
        {
            if (segment is null || segment.SegmentName == ".")
            {
                continue;
            }
            if (segment.SegmentName == "..")
            {
                if (result.Count == 0)
                {
                    throw new InvalidRouteException($"Cannot go up from '{basePath}'. The relative path '{relativePath}' is too deep.");
                }
                result.Pop();
                continue;
            }
            result.Enqueue(segment);
        }

        return result;
    }

    public static string EscapeReservesChars(string? s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        StringBuilder builder = new();
        for (int i = 0; i < s!.Length; i++)
        {
            var c = s[i];
            if (c == '\\' || c == '/' || c == ':' || c == '=')
            {
                builder.Append('\\');
            }
            builder.Append(c);
        }
        return builder.ToString();
    }

    public static string EncodeSegment(string? segmentName, NameValueCollection parameters, bool endsWithSlash = true)
    {
        StringBuilder builder = new(EscapeReservesChars(segmentName));
        foreach (var key in parameters.AllKeys)
        {
            foreach (var value in parameters.GetValues(key))
            {
                builder.Append(":");
                builder.Append(EscapeReservesChars(key));
                builder.Append("=");
                builder.Append(EscapeReservesChars(value));
            }
        }
        if (endsWithSlash)
        {
            builder.Append("/");
        }
        return builder.ToString();
    }

    public static string ToStringAddress(this QueueStack<RouteSegment> segments)
    {
        StringBuilder builder = new();
        while (segments.TryDequeue(out var routeSegment))
        {
            if (routeSegment is null)
            {
                continue;
            }
            builder.Append(EncodeSegment(routeSegment.SegmentName, routeSegment.Parameters));
        }
        return builder.ToString();
    }
}