using Loader.Entries;
using Loader.Loader;
using Microsoft.Extensions.DependencyInjection;

namespace Loader.Services
{
	public static class LoaderServicesExtension
	{
		public static IServiceCollection AddLoaderServices(this IServiceCollection services)
		{
			services.AddScoped<TranslationService>()
			        .AddScoped<LoadoutLoader>()
			        .AddScoped<LocalisationService>()
			        .AddScoped<LoaderService<Manufacturer>, ManufacturersService>()
			        .AddScoped<LoaderService<Shop>, ShopService>()
			        .AddScoped<LoaderService<Item>, ItemsService>()
			        .AddScoped<LoaderService<Ship>, ShipsService>();

			return services;
		}
	}
}
