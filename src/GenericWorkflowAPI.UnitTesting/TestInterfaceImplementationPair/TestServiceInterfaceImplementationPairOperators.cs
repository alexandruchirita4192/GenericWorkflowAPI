using System.Collections.Generic;
using GenericWorkflowAPI.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericWorkflowAPI.UnitTesting
{
    [TestClass]
    public class TestServiceInterfaceImplementationPairOperators
    {
        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_Equals_EqualObjects()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsTrue(mapper1.Equals(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_Equals_NotEqualObjects1()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1.Equals(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_Equals_NotEqualObjects2()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1.Equals(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_Equals_NotEqualObjects3()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));

            Assert.IsFalse(mapper1.Equals(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_Equals_NotEqualObjects4()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));

            Assert.IsFalse(mapper1.Equals(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_Equals_NotEqualObjects5()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(int));

            Assert.IsFalse(mapper1.Equals(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_EqualsOperator_EqualObjects()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsTrue(mapper1 == mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_EqualsOperator_NotEqualObjects1()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1 == mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_EqualsOperator_NotEqualObjects2()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1 == mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_EqualsOperator_NotEqualObjects3()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));

            Assert.IsFalse(mapper1 == mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_EqualsOperator_NotEqualObjects4()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));

            Assert.IsFalse(mapper1 == mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_EqualsOperator_NotEqualObjects5()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(int));

            Assert.IsFalse(mapper1 == mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_NotEqualsOperator_EqualObjects()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1 != mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_NotEqualsOperator_NotEqualObjects1()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsTrue(mapper1 != mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_NotEqualsOperator_NotEqualObjects2()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsTrue(mapper1 != mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_NotEqualsOperator_NotEqualObjects3()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));

            Assert.IsTrue(mapper1 != mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_NotEqualsOperator_NotEqualObjects4()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));

            Assert.IsTrue(mapper1 != mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_NotEqualsOperator_NotEqualObjects5()
        {
            var mapper1 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(int));

            Assert.IsTrue(mapper1 != mapper2);
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_List_Contains_EqualObjects()
        {
            var mapper1 = new List<ServiceInterfaceImplementationPair> { new ServiceInterfaceImplementationPair(typeof(string), typeof(string)) };
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsTrue(mapper1.Contains(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_List_Contains_NotEqualObjects1()
        {
            var mapper1 = new List<ServiceInterfaceImplementationPair> { new ServiceInterfaceImplementationPair(typeof(string), typeof(int)) };
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1.Contains(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_List_Contains_NotEqualObjects2()
        {
            var mapper1 = new List<ServiceInterfaceImplementationPair> { new ServiceInterfaceImplementationPair(typeof(int), typeof(string)) };
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(string));

            Assert.IsFalse(mapper1.Contains(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_List_Contains_NotEqualObjects3()
        {
            var mapper1 = new List<ServiceInterfaceImplementationPair> { new ServiceInterfaceImplementationPair(typeof(string), typeof(string)) };
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(string), typeof(int));

            Assert.IsFalse(mapper1.Contains(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_List_Contains_NotEqualObjects4()
        {
            var mapper1 = new List<ServiceInterfaceImplementationPair> { new ServiceInterfaceImplementationPair(typeof(string), typeof(string)) };
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(string));

            Assert.IsFalse(mapper1.Contains(mapper2));
        }

        [TestMethod]
        public void Test_ServiceInterfaceImplementationPair_List_Contains_NotEqualObjects5()
        {
            var mapper1 = new List<ServiceInterfaceImplementationPair> { new ServiceInterfaceImplementationPair(typeof(string), typeof(string)) };
            var mapper2 = new ServiceInterfaceImplementationPair(typeof(int), typeof(int));

            Assert.IsFalse(mapper1.Contains(mapper2));
        }
    }
}