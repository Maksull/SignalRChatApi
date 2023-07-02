using SignalRChatApi.Middlewares;
using SpanJson.AspNetCore.Formatter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddSpanJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
