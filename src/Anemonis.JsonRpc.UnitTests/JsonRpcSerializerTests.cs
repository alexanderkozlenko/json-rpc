using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Anemonis.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed partial class JsonRpcSerializerTests
    {
        [DebuggerStepThrough]
        internal static void CompareJsonStrings(string expected, string actual)
        {
            Assert.IsTrue(JToken.DeepEquals(JToken.Parse(expected), JToken.Parse(actual)), "Actual JSON string differs from expected");
        }

        [TestMethod]
        public void SerializeRequestWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequest(null));
        }

        [TestMethod]
        public void SerializeRequestsWhenCollectionIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequests(null));
        }

        [TestMethod]
        public void SerializeRequestsWhenCollectionIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.SerializeRequests(new JsonRpcRequest[] { }));

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void SerializeRequestsWhenCollectionContainsNull()
        {
            var jrs = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.SerializeRequests(new JsonRpcRequest[] { null }));

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void SerializeRequestWithStreamWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequest(null, stream));
        }

        [TestMethod]
        public void SerializeRequestWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(0L, "m");

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequest(jrm, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeRequestAsyncWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestAsync(null, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeRequestAsyncWithStreamWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestAsync(null, stream, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeRequestAsyncWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(0L, "m");

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestAsync(jrm, (Stream)null, default).AsTask());
        }

        [TestMethod]
        public void SerializeRequestsWithStreamWhenRequestsIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequests(null, stream));
        }

        [TestMethod]
        public void SerializeRequestsWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcRequest(0L, "m");
            var jrm2 = new JsonRpcRequest(1L, "m");
            var jrms = new[] { jrm1, jrm2 };

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequests(jrms, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeRequestsAsyncWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestsAsync(null, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeRequestsAsyncWithStreamWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestsAsync(null, stream, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeRequestsAsyncWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcRequest(0L, "m");
            var jrm2 = new JsonRpcRequest(1L, "m");
            var jrms = new[] { jrm1, jrm2 };

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestsAsync(jrms, (Stream)null, default).AsTask());
        }

        [TestMethod]
        public void SerializeResponseWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponse(null));
        }

        [TestMethod]
        public void SerializeResponsesWhenCollectionIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponses(null));
        }

        [TestMethod]
        public void SerializeResponsesWhenCollectionIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.SerializeResponses(new JsonRpcResponse[] { }));

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void SerializeResponsesWhenCollectionContainsNull()
        {
            var jrs = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.SerializeResponses(new JsonRpcResponse[] { null }));

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void SerializeResponseWithStreamWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponse(null, stream));
        }

        [TestMethod]
        public void SerializeResponseWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(0L, 0L);

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponse(jrm, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeResponseAsyncWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponseAsync(null, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeResponseAsyncWithStreamWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponseAsync(null, stream, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeResponseAsyncWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(0L, 0L);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponseAsync(jrm, (Stream)null, default).AsTask());
        }

        [TestMethod]
        public void SerializeResponsesWithStreamWhenResponsesIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponses(null, stream));
        }

        [TestMethod]
        public void SerializeResponsesWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcResponse(0L, 0L);
            var jrm2 = new JsonRpcResponse(1L, 0L);
            var jrms = new[] { jrm1, jrm2 };

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponses(jrms, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeResponsesAsyncWhenResponsesIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponsesAsync(null, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeResponsesAsyncWithStreamWhenResponsesIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using var stream = new MemoryStream();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponsesAsync(null, stream, default).AsTask());
        }

        [TestMethod]
        public async Task SerializeResponsesAsyncWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcResponse(0L, 0L);
            var jrm2 = new JsonRpcResponse(1L, 0L);
            var jrms = new[] { jrm1, jrm2 };

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponsesAsync(jrms, (Stream)null, default).AsTask());
        }

        [TestMethod]
        public void DeserializeRequestDataWithStringWhenStringIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeRequestData((string)null));
        }

        [TestMethod]
        public void DeserializeRequestDataWithStringWhenStringIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(string.Empty));
        }

        [TestMethod]
        public void DeserializeRequestDataWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeRequestData((Stream)null));
        }

        [TestMethod]
        public void DeserializeRequestDataWithStreamWhenStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(Stream.Null));
        }

        [TestMethod]
        public async Task DeserializeRequestDataAsyncWithStringWhenStringIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeRequestDataAsync((string)null, default).AsTask());
        }

        [TestMethod]
        public async Task DeserializeRequestDataAsyncWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeRequestDataAsync((Stream)null, default).AsTask());
        }

        [TestMethod]
        public async Task DeserializeRequestDataAsyncWithStreamWhenStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                jrs.DeserializeRequestDataAsync(Stream.Null, default).AsTask());
        }

        [TestMethod]
        public void DeserializeRequestDataWithStringWhenDataIsBatchAndCountIsZero()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.tc_ibl_req_b1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.DeserializeRequestData(jsont));

            Assert.AreEqual(default, exception.MessageId);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataWithStringWhenStringIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeResponseData((string)null));
        }

        [TestMethod]
        public void DeserializeResponseDataWithStringWhenStringIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeResponseData(string.Empty));
        }

        [TestMethod]
        public void DeserializeResponseDataWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeResponseData((Stream)null));
        }

        [TestMethod]
        public void DeserializeResponseDataWithStreamWhenStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeResponseData(Stream.Null));
        }

        [TestMethod]
        public async Task DeserializeResponseDataAsyncWithStringWhenStringIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeResponseDataAsync((string)null, default).AsTask());
        }

        [TestMethod]
        public async Task DeserializeResponseDataAsyncWithStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeResponseDataAsync((Stream)null, default).AsTask());
        }

        [TestMethod]
        public async Task DeserializeResponseDataAsyncWithStreamWhenStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                jrs.DeserializeResponseDataAsync(Stream.Null, default).AsTask());
        }

        [TestMethod]
        public void DeserializeResponseDataWithStringWhenDataIsBatchAndCountIsZero()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.tc_ibl_res_b1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.DeserializeResponseData(jsont));

            Assert.AreEqual(default, exception.MessageId);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataWithStringWhenContractIsNotFoundTCB0I1E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i1e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNull(jrd.Item.Message);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual("1", jre.MessageId);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMethod, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataWithStringWhenContractIsNotFoundTCB0I1E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i1e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNull(jrd.Item.Message);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual("1", jre.MessageId);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMethod, jre.ErrorCode);
        }
    }
}
