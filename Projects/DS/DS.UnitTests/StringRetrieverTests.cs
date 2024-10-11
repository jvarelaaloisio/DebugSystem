using System;
using System.Linq;
using DS.DataRetriever;
using NUnit.Framework;

namespace DS.UnitTests
{
	public class StringRetrieverTests
	{
		private const string Data1 = "data1";
		private const string Data2 = "data2";
		private IRetriever<string> _retriever;

		private readonly Func<string> _method1 = () => Data1;
		private readonly Func<string> _method2 = () => Data2;

		[SetUp]
		public void SetUp()
		{
			_retriever = new StringRetriever();
		}

		#region AddSource

		[Test]
		public void AddSource_GivenMethod_AddsMethod()
		{
			GivenMethod1();
			Assert.IsNotNull(_retriever.FetchSources);
			Assert.IsNotEmpty(_retriever.FetchSources.GetInvocationList());
			Assert.IsTrue(_retriever.FetchSources.GetInvocationList().Contains(_method1));
		}

		#endregion

		#region RemoveSource

		[Test]
		public void RemoveSource_GivenMethod_RemovesMethod()
		{
			GivenMethod1();
			_retriever.RemoveSource(_method1);
			Assert.IsNull(_retriever.FetchSources);
		}

		#endregion

		#region TryRetrieveData

		[Test]
		public void TryRetrieveData_GivenNoMethod_ReturnsFalse()
		{
			Assert.IsFalse(_retriever.TryFetch(out _));
		}

		[Test]
		public void TryRetrieveData_GivenMethod_ReturnsTrue()
		{
			GivenMethod1();
			Assert.IsTrue(_retriever.TryFetch(out _));
		}

		[Test]
		public void TryRetrieveData_GivenTwoMethods_RetrievesData()
		{
			GivenMethod1();
			GivenMethod2();
			_retriever.TryFetch(out string retrievedData);
			Assert.AreEqual($"{Data1}{Data2}", retrievedData);
		}

		[Test]
		public void TryRetrieveData_GivenFormat_RetrievesFormattedData()
		{
			_retriever = new StringRetriever((str) => str + "\n");
			GivenMethod1();
			GivenMethod2();
			_retriever.TryFetch(out var fetchedData);
			Assert.AreEqual($"{Data1}\n{Data2}\n", fetchedData);
		}

		#endregion

		#region Given

		private void GivenMethod1()
		{
			_retriever.AddSource(_method1);
		}
		
		private void GivenMethod2()
		{
			_retriever.AddSource(_method2);
		}

		#endregion
	}
}