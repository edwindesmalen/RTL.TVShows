namespace RTL.TVShows.Extensions.Interfaces
{
	public interface IMapper<in TIn, out TOut>
    {
        /// <summary>
        /// Maps data from an object to another object.
        /// </summary>
        /// <param name="instance">The input object</param>
        /// <returns>The output object</returns>
        TOut Map(TIn instance);
    }
}
