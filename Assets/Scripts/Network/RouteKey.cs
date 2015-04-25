/// <summary>
/// Tuple.
/// </summary>
public class Tuple<T1, T2> 
{
	public readonly T1 Item1;
	public readonly T2 Item2;
	public Tuple(T1 item1, T2 item2) 
	{ 
		this.Item1 = item1; this.Item2 = item2;
	}
}

public static class Tuple
{ 
	// for type-inference goodness.
	public static Tuple<T1,T2> Create<T1,T2>(T1 item1, T2 item2) 
	{ 
/*		Tuple newTuple;
		newTuple.*/
		return new Tuple<T1,T2>(item1, item2); 
	}
}

public class RouteKey : Tuple<int, int>
{
	public RouteKey(int originID, int destinationID)
		: base(originID, destinationID)
	{
	}
}