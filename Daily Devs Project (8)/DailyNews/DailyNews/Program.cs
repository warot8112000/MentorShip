using DailyNews;
using DailyNews.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using DailyNews.Mapping;
using System.Text.Json.Serialization;
using Quartz;
using Quartz.Spi;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình JSON Serializer Options ;cho phép bộ tuần tự hóa xử lý các tham chiếu vòng bằng cách theo dõi các đối tượng đã được tuần tự hóa trước đó.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; 
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping; // Cho phép mã hóa ký tự Unicode đúng cách
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();  
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(AutoMappingP));

// Register DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register IHttpClientFactory and RssFeedService
builder.Services.AddHttpClient(); // Register IHttpClientFactory
builder.Services.AddScoped<FetchDataService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ArticleService>();

var cronSchedule = builder.Configuration["Quartz:FetchArticlesCronSchedule"];

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("fetchArticlesJob");
    q.AddJob<FetchArticlesJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("fetchArticlesJobTrigger")
        .WithCronSchedule(cronSchedule));
});

// Register QuartzHostedService to run Quartz
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
