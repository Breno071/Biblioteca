using AutoMapper;
using Domain.Interfaces;
using DotNet.Testcontainers.Builders;
using Infraestructure.Configuration;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace Tests
{
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebApiFactory>, IDisposable
    { 
        protected readonly IServiceScope _scope;
        protected readonly IMapper _mapper;
        protected readonly IReservationService _reservationService;
        protected readonly BaseDbContext DbContext;

        public BaseIntegrationTest(IntegrationTestWebApiFactory factory) 
        {
            _scope = factory.Services.CreateScope();
            _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
            _reservationService =_scope.ServiceProvider.GetRequiredService<IReservationService>();
            DbContext = _scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();
        }
    }
}
