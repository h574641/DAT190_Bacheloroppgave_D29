using System;

//Circular queue
public class PeekAQueue<T>
{
    public int Length;
    public int Count;
    public int current;

    private T[] data;

    public PeekAQueue(int length)
    {
        Length = length;
        data = new T[length];
    }

    // Add element to queue
    public void Enqueue(T item)
    {
        current = wrapIndex(current + 1);
        data[current] = item;
        Count = Math.Min(Count + 1, Length);
    }

    // Remove element from queue
    public T Dequeue()
    {
        T res = default(T);

        if (Count > 0)
        {
            res = data[current];
            current = wrapIndex(current - 1);
            Count--;
        }

        return res;
    }

    // Check previous added items offset amount of items back
    public T Peek(int offset = 0)
    {
        T res = default(T);

        if (Count > offset)
        {
            res = data[wrapIndex(current - offset)];
        }

        return res;
    }

    // Proper modulo operator
    // Thanks .NET
    private int mod(int n, int m)
    {
        return ((n % m) + m) % m;
    }

    // Calculate circular index
    private int wrapIndex(int index)
    {
        return (mod(index, Length));
    }
}

