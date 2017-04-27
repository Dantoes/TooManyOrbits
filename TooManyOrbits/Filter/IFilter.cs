namespace TooManyOrbits.Filter
{
	internal interface IFilter<in T>
	{
		bool Accect(T obj);
	}
}
