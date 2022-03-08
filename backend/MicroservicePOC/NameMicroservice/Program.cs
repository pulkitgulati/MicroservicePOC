using Microsoft.OpenApi.Models;
using NameMicroservice.Repository.Implementation;
using NameMicroservice.Repository.Interfaces;
using NameMicroservice.Services.Implementation;
using NameMicroservice.Services.Interfaces;
using SharedAzureServiceBus.Queue.Implementation;
using SharedAzureServiceBus.Queue.Interface;
using SharedAzureServiceBus.Topic.Implementation;
using SharedAzureServiceBus.Topic.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITopicSender,TopicSender>();
builder.Services.AddScoped<IQueueSender, QueueSender>();
builder.Services.AddScoped<INameRepository,NameRepository>();
builder.Services.AddScoped<INameService, NameService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Name Microservice API",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Name Microservice V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
