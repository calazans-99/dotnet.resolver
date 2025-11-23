using Microsoft.EntityFrameworkCore;
using Mentorax.Api.Data;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Mentorax.Api.Repositories.Interfaces;
using Mentorax.Api.Repositories.Implementations;
using Mentorax.Api.Services.Mappings;

var builder = WebApplication.CreateBuilder(args);

// ============================
// Serilog bootstrap
// ============================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ============================
// Services
// ============================
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// API Versioning (simples)
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HealthChecks
builder.Services.AddHealthChecks();

// DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(conn))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(conn));
}

// Repositories registration
builder.Services.AddScoped<IMentorRepository, MentorRepository>();
builder.Services.AddScoped<IMentoradoRepository, MentoradoRepository>();
builder.Services.AddScoped<IQuestionarioRepository, QuestionarioRepository>();
builder.Services.AddScoped<IPlanoMentoriaRepository, PlanoMentoriaRepository>();
builder.Services.AddScoped<ITarefaMentoriaRepository, TarefaMentoriaRepository>();

var app = builder.Build();

// ============================
// Middleware pipeline
// ============================

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mentorax API V1");
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/health/ready");
app.MapHealthChecks("/health/live");

app.MapControllers();

app.Run();
