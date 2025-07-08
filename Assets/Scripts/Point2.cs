public struct Point2
{
	public int x;

	public int y;

	public Point2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj)
	{
		return obj is Point2 && this == (Point2)obj;
	}

	public override int GetHashCode()
	{
		return x.GetHashCode() ^ y.GetHashCode();
	}

	public static bool operator ==(Point2 x, Point2 y)
	{
		return x.x == y.x && x.y == y.y;
	}

	public static bool operator !=(Point2 x, Point2 y)
	{
		return !(x == y);
	}
}
