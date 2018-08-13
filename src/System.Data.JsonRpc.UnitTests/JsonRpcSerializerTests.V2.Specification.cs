using System.Collections.Generic;
using System.Data.JsonRpc.UnitTests.Resources;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    public partial class JsonRpcSerializerTests
    {
        // Tests based on the JSON-RPC 2.0 specification (http://www.jsonrpc.org/specification)

        #region Example V2 T01: RPC call with positional parameters

        [TestMethod]
        public void V2SpecT010DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("subtract", new JsonRpcRequestContract(new[] { typeof(long), typeof(long) }));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(1L, jsonRpcMessage.Id);
            Assert.AreEqual("subtract", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);
            CollectionAssert.AreEqual(new object[] { 42L, 23L }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V2SpecT010SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("subtract", 1L, new object[] { 42L, 23L });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT010DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            JsonRpcContractResolver.AddResponseBinding(1L, "subtract");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(1L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(long));
            Assert.AreEqual(19L, jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V2SpecT010SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(19L, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT011DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("subtract", new JsonRpcRequestContract(new[] { typeof(long), typeof(long) }));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(2L, jsonRpcMessage.Id);
            Assert.AreEqual("subtract", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);
            CollectionAssert.AreEqual(new object[] { 23L, 42L }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V2SpecT011SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("subtract", 2L, new object[] { 23L, 42L });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT011DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            JsonRpcContractResolver.AddResponseBinding(2L, "subtract");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(2L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(long));
            Assert.AreEqual(-19L, jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V2SpecT011SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(-19L, 2L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T02: RPC call with named parameters

        [TestMethod]
        public void V2SpecT020DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            var jsonRpcSubtractParametersScheme = new Dictionary<string, Type>
            {
                ["subtrahend"] = typeof(long),
                ["minuend"] = typeof(long)
            };

            JsonRpcContractResolver.AddRequestContract("subtract", new JsonRpcRequestContract(jsonRpcSubtractParametersScheme));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(3L, jsonRpcMessage.Id);
            Assert.AreEqual("subtract", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcMessage.ParametersType);
            Assert.AreEqual(23L, jsonRpcMessage.ParametersByName["subtrahend"]);
            Assert.AreEqual(42L, jsonRpcMessage.ParametersByName["minuend"]);
        }

        [TestMethod]
        public void V2SpecT020SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            var jsonRpcSubtractParameters = new Dictionary<string, object>
            {
                ["subtrahend"] = 23L,
                ["minuend"] = 42L
            };

            var jsonRpcMessage = new JsonRpcRequest("subtract", 3L, jsonRpcSubtractParameters);
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT020DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            JsonRpcContractResolver.AddResponseBinding(3L, "subtract");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(3L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(long));
            Assert.AreEqual(19L, jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V2SpecT020SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(19L, 3L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT021DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            var jsonRpcSubtractParametersScheme = new Dictionary<string, Type>
            {
                ["subtrahend"] = typeof(long),
                ["minuend"] = typeof(long)
            };

            JsonRpcContractResolver.AddRequestContract("subtract", new JsonRpcRequestContract(jsonRpcSubtractParametersScheme));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(4L, jsonRpcMessage.Id);
            Assert.AreEqual("subtract", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jsonRpcMessage.ParametersType);
            Assert.AreEqual(23L, jsonRpcMessage.ParametersByName["subtrahend"]);
            Assert.AreEqual(42L, jsonRpcMessage.ParametersByName["minuend"]);
        }

        [TestMethod]
        public void V2SpecT021SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            var jsonRpcSubtractParameters = new Dictionary<string, object>
            {
                ["subtrahend"] = 23L,
                ["minuend"] = 42L
            };

            var jsonRpcMessage = new JsonRpcRequest("subtract", 4L, jsonRpcSubtractParameters);
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT021DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            JsonRpcContractResolver.AddResponseBinding(4L, "subtract");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(4L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(long));
            Assert.AreEqual(19L, jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V2SpecT021SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(19L, 4L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T03: a notification

        [TestMethod]
        public void V2SpecT030DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_03.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcSubtractParametersScheme = new[] { typeof(long), typeof(long), typeof(long), typeof(long), typeof(long) };

            JsonRpcContractResolver.AddRequestContract("update", new JsonRpcRequestContract(jsonRpcSubtractParametersScheme));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.AreEqual("update", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);
            CollectionAssert.AreEqual(new object[] { 1L, 2L, 3L, 4L, 5L }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V2SpecT030SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_03.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("update", new object[] { 1L, 2L, 3L, 4L, 5L });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT031DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_03.1_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("foobar", new JsonRpcRequestContract());

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.AreEqual("foobar", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcMessage.ParametersType);
        }

        [TestMethod]
        public void V2SpecT031SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_03.1_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("foobar");
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T04: RPC call of non-existent method

        [TestMethod]
        public void V2SpecT040DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("foobar", new JsonRpcRequestContract());

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual("1", jsonRpcMessage.Id);
            Assert.AreEqual("foobar", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcMessage.ParametersType);
        }

        [TestMethod]
        public void V2SpecT040SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("foobar", "1");
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT040DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual("1", jsonRpcMessage.Id);
            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMethod, jsonRpcError.Code);
            Assert.IsNotNull(jsonRpcError.Message);
            Assert.AreEqual("Method not found", jsonRpcError.Message);
            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void V2SpecT040SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMethod, "Method not found"), "1");
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T05: RPC call with invalid JSON

        [TestMethod]
        public void V2SpecT050DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_05.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeRequestData(jsonSample));
        }

        [TestMethod]
        public void V2SpecT050SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT050DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_05.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidFormat, jsonRpcError.Code);
            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void V2SpecT050SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_05.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidFormat, "Parse error"), default);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T06: RPC call with invalid request object

        [TestMethod]
        public void V2SpecT060DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_06.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("subtract", new JsonRpcRequestContract());

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcMessageInfo.Exception.ErrorCode);
        }

        [TestMethod]
        public void V2SpecT060SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT060DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_06.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcError.Code);
            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void V2SpecT060SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_06.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T07: RPC call batch, invalid JSON

        [TestMethod]
        public void V2SpecT070DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_07.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeRequestData(jsonSample));
        }

        [TestMethod]
        public void V2SpecT070SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT070DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_07.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidFormat, jsonRpcError.Code);
            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void V2SpecT070SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_07.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidFormat, "Parse error"), default);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T08: RPC call with an empty array

        [TestMethod]
        public void V2SpecT080DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_08.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jsonRpcSerializer.DeserializeRequestData(jsonSample));
        }

        [TestMethod]
        public void V2SpecT080SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT080DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_08.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcError.Code);
            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void V2SpecT080SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_08.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T09: RPC call with an invalid batch (but not empty)

        [TestMethod]
        public void V2SpecT090DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_09.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(1, jsonRpcInfo.Messages.Count);

            var jsonRpcMessageInfo0 = jsonRpcInfo.Messages[0];

            Assert.IsFalse(jsonRpcMessageInfo0.IsValid);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcMessageInfo0.Exception.ErrorCode);
        }

        [TestMethod]
        public void V2SpecT090SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT090DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_09.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(1, jsonRpcInfo.Messages.Count);

            var jsonRpcMessageInfo0 = jsonRpcInfo.Messages[0];

            Assert.IsTrue(jsonRpcMessageInfo0.IsValid);

            var jsonRpcMessage0 = jsonRpcMessageInfo0.Message;

            Assert.AreEqual(default, jsonRpcMessage0.Id);
            Assert.IsFalse(jsonRpcMessage0.Success);

            var jsonRpcError0 = jsonRpcMessage0.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcError0.Code);
            Assert.IsFalse(jsonRpcError0.HasData);
        }

        [TestMethod]
        public void V2SpecT090SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_09.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default);
            var jsonResult = jsonRpcSerializer.SerializeResponses(new[] { jsonRpcMessage });

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T10: RPC call with invalid batch

        [TestMethod]
        public void V2SpecT100DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_10.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(3, jsonRpcInfo.Messages.Count);

            foreach (var jsonRpcMessageInfo in jsonRpcInfo.Messages)
            {
                Assert.IsFalse(jsonRpcMessageInfo.IsValid);
            }
        }

        [TestMethod]
        public void V2SpecT100SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT100DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_10.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);
            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(3, jsonRpcInfo.Messages.Count);

            foreach (var jsonRpcMessageInfo in jsonRpcInfo.Messages)
            {
                Assert.IsTrue(jsonRpcMessageInfo.IsValid);

                var jsonRpcMessage = jsonRpcMessageInfo.Message;

                Assert.AreEqual(default, jsonRpcMessage.Id);
                Assert.IsFalse(jsonRpcMessage.Success);

                var jsonRpcError = jsonRpcMessage.Error;

                Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcError.Code);
                Assert.IsFalse(jsonRpcError.HasData);
            }
        }

        [TestMethod]
        public void V2SpecT100SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_10.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            var jsonRpcMessages = new[]
            {
                new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default),
                new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default),
                new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default)
            };

            var jsonResult = jsonRpcSerializer.SerializeResponses(jsonRpcMessages);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T11: RPC call batch

        [TestMethod]
        public void V2SpecT110DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_11.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("sum", new JsonRpcRequestContract(new[] { typeof(long), typeof(long), typeof(long) }));
            JsonRpcContractResolver.AddRequestContract("notify_hello", new JsonRpcRequestContract(new[] { typeof(long) }));
            JsonRpcContractResolver.AddRequestContract("subtract", new JsonRpcRequestContract(new[] { typeof(long), typeof(long) }));
            JsonRpcContractResolver.AddRequestContract("get_data", new JsonRpcRequestContract());

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(6, jsonRpcInfo.Messages.Count);

            var jsonRpcMessageInfo0 = jsonRpcInfo.Messages[0];

            Assert.IsTrue(jsonRpcMessageInfo0.IsValid);

            var jsonRpcMessage0 = jsonRpcMessageInfo0.Message;

            Assert.AreEqual("1", jsonRpcMessage0.Id);
            Assert.AreEqual("sum", jsonRpcMessage0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage0.ParametersType);
            CollectionAssert.AreEqual(new object[] { 1L, 2L, 4L }, jsonRpcMessage0.ParametersByPosition?.ToArray());

            var jsonRpcMessageInfo1 = jsonRpcInfo.Messages[1];

            Assert.IsTrue(jsonRpcMessageInfo1.IsValid);

            var jsonRpcMessage1 = jsonRpcMessageInfo1.Message;

            Assert.AreEqual(default, jsonRpcMessage1.Id);
            Assert.AreEqual("notify_hello", jsonRpcMessage1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage1.ParametersType);
            CollectionAssert.AreEqual(new object[] { 7L }, jsonRpcMessage1.ParametersByPosition?.ToArray());

            var jsonRpcMessageInfo2 = jsonRpcInfo.Messages[2];

            Assert.IsTrue(jsonRpcMessageInfo2.IsValid);

            var jsonRpcMessage2 = jsonRpcMessageInfo2.Message;

            Assert.AreEqual("2", jsonRpcMessage2.Id);
            Assert.AreEqual("subtract", jsonRpcMessage2.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage2.ParametersType);
            CollectionAssert.AreEqual(new object[] { 42L, 23L }, jsonRpcMessage2.ParametersByPosition?.ToArray());

            var jsonRpcMessageInfo3 = jsonRpcInfo.Messages[3];

            Assert.IsFalse(jsonRpcMessageInfo3.IsValid);

            var jsonRpcMessageInfo4 = jsonRpcInfo.Messages[4];

            Assert.IsFalse(jsonRpcMessageInfo4.IsValid);

            var jsonRpcMessageInfo5 = jsonRpcInfo.Messages[5];

            Assert.IsTrue(jsonRpcMessageInfo5.IsValid);

            var jsonRpcMessage5 = jsonRpcMessageInfo5.Message;

            Assert.AreEqual("9", jsonRpcMessage5.Id);
            Assert.AreEqual("get_data", jsonRpcMessage5.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jsonRpcMessage5.ParametersType);
        }

        [TestMethod]
        public void V2SpecT110SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT110DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_11.0_res.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddResponseContract("sum", new JsonRpcResponseContract(typeof(long)));
            JsonRpcContractResolver.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            JsonRpcContractResolver.AddResponseContract("get_data", new JsonRpcResponseContract(typeof(object[])));
            JsonRpcContractResolver.AddResponseBinding("1", "sum");
            JsonRpcContractResolver.AddResponseBinding("2", "subtract");
            JsonRpcContractResolver.AddResponseBinding("5", "foo.get");
            JsonRpcContractResolver.AddResponseBinding("9", "get_data");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(5, jsonRpcInfo.Messages.Count);

            var jsonRpcMessageInfo0 = jsonRpcInfo.Messages[0];

            Assert.IsTrue(jsonRpcMessageInfo0.IsValid);

            var jsonRpcMessage0 = jsonRpcMessageInfo0.Message;

            Assert.AreEqual("1", jsonRpcMessage0.Id);
            Assert.IsInstanceOfType(jsonRpcMessage0.Result, typeof(long));
            Assert.AreEqual(7L, jsonRpcMessage0.Result);

            var jsonRpcMessageInfo1 = jsonRpcInfo.Messages[1];

            Assert.IsTrue(jsonRpcMessageInfo1.IsValid);

            var jsonRpcMessage1 = jsonRpcMessageInfo1.Message;

            Assert.AreEqual("2", jsonRpcMessage1.Id);
            Assert.IsInstanceOfType(jsonRpcMessage1.Result, typeof(long));
            Assert.AreEqual(19L, jsonRpcMessage1.Result);

            var jsonRpcMessageInfo2 = jsonRpcInfo.Messages[2];

            Assert.IsTrue(jsonRpcMessageInfo2.IsValid);

            var jsonRpcMessage2 = jsonRpcMessageInfo2.Message;

            Assert.AreEqual(default, jsonRpcMessage2.Id);
            Assert.IsFalse(jsonRpcMessage2.Success);

            var jsonRpcError2 = jsonRpcMessage2.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcError2.Code);
            Assert.IsFalse(jsonRpcError2.HasData);

            var jsonRpcMessageInfo3 = jsonRpcInfo.Messages[3];

            Assert.IsTrue(jsonRpcMessageInfo3.IsValid);

            var jsonRpcMessage3 = jsonRpcMessageInfo3.Message;

            Assert.AreEqual("5", jsonRpcMessage3.Id);
            Assert.IsFalse(jsonRpcMessage3.Success);

            var jsonRpcError3 = jsonRpcMessage3.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMethod, jsonRpcError3.Code);
            Assert.IsFalse(jsonRpcError3.HasData);

            var jsonRpcMessageInfo4 = jsonRpcInfo.Messages[4];

            Assert.IsTrue(jsonRpcMessageInfo4.IsValid);

            var jsonRpcMessage4 = jsonRpcMessageInfo4.Message;

            Assert.AreEqual("9", jsonRpcMessage4.Id);
            Assert.IsInstanceOfType(jsonRpcMessage4.Result, typeof(object[]));
            CollectionAssert.AreEqual(new object[] { "hello", 5L }, (object[])jsonRpcMessage4.Result);
        }

        [TestMethod]
        public void V2SpecT110SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_11.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            var jsonRpcMessages = new[]
            {
                new JsonRpcResponse(7L, "1"),
                new JsonRpcResponse(19L, "2"),
                new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"), default),
                new JsonRpcResponse(new JsonRpcError(JsonRpcErrorCode.InvalidMethod, "Method not found"), "5"),
                new JsonRpcResponse(new object[] { "hello", 5L }, "9")
            };

            var jsonResult = jsonRpcSerializer.SerializeResponses(jsonRpcMessages);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T12: RPC call batch (all notifications)

        [TestMethod]
        public void V2SpecT120DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_12.0_req.json");
            var JsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(JsonRpcContractResolver);

            JsonRpcContractResolver.AddRequestContract("notify_sum", new JsonRpcRequestContract(new[] { typeof(long), typeof(long), typeof(long) }));
            JsonRpcContractResolver.AddRequestContract("notify_hello", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsTrue(jsonRpcInfo.IsBatch);
            Assert.AreEqual(2, jsonRpcInfo.Messages.Count);

            var jsonRpcMessageInfo0 = jsonRpcInfo.Messages[0];

            Assert.IsTrue(jsonRpcMessageInfo0.IsValid);

            var jsonRpcMessage0 = jsonRpcMessageInfo0.Message;

            Assert.AreEqual(default, jsonRpcMessage0.Id);
            Assert.AreEqual("notify_sum", jsonRpcMessage0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage0.ParametersType);
            CollectionAssert.AreEqual(new object[] { 1L, 2L, 4L }, jsonRpcMessage0.ParametersByPosition?.ToArray());

            var jsonRpcMessageInfo1 = jsonRpcInfo.Messages[1];

            Assert.IsTrue(jsonRpcMessageInfo1.IsValid);

            var jsonRpcMessage1 = jsonRpcMessageInfo1.Message;

            Assert.AreEqual(default, jsonRpcMessage1.Id);
            Assert.AreEqual("notify_hello", jsonRpcMessage1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage1.ParametersType);
            CollectionAssert.AreEqual(new object[] { 7L }, jsonRpcMessage1.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V2SpecT120SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_spec_12.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            var jsonRpcMessages = new[]
            {
                new JsonRpcRequest("notify_sum", new object[] { 1L, 2L, 4L }),
                new JsonRpcRequest("notify_hello", new object[] { 7L })
            };

            var jsonResult = jsonRpcSerializer.SerializeRequests(jsonRpcMessages);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2SpecT120DeserializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V2SpecT120SerializeResponse()
        {
            // N/A
        }

        #endregion
    }
}