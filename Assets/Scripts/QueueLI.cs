using System.Collections.Generic;

public class QueueLI<T> : Queue<T>
{
	public bool Untouched = true;

	public T Last
	{
		get;
		private set;
	}

	public new void Enqueue(T item)
	{
		Last = item;
		base.Enqueue(item);
	}

	public new T Dequeue()
	{
		Untouched = false;
		return base.Dequeue();
	}
}
