using System.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace Anemonis.JsonRpc.ConformanceTests
{
    internal static class JsonRpcSerializerTests
    {
        [DebuggerStepThrough]
        internal static void CompareJsonStrings(string expected, string actual)
        {
            Assert.IsTrue(JToken.DeepEquals(JToken.Parse(expected), JToken.Parse(actual)), "Actual JSON string differs from expected");
        }
    }
}
