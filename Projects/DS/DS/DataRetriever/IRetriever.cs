using System;

namespace DS.DataRetriever
{
	public interface IRetriever<T>
	{
		/// <summary>
		/// Sources to fetch data from
		/// </summary>
		Func<T> FetchSources { get; }
		/// <summary>
		/// Tries to fetch data from sources
		/// </summary>
		/// <param name="data">The fetched data</param>
		/// <returns>True if successful, false if Fetch sources returns an error or is null</returns>
		bool TryFetch(out T data);
		/// <summary>
		/// Adds a source to fetch sources
		/// </summary>
		/// <param name="getData">The source to add</param>
		void AddSource(Func<T> getData);
		/// <summary>
		/// Removes a source from fetch sources
		/// </summary>
		/// <param name="getData">The source to remove</param>
		void RemoveSource(Func<T> getData);
	}
}