using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Grpc.Core.Server;

namespace server
{

  class GrpcHostedService : IHostedService, IDisposable
  {
    private readonly ILogger _logger;
    private Server _server;
    private IEnumerable<ServerServiceDefinition> _services;
    private IEnumerable<ServerPort> _ports;

    public GrpcHostedService(ILogger<HelloService> logger, IEnumerable<ServerServiceDefinition> services, IEnumerable<ServerPort> ports)
    {
      _logger = logger;
      _services = services;
      _ports = ports;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {

      _logger.LogInformation("GrpcHostedService is starting.");

      _server = new Server();

      foreach (var port in _ports)
      {
        _server.Ports.Add(port);
      }

      foreach (var service in _services)
      {
        _server.Services.Add(service);
      }

      _server.Start();

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("GrpcHostedService is stopping.");

      return _server.ShutdownAsync();
    }

    public void Dispose()
    {
    }

  }
}