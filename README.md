## System.Data.JsonRpc

Provides support for serializing and deserializing [JSON-RPC 2.0](http://www.jsonrpc.org/specification) messages.

[![NuGet package](https://img.shields.io/nuget/v/System.Data.JsonRpc.svg?style=flat-square)](https://www.nuget.org/packages/System.Data.JsonRpc)

### Important Features

- The serializer supports transparent usage of number and string message identifiers.
- The serializer supports dynamic response type contracts when result data type depends on method parameters.
- The serializer supports serializing to and deserializing from streams.

### Characteristics

The serializer is stateful. Due to the JSON-RPC protocol peculiarity, deserializing a response requires more than only defining a response contract. Before deserializing a caller must specify a request identifier mapping to the corresponding method name (static bindings), or specify a request identifier mapping to the corresponding response contract (dynamic bindings).

As recommended by the specification, the serializer provides backward compatibility for [JSON-RPC 1.0](http://www.jsonrpc.org/specification_v1) messages, limited to the intersection of JSON-RPC 1.0 and JSON-RPC 2.0 requirements and the API. As an example, the serializer can be used for serializing and deserializing Bitcoin protocol messages, according to the ["Bitcoin Core APIs - Remote Procedure Calls"](https://bitcoin.org/en/developer-reference#remote-procedure-calls-rpcs) documentation. To enable JSON-RPC 1.0 compatibility, a caller must set the corresponding compatibility level on a serializer instance.

### Usage Examples

- Communication with a JSON-RPC 2.0 server:

```cs
var serializer = new JsonRpcSerializer();
var request = new JsonRpcRequest("sum", 1L, new[] { 1L, 2L });
var requestString = serializer.SerializeRequest(request);

// [Sending an HTTP request and storing a response string in the "responseString"]

serializer.ResponseContracts["sum"] = new JsonRpcResponseContract(typeof(int));
serializer.StaticResponseBindings[request.Id] = "sum";

var responseData = serializer.DeserializeResponseData(responseString);
var result = (int)responseData.Item.Message.Result;
```

- Example of client-side usage: https://github.com/alexanderkozlenko/json-rpc-client
- Example of server-side usage: https://github.com/alexanderkozlenko/aspnetcore-json-rpc