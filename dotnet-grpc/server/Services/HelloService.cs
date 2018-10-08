using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;

namespace server
{

  class HelloService : Hello.HelloBase
  {

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
      return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }

  }
}