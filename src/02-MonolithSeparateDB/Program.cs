using SimpleSeparateDb.Api.Application;
using SimpleSeparateDb.Api.WriteInfrastructure;
using SimpleSeparateDb.Api.ReadInfrastructure;
using SimpleSeparateDb.Api.WriteInfrastructure.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplication()
    .AddWriteInfrastructure(builder.Configuration)
    .AddReadInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHostedService<OutboxProcessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
