using System;
using System.Linq;
using DS.DataRetriever;
using NUnit.Framework;

namespace DS.UnitTests
{
	public class TextDataRetrieverTests
	{
		private struct SourceHelper
		{
			public string GetData()
			{
				return "data";
			}
		}
	
		[Test]
		public void AddDataSource_OneDataSource_ContainsSource()
		{
			IDataRetriever<string> dataRetriever = new TextDataRetriever();
			SourceHelper helper = new SourceHelper();
			Func<string> getData = helper.GetData;
			dataRetriever.AddDataSource(getData);
			Assert.IsNotNull(dataRetriever.OnRetrieveData);
			Assert.IsNotEmpty(dataRetriever.OnRetrieveData.GetInvocationList());
			Assert.IsTrue(dataRetriever.OnRetrieveData.GetInvocationList().Contains(getData));
		}
		
		[Test]
		public void RemoveDataSource_DataSourceRemoved_DoesNotContainSource()
		{
			SourceHelper helper = new SourceHelper();
			Func<string> getData = helper.GetData;
			IDataRetriever<string> dataRetriever = new TextDataRetriever();
			dataRetriever.AddDataSource(helper.GetData);
			dataRetriever.RemoveDataSource(helper.GetData);
			Assert.IsFalse(dataRetriever.OnRetrieveData.GetInvocationList().Contains(getData));
		}

		[Test]
		public void TryRetrieveData_NoDelegateAdded_ReturnsFalse()
		{
			IDataRetriever<string> dataRetriever = new TextDataRetriever();
			Assert.IsFalse(dataRetriever.TryRetrieveData(out _));
		}

		[Test]
		public void TryRetrieveData_OneDelegateAdded_ReturnsTrue()
		{
			IDataRetriever<string> dataRetriever = new TextDataRetriever();
			dataRetriever.AddDataSource(() => "data");
			Assert.IsTrue(dataRetriever.TryRetrieveData(out _));
		}

		[Test]
		public void TryRetrieveData_TwoDelegatesAdded_RetrievesData()
		{
			const string data1 = "data1";
			const string data2 = "data2";
			IDataRetriever<string> dataRetriever = new TextDataRetriever();
			dataRetriever.AddDataSource(() => data1);
			dataRetriever.AddDataSource(() => data2);
			dataRetriever.TryRetrieveData(out string retrievedData);
			Assert.AreEqual($"{data1}{data2}", retrievedData);
		}

		[Test]
		public void TryRetrieveData_TwoDelegatesWithModifier_RetrievesDataModified()
		{
			const string data1 = "data1";
			const string data2 = "data2";
			IDataRetriever<string> dataRetriever = new TextDataRetriever((str) => str + "\n");
			dataRetriever.AddDataSource(() => data1);
			dataRetriever.AddDataSource(() => data2);
			dataRetriever.TryRetrieveData(out string retrievedData);
			Assert.AreEqual($"{data1}\n{data2}\n", retrievedData);
		}
	}
}