using DailyNews;
using DailyNews.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using DailyNews.Mapping;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Cấu hình JSON Serializer Options ;cho phép bộ tuần tự hóa xử lý các tham chiếu vòng bằng cách theo dõi các đối tượng đã được tuần tự hóa trước đó.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Thay từ Preserve sang IgnoreCycles
    options.JsonSerializerOptions.WriteIndented = true; // Tùy chọn: Định dạng JSON đẹp hơn
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping; // Cho phép mã hóa ký tự Unicode đúng cách
});

// Add services to the container.
builder.Services.AddControllers();

// Thêm dịch vụ AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMappingP));

// Register DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký IHttpClientFactory và RssFeedService
builder.Services.AddHttpClient(); // Đăng ký IHttpClientFactory
builder.Services.AddScoped<FetchDataService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<ArticleService>();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
