using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm các dịch vụ (Services) vào container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- KẾT NỐI SQL SERVER ---
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Cố định culture thành InvariantCulture (chấm thập phân)
var cultureInfo = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// 2. Cấu hình luồng xử lý HTTP (Middleware pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- CHO PHÉP ĐỌC FILE TĨNH (ẢNH) ---
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Bắt buộc nằm cuối cùng
app.Run();