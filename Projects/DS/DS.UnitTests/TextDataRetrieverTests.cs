using System;
using System.Linq;
using DS.DataRetriever;
using NUnit.Framework;

namespace DS.UnitTests
{
	public class TextDataRetrieverTests
	{
		private const string DATA1 = "data1";
		private const string DATA2 = "data2";
		private IDataRetriever<string> _dataRetriever;

		private readonly Func<string> _method1 = () => DATA1;
		private readonly Func<string> _method2 = () => DATA2;

		[SetUp]
		public void SetUp()
		{
			_dataRetriever = new TextDataRetriever();
		}

		#region AddDataSource

		[Test]
		public void AddDataSource_GivenMethod_AddsMethod()
		{
			GivenMethod1();
			Assert.IsNotNull(_dataRetriever.OnRetrieveData);
			Assert.IsNotEmpty(_dataRetriever.OnRetrieveData.GetInvocationList());
			Assert.IsTrue(_dataRetriever.OnRetrieveData.GetInvocationList().Contains(_method1));
		}

		#endregion

		#region RemoveDataSource

		[Test]
		public void RemoveDataSource_GivenMethod_RemovesMethod()
		{
			GivenMethod1();
			_dataRetriever.RemoveDataSource(_method1);
			Assert.IsNull(_dataRetriever.OnRetrieveData);
		}

		#endregion

		#region TryRetrieveData

		[Test]
		public void TryRetrieveData_GivenNoMethod_ReturnsFalse()
		{
			Assert.IsFalse(_dataRetriever.TryRetrieveData(out _));
		}

		[Test]
		public void TryRetrieveData_GivenMethod_ReturnsTrue()
		{
			GivenMethod1();
			Assert.IsTrue(_dataRetriever.TryRetrieveData(out _));
		}

		[Test]
		public void TryRetrieveData_GivenTwoMethods_RetrievesData()
		{
			GivenMethod1();
			GivenMethod2();
			_dataRetriever.TryRetrieveData(out string retrievedData);
			Assert.AreEqual($"{DATA1}{DATA2}", retrievedData);
		}

		[Test]
		public void TryRetrieveData_GivenModifier_RetrievesDataModified()
		{
			_dataRetriever = new TextDataRetriever((str) => str + "\n");
			GivenMethod1();
			GivenMethod2();
			_dataRetriever.TryRetrieveData(out string retrievedData);
			Assert.AreEqual($"{DATA1}\n{DATA2}\n", retrievedData);
		}

		#endregion

		#region Given

		private void GivenMethod1()
		{
			_dataRetriever.AddDataSource(_method1);
		}
		private void GivenMethod2()
		{
			_dataRetriever.AddDataSource(_method2);
		}

		#endregion
	}
}