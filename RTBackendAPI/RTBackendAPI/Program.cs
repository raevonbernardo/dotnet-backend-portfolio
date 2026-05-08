using FluentValidation;
using RTBackendAPI.Employees.Endpoints;
using RTBackendAPI.Employees.Extensions;
using Scalar.AspNetCore;

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDefaultParameters();
});

builder.Services.AddSwaggerGen();

builder.RegisterDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.MapAuthEndpoint();
app.MapEmployeeEndpoint();

app.AutoMigrateDatabases();

app.Run();