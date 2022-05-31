using System.Net.Mime;
using System.Text.Json;
using Catalog.Collections;
using Catalog.Config;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment env = builder.Environment;

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var mongoDbConfig = configuration.GetSection(nameof(MongoDBConfig)).Get<MongoDBConfig>(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Check fails if not connected to DB for more than 5 seconds
builder.Services.AddHealthChecks()
        .AddMongoDb(
            mongoDbConfig.ConnectionString, 
            name: "mongodb", 
            timeout: TimeSpan.FromSeconds(5),
            tags: new[] {"ready"}
        ); 


// Tell the MongoDB how to serialize the Date and Guid format because it might end up serializing as false vars
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

builder.Services.AddSingleton<IMongoClient>(serviceProvider => 
                {
                    return new MongoClient(mongoDbConfig.ConnectionString);
                });

 builder.Services.AddSingleton<IInMemoryCollections, MongoDBCollections>();                

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Check if the environment is development or production and enable it accordingly
// if (env.IsDevelopment()) {app.UseHttpsRedirection(); }

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready"),
                ResponseWriter = async(context, report) => 
                {
                    var result = JsonSerializer.Serialize(
                        new{
                            status = report.Status.ToString(),
                            checks = report.Entries.Select(entry => new{
                                name = entry.Key,
                                status = entry.Value.Status.ToString(),
                                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                duration = entry.Value.Duration.ToString()
                            })
                        }
                    );

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });

app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = (_) => false 
            });

app.Run();
