using System.Collections.Generic;

//공동환 교수님꼐서 친히 하사해 주신 코드 건들지 말것!!(중요)
public class Dequeue<T>
{
    private LinkedList<T> _buf;

    public int Count { get { return _buf.Count; } }

    public Dequeue()
    {
        _buf = new LinkedList<T>();
    }

    public void PushFront(T item)
    {
        _buf.AddFirst(item);
    }

    public void PushBack(T item)
    {
        _buf.AddLast(item);
    }

    public T PopFront()
    {
        if (_buf.Count == 0)
            throw new System.Exception("원소의 개수가 0임.");
        T first = _buf.First.Value;

        _buf.RemoveFirst();

        return first;
    }

    public T PopBack()
    {
        if (_buf.Count == 0)
            throw new System.Exception("원소의 개수가 0임.");
        T last = _buf.Last.Value;

        _buf.RemoveLast();

        return last;
    }

    public void Clear() { _buf.Clear(); } //이건 내가 추가한 코드

}