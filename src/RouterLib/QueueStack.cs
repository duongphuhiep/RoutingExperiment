using System;
using System.Collections.Generic;

namespace Starfruit.RouterLib;

public class QueueStack<T>
{
    private readonly LinkedList<T> _list = new LinkedList<T>();

    /// <summary>
    /// Enqueue (FIFO) AddLast()
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(T item) => _list.AddLast(item);

    /// <summary>
    /// Dequeue (FIFO) RemoveFirst()
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryDequeue(out T? value)
    {
        if (_list.Count == 0)
        {
            value = default;
            return false;
        }
        value = _list.First.Value;
        _list.RemoveFirst();
        return true;
    }

    /// <summary>
    /// Dequeue (FIFO) RemoveFirst()
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T Dequeue()
    {
        if (!TryDequeue(out T? value))
        {
            throw new InvalidOperationException("The collection is empty.");
        }
        return value!;
    }

    /// <summary>
    /// Push (LIFO) AddLast()
    /// </summary>
    /// <param name="item"></param>
    public void Push(T item) => Enqueue(item);

    /// <summary>
    /// Pop (LIFO) RemoveLast()
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryPop(out T? value)
    {
        if (_list.Count == 0)
        {
            value = default;
            return false;
        }

        value = _list.Last.Value;
        _list.RemoveLast();
        return true;
    }

    /// <summary>
    /// Pop (LIFO) RemoveLast()
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T Pop()
    {
        if (!TryPop(out T? value))
        {
            throw new InvalidOperationException("The collection is empty.");
        }
        return value!;
    }

    public void Prepend(T item) => _list.AddFirst(item);

    public int Count => _list.Count;
}
