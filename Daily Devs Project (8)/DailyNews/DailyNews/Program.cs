using DailyNews;
using DailyNews.Services;
using Microsoft.EntityFrameworkCore;
using DailyNews.Mapping;
using System.Text.Json.Serialization;
using Quartz;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using DailyNews.OData;

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

builder.Services.AddControllers()
    .AddOData(opt => opt
        .AddRouteComponents("odata", ODataModelBuilder.GetEdmModel()) 
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .SetMaxTop(100)
        .Count());



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
builder.Services.AddScoped<RSS_SourcesService>();
builder.Services.AddScoped<RSS_CategoryService>();



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

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Ánh xạ các controller
});

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
