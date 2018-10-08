using System.IO;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace server
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var host = new HostBuilder()
          .ConfigureLogging((hostContext, config) =>
          {
            config.AddConsole();
            config.AddDebug();
          })
          .ConfigureAppConfiguration((context, config) =>
          {
            config.AddEnvironmentVariables();
            config.AddJsonFile("appsettings.json", optional: true);
            config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
            config.AddCommandLine(args);
          })
          .ConfigureServices((hostContext, services) =>
          {
            services.AddLogging();
            services.AddHostedService<GrpcHostedService>();

            services.AddSingleton<ServerPort>(new ServerPort("0.0.0.0", 80,
              ServerCredentials.Insecure));

            services.AddSingleton<ServerPort>(new ServerPort("0.0.0.0", 443,
              new SslServerCredentials(new [] {
                new KeyCertificatePair(
                  File.ReadAllText("cert.pem"),
                  File.ReadAllText("key.pem"))
              })));

            services.AddSingleton(Hello.BindService(new HelloService()));

          })
          .UseConsoleLifetime()
          .Build();

      using (host)
      {
        // Start the host
        await host.StartAsync();
        // Wait for the host to shutdown
        await host.WaitForShutdownAsync();
      }
    }
  }
}
