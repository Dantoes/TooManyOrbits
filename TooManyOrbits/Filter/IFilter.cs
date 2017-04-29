namespace TooManyOrbits.Filter
{
	internal interface IFilter<in T>
	{
		bool Accept(T obj);
	}
}
