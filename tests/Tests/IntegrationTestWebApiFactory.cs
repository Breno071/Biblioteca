﻿using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using Infraestructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace Tests
{
    public class IntegrationTestWebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
                            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
                            .WithWaitStrategy(
                                Wait.ForUnixContainer()
                                    .UntilPortIsAvailable(1433))
                        .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptorType =
                    typeof(DbContextOptions<BaseDbContext>);

                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == descriptorType);

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<BaseDbContext>(options =>
                    options.UseSqlServer(_msSqlContainer.GetConnectionString()));
                services.AddAutoMapper(typeof(Program).Assembly);
            });
        }

        public Task InitializeAsync()
        {
            return _msSqlContainer.StartAsync();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _msSqlContainer.StopAsync();
        }
    }
}
