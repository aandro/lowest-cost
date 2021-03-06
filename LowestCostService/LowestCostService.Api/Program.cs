using ExternalServiceA.Client;
using ExternalServiceB.Client;
using LowestCostService.Api.Services;
using LowestCostService.Data;

namespace LowestCostService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<IExternalServiceA, ExternalServiceAMock>();
            builder.Services.AddTransient<IExternalServiceB, ExternalServiceBMock>();
            builder.Services.AddTransient<IPackageRepository, PackageRepositoryMock>();
            builder.Services.AddTransient<IWorkerService, WorkerService>();

            builder.Services.AddControllers();
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