using System.Reflection;
using Application.Mappings;
using Application.Services.Base;
using Application.Services.Projects;
using Application.Services.Tasks;
using Data.Contexts;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(opt => {
    opt.InvalidModelStateResponseFactory = context => 
        new BadRequestObjectResult(context.ModelState.Values.First(q => q.Errors.Count > 0).Errors
            .First(er => !string.IsNullOrEmpty(er.ErrorMessage)).ErrorMessage);
});
builder.Services.AddDbContext<ApplicationContext>(o =>
    o.UseNpgsql(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddAutoMapper(typeof(TasksProfile), typeof(ProjectsProfile));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Test Task API",
        Description =
            "Basic CRUD API for Projects and Tasks"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();