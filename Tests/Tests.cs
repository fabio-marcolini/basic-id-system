using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Test()
        {
            var test = new Dictionary<string, string>();
            string aa = test["aa"];
        }
    }
}
