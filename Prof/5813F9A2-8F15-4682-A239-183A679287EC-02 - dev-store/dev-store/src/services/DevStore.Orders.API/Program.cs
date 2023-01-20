using System;
using DevStore.Orders.API.Configuration;
using DevStore.WebAPI.Core.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using NetDevPack.OpenTelemetry.Otlp;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger());

builder.Services.AddDevPackTracingOtlp(builder.Environment.ApplicationName);

#region Configure Services
builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.RegisterServices();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();
#endregion

#region Configure Pipeline

DbMigrationHelpers.EnsureSeedData(app).Wait();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.Run();

#endregion