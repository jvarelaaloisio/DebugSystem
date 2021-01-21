using System;

namespace DS.DataRetriever
{
	public interface IDataRetriever<T>
	{
		Func<T> OnRetrieveData { get; }
		bool TryRetrieveData(out T data);
		void AddDataSource(Func<T> getData);
		void RemoveDataSource(Func<T> getData);
	}
}