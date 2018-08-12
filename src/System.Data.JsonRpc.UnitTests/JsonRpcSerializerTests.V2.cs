using System.Data.JsonRpc.UnitTests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    public partial class JsonRpcSerializerTests
    {
        // 2.0 Core tests

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenParametersAreUndefined()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_params_undefined.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidParameters, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenParametersHaveWrongType()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_params_type_wrong.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidParameters, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenParametersHaveInvalidType()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_params_type_invalid.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenErrorTypeIsInvalid()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_type_invalid.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenErrorCodeIsReserved()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_code_reserved.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenErrorMessageIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_message_null.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenHasDataIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_false.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsFalse(jsonRpcError.HasData);
            Assert.IsNull(jsonRpcError.Data);
        }

        [TestMethod]
        public void V2CoreSerializeResponseDataWhenHasDataIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_false.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcError = new JsonRpcError(0L, "m");
            var jsonRpcMessage = new JsonRpcResponse(jsonRpcError, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenHasDataIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string), typeof(long)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsTrue(jsonRpcError.HasData);
            Assert.AreEqual(0L, jsonRpcError.Data);
        }

        [TestMethod]
        public void V2CoreSerializeResponseDataWhenHasDataIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcError = new JsonRpcError(0L, "m", 0L);
            var jsonRpcMessage = new JsonRpcResponse(jsonRpcError, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenHasDataIsTrueAnsIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true_null.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string), typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsTrue(jsonRpcError.HasData);
            Assert.IsNull(jsonRpcError.Data);
        }

        [TestMethod]
        public void V2CoreSerializeResponseDataWhenHasDataIsTrueAnsIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true_null.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcError = new JsonRpcError(0L, "m", null);
            var jsonRpcMessage = new JsonRpcResponse(jsonRpcError, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenIdTypeIsInvalid()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_type_invalid_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract());

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenIdTypeIsInvalid()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_type_invalid_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1D, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreSerializeRequestWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 1D);
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddRequestContract("m", new JsonRpcRequestContract());

            var jsonRpcInfo = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;
            var jsonRpcId = jsonRpcMessage.Id;

            Assert.AreEqual(JsonRpcIdType.Float, jsonRpcId.Type);
            Assert.AreEqual(1D, (double)jsonRpcId);
        }

        [TestMethod]
        public void V2CoreSerializeResponseWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(string.Empty, 1D);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1D, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;
            var jsonRpcId = jsonRpcMessage.Id;

            Assert.AreEqual(JsonRpcIdType.Float, jsonRpcId.Type);
            Assert.AreEqual(1D, (double)jsonRpcId);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseWhenStructureIsInvalidAndSuccessIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_struct_invalid_success_false.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1D, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseWhenStructureIsInvalidAndSuccessIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_struct_invalid_success_true.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1D, "m");

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseWhenErrorIsGenericAndHasData()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_generic_has_data_true.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver);

            jsonRpcContractResolver.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(null, typeof(long)));

            var jsonRpcInfo = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcInfo.IsBatch);

            var jsonRpcMessageInfo = jsonRpcInfo.Message;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsTrue(jsonRpcError.HasData);
            Assert.AreEqual(0L, jsonRpcError.Data);
        }
    }
}