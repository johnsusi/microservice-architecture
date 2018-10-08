using System;
using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;

namespace client
{
  class Program
  {
    public static async Task Main(string[] args)
    {

      await SayHelloInsecure();

      await SayHelloSecure();

    }

    protected static async Task SayHelloInsecure()
    {
      var channel = new Channel("server:80", ChannelCredentials.Insecure);
      var client = new Hello.HelloClient(channel);
      var reply = client.SayHello(new HelloRequest { Name = "Insecure" });
      Console.WriteLine("Greeting: " + reply.Message);
      await channel.ShutdownAsync();
    }

    protected static async Task SayHelloSecure()
    {
      var channel = new Channel(
        "server:443",
        new SslCredentials(File.ReadAllText("roots.pem")),
        new ChannelOption[] {
          // Uncomment for testing:
          // new ChannelOption(ChannelOptions.SslTargetNameOverride, "server")
        });

      var client = new Hello.HelloClient(channel);

      var reply = client.SayHello(new HelloRequest { Name = "Secure" });
      Console.WriteLine("Greeting: " + reply.Message);

      await channel.ShutdownAsync();
    }
  }
}