using BackEndCyrk.Data;
using BackEndCyrk.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BackEndCyrk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Rejestracja repozytoriow
            builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
            builder.Services.AddScoped<IShippingRateRepository, ShippingRateRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

            // Konfiguracja JSON serializera, aby uniknąć cykli w relacjach
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Ignoruj cykle referencji (rozwiązuje problem z nawigacją między obiektami)
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                    // Nie uwzględniaj właściwości, które mają wartość null
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

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
        }
    }
}