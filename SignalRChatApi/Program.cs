using Dependencies;
using SignalRChatApi.Endpoints;
using SignalRChatApi.Hubs;
using SignalRChatApi.Middlewares;
using SpanJson.AspNetCore.Formatter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddSpanJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(opts =>
{
    opts.AddPolicy("SignalRChat", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("SignalRChatAngular:Url").Value!).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.ConfigureDI(builder.Configuration);


builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SignalRChat");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();
app.MapAuthEndpoints();

app.MapHub<ChatHub>("/chatHub");

app.MigrateDb();

app.Run();
