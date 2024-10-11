using System;
using System.Linq;

namespace DS.DataRetriever
{
	public class StringRetriever : IRetriever<string>
	{
		private readonly Func<string, string> _formatData;
		public Func<string> FetchSources { get; private set; }

		public StringRetriever(Func<string, string> formatData = null)
		{
			if (formatData != null)
				_formatData = formatData;
		}

		public bool TryFetch(out string data)
		{
			data = string.Empty;
			if (FetchSources is null)
				return false;
			string fetchedData = string.Empty;
			foreach (var getData in FetchSources.GetInvocationList())
			{
				fetchedData += getData.DynamicInvoke();
				if (_formatData != null)
					fetchedData = _formatData(fetchedData);
			}

			data = fetchedData;
			return true;
		}

		public void AddSource(Func<string> getData)
		{
			if (FetchSources != null && FetchSources.GetInvocationList().Contains(getData))
				return;
			FetchSources += getData;
		}

		public void RemoveSource(Func<string> getData)
			=> FetchSources -= getData;
	}
}