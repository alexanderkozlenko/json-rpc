using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed partial class JsonRpcSerializerTests
    {
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
        public void SerializeRequestToStreamWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jrs.SerializeRequest(null, stream));
            }
        }

        [TestMethod]
        public void SerializeRequestToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(0L, "m");

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequest(jrm, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeRequestAsyncToStringWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestAsync(null).AsTask());
        }

        [TestMethod]
        public async Task SerializeRequestAsyncToStreamWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jrs.SerializeRequestAsync(null, stream).AsTask());
            }
        }

        [TestMethod]
        public async Task SerializeRequestAsyncToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(0L, "m");

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestAsync(jrm, (Stream)null).AsTask());
        }

        [TestMethod]
        public void SerializeRequestsToStreamWhenRequestsIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jrs.SerializeRequests(null, stream));
            }
        }

        [TestMethod]
        public void SerializeRequestsToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcRequest(0L, "m");
            var jrm2 = new JsonRpcRequest(1L, "m");
            var jrms = new[] { jrm1, jrm2 };

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeRequests(jrms, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeRequestsAsyncToStringWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestsAsync(null).AsTask());
        }

        [TestMethod]
        public async Task SerializeRequestsAsyncToStreamWhenRequestIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jrs.SerializeRequestsAsync(null, stream).AsTask());
            }
        }

        [TestMethod]
        public async Task SerializeRequestsAsyncToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcRequest(0L, "m");
            var jrm2 = new JsonRpcRequest(1L, "m");
            var jrms = new[] { jrm1, jrm2 };

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeRequestsAsync(jrms, (Stream)null).AsTask());
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
        public void SerializeResponseToStreamWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jrs.SerializeResponse(null, stream));
            }
        }

        [TestMethod]
        public void SerializeResponseToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(0L, 0L);

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponse(jrm, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeResponseAsyncToStringWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponseAsync(null).AsTask());
        }

        [TestMethod]
        public async Task SerializeResponseAsyncToStreamWhenResponseIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jrs.SerializeResponseAsync(null, stream).AsTask());
            }
        }

        [TestMethod]
        public async Task SerializeResponseAsyncToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(0L, 0L);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponseAsync(jrm, (Stream)null).AsTask());
        }

        [TestMethod]
        public void SerializeResponsesToStreamWhenResponsesIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jrs.SerializeResponses(null, stream));
            }
        }

        [TestMethod]
        public void SerializeResponsesToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcResponse(0L, 0L);
            var jrm2 = new JsonRpcResponse(1L, 0L);
            var jrms = new[] { jrm1, jrm2 };

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.SerializeResponses(jrms, (Stream)null));
        }

        [TestMethod]
        public async Task SerializeResponsesAsyncToStringWhenResponsesIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponsesAsync(null).AsTask());
        }

        [TestMethod]
        public async Task SerializeResponsesAsyncToStreamWhenResponsesIsNull()
        {
            var jrs = new JsonRpcSerializer();

            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jrs.SerializeResponsesAsync(null, stream).AsTask());
            }
        }

        [TestMethod]
        public async Task SerializeResponsesAsyncToStreamWhenStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();
            var jrm1 = new JsonRpcResponse(0L, 0L);
            var jrm2 = new JsonRpcResponse(1L, 0L);
            var jrms = new[] { jrm1, jrm2 };

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.SerializeResponsesAsync(jrms, (Stream)null).AsTask());
        }

        [TestMethod]
        public void DeserializeRequestDataWhenJsonStringIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeRequestData((string)null));
        }

        [TestMethod]
        public void DeserializeRequestDataWhenJsonStringIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(string.Empty));
        }

        [TestMethod]
        public void DeserializeRequestDataFromStreamWhenJsonStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeRequestData((Stream)null));
        }

        [TestMethod]
        public void DeserializeRequestDataFromStreamWhenJsonStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(Stream.Null));
        }

        [TestMethod]
        public async Task DeserializeRequestDataAsyncFromStringWhenJsonStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeRequestDataAsync((string)null).AsTask());
        }

        [TestMethod]
        public async Task DeserializeRequestDataAsyncFromStreamWhenJsonStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeRequestDataAsync((Stream)null).AsTask());
        }

        [TestMethod]
        public async Task DeserializeRequestDataAsyncFromStreamWhenJsonStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                jrs.DeserializeRequestDataAsync(Stream.Null).AsTask());
        }

        [TestMethod]
        public void DeserializeResponseDataWhenJsonStringIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeResponseData((string)null));
        }

        [TestMethod]
        public void DeserializeResponseDataWhenJsonStringIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeResponseData(string.Empty));
        }

        [TestMethod]
        public void DeserializeResponseDataFromStreamWhenJsonStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jrs.DeserializeResponseData((Stream)null));
        }

        [TestMethod]
        public void DeserializeResponseDataFromStreamWhenJsonStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeResponseData(Stream.Null));
        }

        [TestMethod]
        public async Task DeserializeResponseDataAsyncFromStringWhenJsonStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeResponseDataAsync((string)null).AsTask());
        }

        [TestMethod]
        public async Task DeserializeResponseDataAsyncFromStreamWhenJsonStreamIsNull()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jrs.DeserializeResponseDataAsync((Stream)null).AsTask());
        }

        [TestMethod]
        public async Task DeserializeResponseDataAsyncFromStreamWhenJsonStreamIsEmpty()
        {
            var jrs = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                jrs.DeserializeResponseDataAsync(Stream.Null).AsTask());
        }
    }
}