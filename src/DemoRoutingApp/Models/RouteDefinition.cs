using CommunityToolkit.Mvvm.ComponentModel;
using DemoRoutingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace DemoRoutingApp.Models;

public record RouteDefinition
{
    public virtual string Path { get; set; } = string.Empty;
    public virtual Type Component { get; set; } = typeof(NotFoundViewModel);
    public List<RouteDefinition> Children { get; init; } = new List<RouteDefinition>();
}
public record RouteDefinition<TRoutableViewModel> : RouteDefinition where TRoutableViewModel : ViewModelBase, IRoutable
{
    public override Type Component { get; set; } = typeof(TRoutableViewModel);
}
public record RootRouteDefinition<TRoutableViewModel> : RouteDefinition<TRoutableViewModel> where TRoutableViewModel : ViewModelBase, IRoutable
{
    public override string Path { get; set; } = "/";
}

public partial class RouteData : ObservableObject
{
    public string FullPath { get; init; } = string.Empty;
    public string CurrentPathSegment { get; init; } = string.Empty;
    public NameValueCollection QueryString { get; init; } = [];
    public RouteDefinition Definition { get; init; } = null!;
    public RouteData? Parent { get; init; }

    [ObservableProperty]
    private RouteData? _selectedChild;
    public Type? Component => Definition.Component;
}

public static class RouteParser
{
    const string RootPath = "/";
    const string Separator = "/";
    public static Queue<RouteData> Parse(string path, RouteDefinition root)
    {
        if (path?.StartsWith(RootPath) != true)
        {
            throw new ArgumentException($"Path must to be absolute, it has to start with '{RootPath}'", nameof(path));
        }
        if (root.Path != RootPath)
        {
            throw new ArgumentException($"Not a root definition. Root's path must to be '{RootPath}'", nameof(root));
        }
        Uri uri = new Uri("route:" + path);
        NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);

        Queue<RouteData> result = new Queue<RouteData>();
        RouteData parentRouteData = new RouteData
        {
            CurrentPathSegment = RootPath,
            Definition = root,
            FullPath = path,
            QueryString = queryString
        };
        result.Enqueue(parentRouteData);

        var currentNode = root;
        for (int i = 1; i < uri.Segments.Length; i++) // next node
        {
            string rawSegment = uri.Segments[i];
            var segment = CleanSegment(rawSegment);
            if (string.IsNullOrEmpty(segment))
            {
                throw new InvalidRouteException($"Detected empty segment (at index {i}) in the path: '{path}'");
            }

            //find route definition correspond to this segment
            var child = currentNode.Children.Find(
                x => string.Equals(x.Path, segment, StringComparison.InvariantCultureIgnoreCase)
                    || x.Path.StartsWith(':'));
            if (child is null)
            {
                throw new RouteNotFoundException($"Route's definition not found for the Segment '{segment}' (at index {i}) in the path: '{path}'");
            }
            var currentRouteData = new RouteData
            {
                Parent = parentRouteData,
                CurrentPathSegment = segment,
                Definition = currentNode,
                FullPath = path,
                QueryString = queryString
            };
            result.Enqueue(currentRouteData);
            //trigger view changes! animation
            parentRouteData.SelectedChild = currentRouteData;
            parentRouteData = currentRouteData;
        }
        return result;
    }

    private static string CleanSegment(string rawSegment)
    {
        if (string.IsNullOrEmpty(rawSegment))
        {
            return string.Empty;
        }
        if (rawSegment == RootPath) { return RootPath; }
        var segment = rawSegment.Replace(Separator, string.Empty);
        return segment.Trim();
    }
}

public class InvalidRouteException : Exception
{
    public InvalidRouteException(string message) : base(message) { }
}
public class RouteNotFoundException : Exception
{
    public RouteNotFoundException(string message) : base(message) { }
}