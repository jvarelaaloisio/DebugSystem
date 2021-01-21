using System;
using System.Linq;

namespace DS.DataRetriever
{
	public class TextDataRetriever : IDataRetriever<string>
	{
		private readonly Func<string, string> _modifyData;
		public Func<string> OnRetrieveData { get; private set; }

		public TextDataRetriever(Func<string, string> modifyData = null)
		{
			if (modifyData != null)
				_modifyData = modifyData;
		}

		public bool TryRetrieveData(out string data)
		{
			data = string.Empty;
			if (OnRetrieveData is null)
				return false;
			string retrievedData = string.Empty;
			foreach (var getData in OnRetrieveData.GetInvocationList())
			{
				retrievedData += getData.DynamicInvoke(null);
				if (_modifyData != null)
					retrievedData = _modifyData(retrievedData);
			}

			data = retrievedData;
			return true;
		}

		public void AddDataSource(Func<string> getData)
		{
			if (!(OnRetrieveData is null) && OnRetrieveData.GetInvocationList().Contains(getData))
				return;
			OnRetrieveData += getData;
		}

		public void RemoveDataSource(Func<string> getData)
		{
			OnRetrieveData -= getData;
		}
	}
}