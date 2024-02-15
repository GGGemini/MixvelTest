
using MixvelTest.Agregators;
using MixvelTest.Mediators;
using MixvelTest.Services;
using MixvelTest.Services.Interfaces;

namespace MixvelTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMemoryCache();

            builder.Services.AddHttpClient<IProviderOneService, ProviderOneService>();
            builder.Services.AddHttpClient<IProviderTwoService, ProviderTwoService>();

            builder.Services.AddScoped<ISearchService, SearchService>();

            builder.Services.AddSingleton<ISearchResponseAggregator, SearchResponseAggregator>();
            builder.Services.AddSingleton<ISearchRequestMediator, SearchRequestMediator>();
            builder.Services.AddSingleton<ICacheService, CacheService>();

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
