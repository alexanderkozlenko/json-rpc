## System.Data.JsonRpc

Provides support for serializing and deserializing [JSON-RPC 2.0](http://www.jsonrpc.org/specification) messages.

[![NuGet package](https://img.shields.io/nuget/v/System.Data.JsonRpc.svg?style=flat-square)](https://www.nuget.org/packages/System.Data.JsonRpc)

### Important Features

- The serializer supports defining response type contracts dependent on method parameters.
- The serializer provides limited backward compatibility with the [JSON-RPC 1.0](http://www.jsonrpc.org/specification_v1).

### Characteristics

- Deserializing a response requires a binding of a message ID to a method name (or a response contract by a message ID).
- Backward compatibility with JSON-RPC 1.0 is limited to the intersection of JSON-RPC 1.0 / 2.0 requirements and the API.

### Usage Examples

```cs
var jrContractResolver = new JsonRpcContractResolver();
var jrSerializer = new JsonRpcSerializer(jrContractResolver);
var jrRequest = new JsonRpcRequest("sum", 1L, new[] { 1L, 2L });
var jrRequestString = jrSerializer.SerializeRequest(jrRequest);

// [...] (Storing an HTTP response string in the "jrResponseString" variable)

jrContractResolver.AddResponseContract("sum", new JsonRpcResponseContract(typeof(long)));
jrContractResolver.AddResponseBinding(jrRequest.Id, "sum");

var jrResponseData = jrSerializer.DeserializeResponseData(jrResponseString);
var jrResult = (long)jrResponseData.Item.Message.Result;
```

- Example of client-side usage: https://github.com/alexanderkozlenko/json-rpc-client
- Example of server-side usage: https://github.com/alexanderkozlenko/aspnetcore-json-rpc