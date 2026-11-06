namespace ClinicApp.Models;

public class WaitingQueue<T>
{
    private Queue<T> _items = new Queue<T>();

    public int Count
    {
        get
        {
            return _items.Count;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return _items.Count == 0;
        }
    }

    public void Enqueue(T item)
    {
        _items.Enqueue(item);
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Черга порожня. Немає кого приймати.");
        }

        return _items.Dequeue();
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Черга порожня. Немає першого елемента.");
        }

        return _items.Peek();
    }

    public T[] ToArray()
    {
        return _items.ToArray();
    }
}