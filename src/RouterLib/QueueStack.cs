using System;
using System.Collections.Generic;

namespace Starfruit.RouterLib;

public class QueueStack<T> : LinkedList<T>
{
    /// <summary>
    /// Enqueue (FIFO) AddLast()
    /// </summary>
    /// <param name="item"></param>
    public void Enqueue(T item) => AddLast(item);

    /// <summary>
    /// Dequeue (FIFO) RemoveFirst()
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryDequeue(out T? value)
    {
        if (Count == 0)
        {
            value = default;
            return false;
        }
        value = First.Value;
        RemoveFirst();
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
        if (Count == 0)
        {
            value = default;
            return false;
        }

        value = Last.Value;
        RemoveLast();
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

    public void Prepend(T item) => AddFirst(item);
}
