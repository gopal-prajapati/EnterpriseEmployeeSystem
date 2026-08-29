using EnterpriseEmployeeSystem.Api.Data;
using EnterpriseEmployeeSystem.Api.Gateways.Payments;
using EnterpriseEmployeeSystem.Api.Repositories;
using EnterpriseEmployeeSystem.Api.Repositories.Payments;
using EnterpriseEmployeeSystem.Api.Repositories.Products;
using EnterpriseEmployeeSystem.Api.Repositories.Purchases;
using EnterpriseEmployeeSystem.Api.Services;
using EnterpriseEmployeeSystem.Api.Services.Payments;
using EnterpriseEmployeeSystem.Api.Services.Purchases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();

builder.Services.AddScoped<IPurchaseService, PurchaseService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

//builder.Services.AddScoped<IPaymentGateway, SandboxPaymentGateway>();

builder.Services.Configure<RazorpayOptions>(
    builder.Configuration.GetSection("Razorpay"));

builder.Services.AddHttpClient<RazorpayPaymentGateway>(client =>
{
    client.BaseAddress = new Uri("https://api.razorpay.com/");
});

builder.Services.AddScoped<IPaymentGateway>(provider =>
    provider.GetRequiredService<RazorpayPaymentGateway>());

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

}
catch (Exception ex)
{

    throw; 
}


app.Run();
