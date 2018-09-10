## System.Data.JsonRpc

Provides support for serializing and deserializing [JSON-RPC 2.0](http://www.jsonrpc.org/specification) messages.

[![NuGet package](https://img.shields.io/nuget/v/System.Data.JsonRpc.svg?style=flat-square)](https://www.nuget.org/packages/System.Data.JsonRpc)

### Important Features

- The serializer supports defining response type contracts dependent on method parameters.
- The serializer provides backward compatibility with the [JSON-RPC 1.0](http://www.jsonrpc.org/specification_v1).

### Limitations

- Backward compatibility with JSON-RPC 1.0 is limited to the intersection of JSON-RPC 1.0 / 2.0 requirements and the API.

### Usage Examples

```cs
var contracts = new JsonRpcContractResolver();
var serializer = new JsonRpcSerializer(contracts);

contracts.AddResponseContract("sum", new JsonRpcResponseContract(typeof(long)));

var request = new JsonRpcRequest("sum", 1L, new[] { 1L, 2L });
var requestString = serializer.SerializeRequest(request);

// [Executing the corresponding HTTP request]

contracts.AddResponseBinding(request.Id, request.Method);

var responseData = serializer.DeserializeResponseData(responseString);
var response = responseData.Item.Message;

Console.WriteLine((long)response.Result);
```

- Example of client-side usage: https://github.com/alexanderkozlenko/json-rpc-client
- Example of server-side usage: https://github.com/alexanderkozlenko/aspnetcore-json-rpc