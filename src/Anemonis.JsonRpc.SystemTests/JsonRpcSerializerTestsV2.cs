using System;
using System.Collections.Generic;
using System.Linq;

using Anemonis.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.SystemTests
{
    [TestClass]
    public sealed class JsonRpcSerializerTestsV2
    {
        #region Example T01: RPC call with positional parameters

        [TestMethod]
        public void DeserializeRequestDataT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("subtract", new JsonRpcRequestContract(new[] { typeof(long), typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.AreEqual("subtract", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            CollectionAssert.AreEqual(new object[] { 42L, 23L }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_req.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(1L, "subtract", new object[] { 42L, 23L });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseBinding(1L, "subtract");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(long));
            Assert.AreEqual(19L, jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(1L, 19L);
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT011()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("subtract", new JsonRpcRequestContract(new[] { typeof(long), typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(2L, jrm.Id);
            Assert.AreEqual("subtract", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            CollectionAssert.AreEqual(new object[] { 23L, 42L }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT011()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_req.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(2L, "subtract", new object[] { 23L, 42L });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT011()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseBinding(2L, "subtract");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(2L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(long));
            Assert.AreEqual(-19L, jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT011()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_01.1_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(2L, -19L);
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T02: RPC call with named parameters

        [TestMethod]
        public void DeserializeRequestDataT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            var jrmp = new Dictionary<string, Type>
            {
                ["subtrahend"] = typeof(long),
                ["minuend"] = typeof(long)
            };

            jrcr.AddRequestContract("subtract", new JsonRpcRequestContract(jrmp));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(3L, jrm.Id);
            Assert.AreEqual("subtract", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm.ParametersType);
            Assert.AreEqual(23L, jrm.ParametersByName["subtrahend"]);
            Assert.AreEqual(42L, jrm.ParametersByName["minuend"]);
        }

        [TestMethod]
        public void SerializeRequestT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_req.json");
            var jrs = new JsonRpcSerializer();

            var jrmp = new Dictionary<string, object>
            {
                ["subtrahend"] = 23L,
                ["minuend"] = 42L
            };

            var jrm = new JsonRpcRequest(3L, "subtract", jrmp);
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseBinding(3L, "subtract");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(3L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(long));
            Assert.AreEqual(19L, jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(3L, 19L);
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT021()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            var jrmp = new Dictionary<string, Type>
            {
                ["subtrahend"] = typeof(long),
                ["minuend"] = typeof(long)
            };

            jrcr.AddRequestContract("subtract", new JsonRpcRequestContract(jrmp));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(4L, jrm.Id);
            Assert.AreEqual("subtract", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm.ParametersType);
            Assert.AreEqual(23L, jrm.ParametersByName["subtrahend"]);
            Assert.AreEqual(42L, jrm.ParametersByName["minuend"]);
        }

        [TestMethod]
        public void SerializeRequestT021()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_req.json");
            var jrs = new JsonRpcSerializer();

            var jrmp = new Dictionary<string, object>
            {
                ["subtrahend"] = 23L,
                ["minuend"] = 42L
            };

            var jrm = new JsonRpcRequest(4L, "subtract", jrmp);
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT021()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseBinding(4L, "subtract");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(4L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(long));
            Assert.AreEqual(19L, jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT021()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_02.1_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(4L, 19L);
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T03: RPC notification

        [TestMethod]
        public void DeserializeRequestDataT030()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_03.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrrcp = new[] { typeof(long), typeof(long), typeof(long), typeof(long), typeof(long) };

            jrcr.AddRequestContract("update", new JsonRpcRequestContract(jrrcp));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("update", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            CollectionAssert.AreEqual(new object[] { 1L, 2L, 3L, 4L, 5L }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT030()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_03.0_req.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(default, "update", new object[] { 1L, 2L, 3L, 4L, 5L });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT031()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_03.1_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("foobar", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("foobar", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm.ParametersType);
        }

        [TestMethod]
        public void SerializeRequestT031()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_03.1_req.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest(default, "foobar");
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T04: RPC call of non-existent method

        [TestMethod]
        public void DeserializeRequestDataT040()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("foobar", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.AreEqual("foobar", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm.ParametersType);
        }

        [TestMethod]
        public void SerializeRequestT040()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_req.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcRequest("1", "foobar");
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT040()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.IsFalse(jrm.Success);

            var jre = jrm.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMethod, jre.Code);
            Assert.IsNotNull(jre.Message);
            Assert.AreEqual("Method not found", jre.Message);
            Assert.IsFalse(jre.HasData);
        }

        [TestMethod]
        public void SerializeResponseT040()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_04.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse("1", new JsonRpcError(JsonRpcErrorCode.InvalidMethod, "Method not found"));
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T05: RPC call with invalid JSON

        [TestMethod]
        public void DeserializeRequestDataT050()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_05.0_req.json");
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(jsont));
        }

        [TestMethod]
        public void DeserializeResponseDataT050()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_05.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);

            var jre = jrm.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidFormat, jre.Code);
            Assert.IsFalse(jre.HasData);
        }

        [TestMethod]
        public void SerializeResponseT050()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_05.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidFormat, "Parse error"));
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T06: RPC call with invalid request object

        [TestMethod]
        public void DeserializeRequestDataT060()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_06.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("subtract", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsFalse(jrmi.IsValid);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jrmi.Exception.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataT060()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_06.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);

            var jre = jrm.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.Code);
            Assert.IsFalse(jre.HasData);
        }

        [TestMethod]
        public void SerializeResponseT060()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_06.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"));
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T07: RPC call batch, invalid JSON

        [TestMethod]
        public void DeserializeRequestDataT070()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_07.0_req.json");
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(jsont));
        }

        [TestMethod]
        public void DeserializeResponseDataT070()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_07.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);

            var jre = jrm.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidFormat, jre.Code);
            Assert.IsFalse(jre.HasData);
        }

        [TestMethod]
        public void SerializeResponseT070()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_07.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidFormat, "Parse error"));
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T08: RPC call with an empty array

        [TestMethod]
        public void DeserializeRequestDataT080()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_08.0_req.json");
            var jrs = new JsonRpcSerializer();

            Assert.ThrowsException<InvalidOperationException>(() =>
                jrs.DeserializeRequestData(jsont));
        }

        [TestMethod]
        public void DeserializeResponseDataT080()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_08.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);

            var jre = jrm.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.Code);
            Assert.IsFalse(jre.HasData);
        }

        [TestMethod]
        public void SerializeResponseT080()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_08.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"));
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T09: RPC call with an invalid batch (but not empty)

        [TestMethod]
        public void DeserializeRequestDataT090()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_09.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(1, jrd.Items.Count);

            var jrmi0 = jrd.Items[0];

            Assert.IsFalse(jrmi0.IsValid);
            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jrmi0.Exception.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataT090()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_09.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(1, jrd.Items.Count);

            var jrmi0 = jrd.Items[0];

            Assert.IsTrue(jrmi0.IsValid);

            var jrm0 = jrmi0.Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.IsFalse(jrm0.Success);

            var jre0 = jrm0.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre0.Code);
            Assert.IsFalse(jre0.HasData);
        }

        [TestMethod]
        public void SerializeResponseT090()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_09.0_res.json");
            var jrs = new JsonRpcSerializer();
            var jrm = new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"));
            var jsonr = jrs.SerializeResponses(new[] { jrm });

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T10: RPC call with invalid batch

        [TestMethod]
        public void DeserializeRequestDataT100()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_10.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(3, jrd.Items.Count);

            foreach (var jrmi in jrd.Items)
            {
                Assert.IsFalse(jrmi.IsValid);
            }
        }

        [TestMethod]
        public void DeserializeResponseDataT100()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_10.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);
            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(3, jrd.Items.Count);

            foreach (var jrmi in jrd.Items)
            {
                Assert.IsTrue(jrmi.IsValid);

                var jrm = jrmi.Message;

                Assert.AreEqual(default, jrm.Id);
                Assert.IsFalse(jrm.Success);

                var jre = jrm.Error;

                Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.Code);
                Assert.IsFalse(jre.HasData);
            }
        }

        [TestMethod]
        public void SerializeResponseT100()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_10.0_res.json");
            var jrs = new JsonRpcSerializer();

            var jrms = new[]
            {
                new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request")),
                new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request")),
                new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request"))
            };

            var result = jrs.SerializeResponses(jrms);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, result);
        }

        #endregion

        #region Example T11: RPC call batch

        [TestMethod]
        public void DeserializeRequestDataT110()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_11.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("sum", new JsonRpcRequestContract(new[] { typeof(long), typeof(long), typeof(long) }));
            jrcr.AddRequestContract("notify_hello", new JsonRpcRequestContract(new[] { typeof(long) }));
            jrcr.AddRequestContract("subtract", new JsonRpcRequestContract(new[] { typeof(long), typeof(long) }));
            jrcr.AddRequestContract("get_data", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(6, jrd.Items.Count);

            var jrmi0 = jrd.Items[0];

            Assert.IsTrue(jrmi0.IsValid);

            var jrm0 = jrmi0.Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.AreEqual("sum", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm0.ParametersType);
            CollectionAssert.AreEqual(new object[] { 1L, 2L, 4L }, jrm0.ParametersByPosition?.ToArray());

            var jrmi1 = jrd.Items[1];

            Assert.IsTrue(jrmi1.IsValid);

            var jrm1 = jrmi1.Message;

            Assert.AreEqual(default, jrm1.Id);
            Assert.AreEqual("notify_hello", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm1.ParametersType);
            CollectionAssert.AreEqual(new object[] { 7L }, jrm1.ParametersByPosition?.ToArray());

            var jrmi2 = jrd.Items[2];

            Assert.IsTrue(jrmi2.IsValid);

            var jrm2 = jrmi2.Message;

            Assert.AreEqual("2", jrm2.Id);
            Assert.AreEqual("subtract", jrm2.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm2.ParametersType);
            CollectionAssert.AreEqual(new object[] { 42L, 23L }, jrm2.ParametersByPosition?.ToArray());

            var jrmi3 = jrd.Items[3];

            Assert.IsFalse(jrmi3.IsValid);

            var jrmi4 = jrd.Items[4];

            Assert.IsFalse(jrmi4.IsValid);

            var jrmi5 = jrd.Items[5];

            Assert.IsTrue(jrmi5.IsValid);

            var jrm5 = jrmi5.Message;

            Assert.AreEqual("9", jrm5.Id);
            Assert.AreEqual("get_data", jrm5.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm5.ParametersType);
        }

        [TestMethod]
        public void DeserializeResponseDataT110()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_11.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("sum", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract("subtract", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract("get_data", new JsonRpcResponseContract(typeof(object[])));
            jrcr.AddResponseBinding("1", "sum");
            jrcr.AddResponseBinding("2", "subtract");
            jrcr.AddResponseBinding("5", "foo.get");
            jrcr.AddResponseBinding("9", "get_data");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(5, jrd.Items.Count);

            var jrmi0 = jrd.Items[0];

            Assert.IsTrue(jrmi0.IsValid);

            var jrm0 = jrmi0.Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.IsInstanceOfType(jrm0.Result, typeof(long));
            Assert.AreEqual(7L, jrm0.Result);

            var jrmi1 = jrd.Items[1];

            Assert.IsTrue(jrmi1.IsValid);

            var jrm1 = jrmi1.Message;

            Assert.AreEqual("2", jrm1.Id);
            Assert.IsInstanceOfType(jrm1.Result, typeof(long));
            Assert.AreEqual(19L, jrm1.Result);

            var jrmi2 = jrd.Items[2];

            Assert.IsTrue(jrmi2.IsValid);

            var jrm2 = jrmi2.Message;

            Assert.AreEqual(default, jrm2.Id);
            Assert.IsFalse(jrm2.Success);

            var jre2 = jrm2.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre2.Code);
            Assert.IsFalse(jre2.HasData);

            var jrmi3 = jrd.Items[3];

            Assert.IsTrue(jrmi3.IsValid);

            var jrm3 = jrmi3.Message;

            Assert.AreEqual("5", jrm3.Id);
            Assert.IsFalse(jrm3.Success);

            var jre3 = jrm3.Error;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMethod, jre3.Code);
            Assert.IsFalse(jre3.HasData);

            var jrmi4 = jrd.Items[4];

            Assert.IsTrue(jrmi4.IsValid);

            var jrm4 = jrmi4.Message;

            Assert.AreEqual("9", jrm4.Id);
            Assert.IsInstanceOfType(jrm4.Result, typeof(object[]));
            CollectionAssert.AreEqual(new object[] { "hello", 5L }, (object[])jrm4.Result);
        }

        [TestMethod]
        public void SerializeResponseT110()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_11.0_res.json");
            var jrs = new JsonRpcSerializer();

            var jrms = new[]
            {
                new JsonRpcResponse("1", 7L),
                new JsonRpcResponse("2", 19L),
                new JsonRpcResponse(default, new JsonRpcError(JsonRpcErrorCode.InvalidMessage, "Invalid Request")),
                new JsonRpcResponse("5", new JsonRpcError(JsonRpcErrorCode.InvalidMethod, "Method not found")),
                new JsonRpcResponse("9", new object[] { "hello", 5L })
            };

            var jsonr = jrs.SerializeResponses(jrms);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T12: RPC call batch (all notifications)

        [TestMethod]
        public void DeserializeRequestDataT120()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_12.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("notify_sum", new JsonRpcRequestContract(new[] { typeof(long), typeof(long), typeof(long) }));
            jrcr.AddRequestContract("notify_hello", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsTrue(jrd.IsBatch);
            Assert.AreEqual(2, jrd.Items.Count);

            var jrmi0 = jrd.Items[0];

            Assert.IsTrue(jrmi0.IsValid);

            var jrm0 = jrmi0.Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.AreEqual("notify_sum", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm0.ParametersType);
            CollectionAssert.AreEqual(new object[] { 1L, 2L, 4L }, jrm0.ParametersByPosition?.ToArray());

            var jrmi1 = jrd.Items[1];

            Assert.IsTrue(jrmi1.IsValid);

            var jrm1 = jrmi1.Message;

            Assert.AreEqual(default, jrm1.Id);
            Assert.AreEqual("notify_hello", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm1.ParametersType);
            CollectionAssert.AreEqual(new object[] { 7L }, jrm1.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT120()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_spec_12.0_req.json");
            var jrs = new JsonRpcSerializer();

            var jrms = new[]
            {
                new JsonRpcRequest(default, "notify_sum", new object[] { 1L, 2L, 4L }),
                new JsonRpcRequest(default, "notify_hello", new object[] { 7L })
            };

            var jsonr = jrs.SerializeRequests(jrms);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion
    }
}
