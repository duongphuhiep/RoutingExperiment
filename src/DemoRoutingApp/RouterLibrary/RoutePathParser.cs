using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace DemoRoutingApp.Models;

public record RouteSegment
{
    public string? Segment { get; set; }
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

    public static Queue<RouteSegment> Parse(string routePath)
    {
        if (!routePath.EndsWith('/'))
        {
            routePath += '/';
        }
        int cursor = 0;
        var result = new Queue<RouteSegment>();
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
                        throw new InvalidRouteException($"Unexpected '/' at postion {cursor}. Key: {currentKey} has no value.");
                    }
                    else if (currentTokenType == TokenType.Value)
                    {
                        currentRouteSegment.Parameters.Add(currentKey.ToString(), currentValue.ToString());
                    }
                    else // currentTokenType == TokenType.Segment
                    {
                        currentRouteSegment.Segment = currentSegment.ToString();
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
                        currentRouteSegment.Segment = currentSegment.ToString();
                    }
                    else // currentTokenType == TokenType.Key
                    {
                        throw new InvalidRouteException($"Unexpected ':' at postion {cursor}. Key: {currentKey} has no value.");
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
                        throw new InvalidRouteException($"Unexpected '=' at postion {cursor}. Missing key for the value after '{currentValue}'.");
                    }
                    else if (currentTokenType == TokenType.Segment)
                    {
                        throw new InvalidRouteException($"Unexpected '=' at postion {cursor}. Cannot assign value to a segment '{currentSegment}'");
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
}