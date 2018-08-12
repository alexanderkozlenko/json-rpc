using System.Data.JsonRpc.UnitTests.Resources;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.Data.JsonRpc.UnitTests
{
    [TestClass]
    public sealed partial class JsonRpcSerializerTests
    {
        [Conditional("DEBUG")]
        private static void TraceJsonToken(JToken token)
        {
            Trace.WriteLine(token.ToString(Formatting.Indented));
        }

        private static void CompareJsonStrings(string expected, string actual)
        {
            var expectedToken = JToken.Parse(expected);
            var actualToken = JToken.Parse(actual);

            TraceJsonToken(actualToken);

            Assert.IsTrue(JToken.DeepEquals(expectedToken, actualToken), "Actual JSON string differs from expected");
        }

        [TestMethod]
        public void CoreSerializeRequestWhenRequestIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeRequest(null));
        }

        [TestMethod]
        public void CoreSerializeRequestsWhenCollectionIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeRequests(null));
        }

        [TestMethod]
        public void CoreSerializeRequestsWhenCollectionIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcException>(() =>
                jsonRpcSerializer.SerializeRequests(new JsonRpcRequest[] { }));

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void CoreSerializeRequestsWhenCollectionContainsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcException>(() =>
                jsonRpcSerializer.SerializeRequests(new JsonRpcRequest[] { null }));

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void CoreSerializeRequestToStreamWhenRequestIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeRequest(null, jsonStream));
            }
        }

        [TestMethod]
        public void CoreSerializeRequestToStreamWhenStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeRequest(jsonRpcMessage, null));
        }

        [TestMethod]
        public void CoreSerializeRequestToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                jsonRpcSerializer.SerializeRequest(jsonRpcMessage, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public async Task CoreSerializeRequestAsyncToStreamWhenRequestIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeRequestAsync(null, jsonStream));
            }
        }

        [TestMethod]
        public async Task CoreSerializeRequestAsyncToStreamWhenStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeRequestAsync(jsonRpcMessage, null));
        }

        [TestMethod]
        public async Task CoreSerializeRequestAsyncToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                await jsonRpcSerializer.SerializeRequestAsync(jsonRpcMessage, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public void CoreSerializeRequestsToStreamWhenRequestsIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeRequests(null, jsonStream));
            }
        }

        [TestMethod]
        public void CoreSerializeRequestsToStreamWhenStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcRequest("m", 0L);
            var jsonRpcMessage2 = new JsonRpcRequest("m", 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeRequests(jsonRpcMessages, null));
        }

        [TestMethod]
        public void CoreSerializeRequestsToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_batch_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcRequest("m", 0L);
            var jsonRpcMessage2 = new JsonRpcRequest("m", 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            using (var jsonStream = new MemoryStream())
            {
                jsonRpcSerializer.SerializeRequests(jsonRpcMessages, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public async Task CoreSerializeRequestsAsyncToStreamWhenRequestIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeRequestsAsync(null, jsonStream));
            }
        }

        [TestMethod]
        public async Task CoreSerializeRequestsAsyncToStreamWhenStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcRequest("m", 0L);
            var jsonRpcMessage2 = new JsonRpcRequest("m", 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeRequestsAsync(jsonRpcMessages, null));
        }

        [TestMethod]
        public async Task CoreSerializeRequestsAsyncToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_batch_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcRequest("m", 0L);
            var jsonRpcMessage2 = new JsonRpcRequest("m", 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            using (var jsonStream = new MemoryStream())
            {
                await jsonRpcSerializer.SerializeRequestsAsync(jsonRpcMessages, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public void CoreSerializeResponseWhenResponseIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeResponse(null));
        }

        [TestMethod]
        public void CoreSerializeResponsesWhenCollectionIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeResponses(null));
        }

        [TestMethod]
        public void CoreSerializeResponsesWhenCollectionIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcException>(() =>
                jsonRpcSerializer.SerializeResponses(new JsonRpcResponse[] { }));

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void CoreSerializeResponsesWhenCollectionContainsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            var exception = Assert.ThrowsException<JsonRpcException>(() =>
                jsonRpcSerializer.SerializeResponses(new JsonRpcResponse[] { null }));

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void CoreSerializeResponseToStreamWhenResponseIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeResponse(null, jsonStream));
            }
        }

        [TestMethod]
        public void CoreSerializeResponseToStreamWhenStreamIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(0L, 0L);

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeResponse(jsonRpcMessage, null));
        }

        [TestMethod]
        public void CoreSerializeResponseToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(0L, 0L);

            using (var jsonStream = new MemoryStream())
            {
                jsonRpcSerializer.SerializeResponse(jsonRpcMessage, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public async Task CoreSerializeResponseAsyncToStreamWhenResponseIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeResponseAsync(null, jsonStream));
            }
        }

        [TestMethod]
        public async Task CoreSerializeResponseAsyncToStreamWhenStreamIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(0L, 0L);

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeResponseAsync(jsonRpcMessage, null));
        }

        [TestMethod]
        public async Task CoreSerializeResponseAsyncToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(0L, 0L);

            using (var jsonStream = new MemoryStream())
            {
                await jsonRpcSerializer.SerializeResponseAsync(jsonRpcMessage, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public void CoreSerializeResponsesToStreamWhenResponsesIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeResponses(null, jsonStream));
            }
        }

        [TestMethod]
        public void CoreSerializeResponsesToStreamWhenStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcResponse(0L, 0L);
            var jsonRpcMessage2 = new JsonRpcResponse(0L, 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeResponses(jsonRpcMessages, null));
        }

        [TestMethod]
        public void CoreSerializeResponsesToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_batch_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcResponse(0L, 0L);
            var jsonRpcMessage2 = new JsonRpcResponse(0L, 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            using (var jsonStream = new MemoryStream())
            {
                jsonRpcSerializer.SerializeResponses(jsonRpcMessages, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public async Task CoreSerializeResponsesAsyncToStreamWhenResponsesIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 0L);

            using (var jsonStream = new MemoryStream())
            {
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                    jsonRpcSerializer.SerializeResponsesAsync(null, jsonStream));
            }
        }

        [TestMethod]
        public async Task CoreSerializeResponsesAsyncToStreamWhenStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcResponse(0L, 0L);
            var jsonRpcMessage2 = new JsonRpcResponse(0L, 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jsonRpcSerializer.SerializeResponsesAsync(jsonRpcMessages, null));
        }

        [TestMethod]
        public async Task CoreSerializeResponsesAsyncToStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_batch_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage1 = new JsonRpcResponse(0L, 0L);
            var jsonRpcMessage2 = new JsonRpcResponse(0L, 1L);
            var jsonRpcMessages = new[] { jsonRpcMessage1, jsonRpcMessage2 };

            using (var jsonStream = new MemoryStream())
            {
                await jsonRpcSerializer.SerializeResponsesAsync(jsonRpcMessages, jsonStream);

                var jsonResult = Encoding.UTF8.GetString(jsonStream.ToArray());

                CompareJsonStrings(jsonSample, jsonResult);
            }
        }

        [TestMethod]
        public void CoreDeserializeRequestDataWhenJsonStringIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.DeserializeRequestData((string)null));
        }

        [TestMethod]
        public void CoreDeserializeRequestDataWhenJsonStringIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeRequestData(string.Empty));
        }

        [TestMethod]
        public void CoreDeserializeRequestDataFromStreamWhenJsonStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.DeserializeRequestData((Stream)null));
        }

        [TestMethod]
        public void CoreDeserializeRequestDataFromStreamWhenJsonStreamIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeRequestData(Stream.Null));
        }

        [TestMethod]
        public void CoreDeserializeRequestDatatFromStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract());

            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonSample)))
            {
                var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonStream);

                Assert.IsFalse(jsonRpcInfo.IsBatch);

                var jsonRpcMessageInfo = jsonRpcInfo.Message;

                Assert.IsTrue(jsonRpcMessageInfo.IsValid);

                var jsonRpcMessage = jsonRpcMessageInfo.Message;

                Assert.AreEqual(0L, jsonRpcMessage.Id);
                Assert.AreEqual("m", jsonRpcMessage.Method);
                Assert.AreEqual(JsonRpcParametersType.None, jsonRpcMessage.ParametersType);
            }
        }

        [TestMethod]
        public async Task CoreDeserializeRequestDataAsyncFromStreamWhenJsonStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jsonRpcSerializer.DeserializeRequestDataAsync((Stream)null));
        }

        [TestMethod]
        public async Task CoreDeserializeRequestDataAsyncFromStreamWhenJsonStreamIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeRequestDataAsync(Stream.Null));
        }

        [TestMethod]
        public async Task CoreDeserializeRequestDatatAsyncFromStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract());

            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonSample)))
            {
                var jsonRpcInfo = await jsonRpcSerializer.DeserializeRequestDataAsync(jsonStream);

                Assert.IsFalse(jsonRpcInfo.IsBatch);

                var jsonRpcMessageInfo = jsonRpcInfo.Message;

                Assert.IsTrue(jsonRpcMessageInfo.IsValid);

                var jsonRpcMessage = jsonRpcMessageInfo.Message;

                Assert.AreEqual(0L, jsonRpcMessage.Id);
                Assert.AreEqual("m", jsonRpcMessage.Method);
                Assert.AreEqual(JsonRpcParametersType.None, jsonRpcMessage.ParametersType);
            }
        }

        [TestMethod]
        public void CoreDeserializeResponseDataWhenJsonStringIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.DeserializeResponseData((string)null));
        }

        [TestMethod]
        public void CoreDeserializeResponseDataWhenJsonStringIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeResponseData(string.Empty));
        }

        [TestMethod]
        public void CoreDeserializeResponseDataFromStreamWhenJsonStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<ArgumentNullException>(() =>
                jsonRpcSerializer.DeserializeResponseData((Stream)null));
        }

        [TestMethod]
        public void CoreDeserializeResponseDataFromStreamWhenJsonStreamIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeResponseData(Stream.Null));
        }

        [TestMethod]
        public void CoreDeserializeResponseDatatFromStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract(0L, new JsonRpcResponseContract(typeof(long)));

            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonSample)))
            {
                var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonStream);

                Assert.IsFalse(jsonRpcInfo.IsBatch);

                var jsonRpcMessageInfo = jsonRpcInfo.Message;

                Assert.IsTrue(jsonRpcMessageInfo.IsValid);

                var jsonRpcMessage = jsonRpcMessageInfo.Message;

                Assert.AreEqual(0L, jsonRpcMessage.Id);
                Assert.IsTrue(jsonRpcMessage.Success);
                Assert.AreEqual(0L, jsonRpcMessage.Result);
            }
        }

        [TestMethod]
        public async Task CoreDeserializeResponseDataAsyncFromStreamWhenJsonStreamIsNull()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                jsonRpcSerializer.DeserializeResponseDataAsync((Stream)null));
        }

        [TestMethod]
        public async Task CoreDeserializeResponseDataAsyncFromStreamWhenJsonStreamIsEmpty()
        {
            var jsonRpcSerializer = new JsonRpcSerializer();

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeResponseDataAsync(Stream.Null));
        }

        [TestMethod]
        public async Task CoreDeserializeResponseDatatAsyncFromStream()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract(0L, new JsonRpcResponseContract(typeof(long)));

            using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonSample)))
            {
                var jsonRpcInfo = await jsonRpcSerializer.DeserializeResponseDataAsync(jsonStream);

                Assert.IsFalse(jsonRpcInfo.IsBatch);

                var jsonRpcMessageInfo = jsonRpcInfo.Message;

                Assert.IsTrue(jsonRpcMessageInfo.IsValid);

                var jsonRpcMessage = jsonRpcMessageInfo.Message;

                Assert.AreEqual(0L, jsonRpcMessage.Id);
                Assert.IsTrue(jsonRpcMessage.Success);
                Assert.AreEqual(0L, jsonRpcMessage.Result);
            }
        }

        [TestMethod]
        public void IsSystemMethodWhenMethodIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcSerializer.IsSystemMethod((string)null));
        }

        [TestMethod]
        public void IsSystemMethodIsFalse()
        {
            var result = JsonRpcSerializer.IsSystemMethod("m");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsSystemMethodIsTrueWithLowerCase()
        {
            var result = JsonRpcSerializer.IsSystemMethod("rpc.m");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSystemMethodIsTrueWithUpperCase()
        {
            var result = JsonRpcSerializer.IsSystemMethod("RPC.M");

            Assert.IsTrue(result);
        }
    }
}