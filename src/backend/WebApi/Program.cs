using Application;
using Infrastructure;
using Microsoft.OpenApi.Models;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrasctrucutreServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products Management", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
    //options.WithOrigins("http://localhost:4200");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
