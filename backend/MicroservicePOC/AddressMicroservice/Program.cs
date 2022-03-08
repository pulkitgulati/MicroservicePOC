using AddressMicroservice;
using AddressMicroservice.Data;
using AddressMicroservice.Repository.Interfaces;
using AddressMicroservice.Services.Implementation;
using AddressMicroservice.Services.Interfaces;
using Microsoft.OpenApi.Models;
using NameMicroservice.Repository.Implementation;
using SharedAzureServiceBus.DTO;
using SharedAzureServiceBus.Topic.Implementation;
using SharedAzureServiceBus.Topic.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IProcessAddressData, ProcessAddressData>();
builder.Services.AddSingleton<ITopicSubscription, TopicSubscription>();
builder.Services.AddSingleton<IAddressService, AddressService>();
builder.Services.AddSingleton<IAddressRepository, AddressRepository>();
builder.Services.AddHostedService<Worker_AzureServiceBus>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Address Microservice API",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Address Microservice V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
