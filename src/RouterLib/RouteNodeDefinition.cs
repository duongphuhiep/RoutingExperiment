using System;
using System.Collections.Generic;

namespace Starfruit.RouterLib;

public class RouteNodeDefinition
{
    public override string ToString()
    {
        return $"{typeof(RouteNodeDefinition)}({PathSegment}<{ComponentType.FullName}>)";
    }
    public virtual string PathSegment { get; internal set; } = string.Empty;
    public virtual Type ComponentType { get; internal set; }

    public RouteNodeDefinition? Parent { get; private set; }

    private readonly Dictionary<string, RouteNodeDefinition> _children = new();
    public IEnumerable<RouteNodeDefinition> Children => _children.Values;

    /// <summary>
    /// Cache all the components instance correspond to this route node. 
    /// </summary>
    public IEnumerable<WeakReference<IRoutableViewModel>> Components => _components;

    private readonly List<WeakReference<IRoutableViewModel>> _components = new();

    public bool IsRoot => Parent is null;

    public void SetParent(RouteNodeDefinition? parent)
    {
        if (parent is null)
        {
            return;
        }
        parent.AddChild(this);
    }

    public void AddChild(RouteNodeDefinition child)
    {
        if (child.Parent is not null)
        {
            throw new InvalidOperationException($"Child '{child.PathSegment}' already has a parent '{child.Parent.PathSegment}'");
        }
        if (_children.ContainsKey(child.PathSegment))
        {
            throw new InvalidOperationException($"Duplicate child path segment: '{child.PathSegment}' in '{PathSegment}'");
        }
        _children[child.PathSegment] = child;
        child.Parent = this;
    }

    public RouteNodeDefinition this[string pathSegment] => _children[pathSegment];
    public bool HasChild(string pathSegment) => _children.ContainsKey(pathSegment);
    internal void RegisterComponent(IRoutableViewModel component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        if (!ComponentType.IsAssignableFrom(component.GetType()))
        {
            throw new InvalidCastException($"Component type mismatch route type. Unable to register the component of type {component.GetType()}) to the {ToString()}.");
        }
        _components.Add(new WeakReference<IRoutableViewModel>(component));
        component.RouteDefinition = this;
    }

    public void CleanDeadComponents() => _components.RemoveAll(x => !x.TryGetTarget(out _));
}

/// <summary>
/// For sugar syntax
/// </summary>
/// <typeparam name="T"></typeparam>
public class RouteNodeDefinition<T> : RouteNodeDefinition where T : IRoutableViewModel
{
    public RouteNodeDefinition(string pathSegment)
    {
        PathSegment = pathSegment;
        ComponentType = typeof(T);
    }

    public RouteNodeDefinition(string pathSegment, IEnumerable<RouteNodeDefinition> children) : this(pathSegment)
    {
        foreach (var child in children)
        {
            AddChild(child);
        }
    }
}