using Loader.Entries;
using Loader.Loader;
using Loader.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace Loader.Services
{
	public static class LoaderServicesExtension
	{
		public static IServiceCollection AddLoaderServices(this IServiceCollection services)
		{
			services.AddSingleton<TranslationService>()
			        .AddSingleton<LocalisationService>()
			        .AddSingleton<LoaderService<Manufacturer>, ManufacturersService>()
			        .AddSingleton<LoaderService<RetailProduct>, RetailProductService>()
			        .AddSingleton<LoaderService<Shop>, ShopService>()
			        .AddSingleton<LoaderService<Item>, ItemsService>()
			        .AddSingleton<LoaderService<Ship>, ShipsService>();

			services.AddScoped<ItemLoader>()
			        .AddScoped<ManufacturerLoader>()
			        .AddScoped<LoadoutLoader>()
			        .AddScoped<ShipLoader>()
			        .AddScoped<RetailProductLoader>()
			        .AddScoped<ShopLoader>();

			services.AddScoped<EntityParser>()
			        .AddScoped<DefaultLoadoutParser>()
			        .AddScoped<ManufacturerParser>()
			        .AddScoped<VehicleParser>();

			return services;
		}
	}
}
