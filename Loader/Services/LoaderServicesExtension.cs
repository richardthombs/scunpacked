using Loader.Entries;
using Loader.Helper;
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
					.AddSingleton<LoaderService<Ship>, ShipsService>()
					.AddSingleton<LoaderService<CommodityTypeAndSubType>, CommodityTypeService>()
					.AddSingleton<LoaderService<Commodity>, CommodityService>();

			services.AddScoped<ItemLoader>()
			        .AddScoped<ManufacturerLoader>()
			        .AddScoped<LoadoutLoader>()
			        .AddScoped<ShipLoader>()
			        .AddScoped<RetailProductLoader>()
					.AddScoped<CommodityTypeLoader>()
					.AddScoped<CommodityLoader>()
			        .AddScoped<ShopLoader>();

			services.AddScoped<EntityParser>()
			        .AddScoped<DefaultLoadoutParser>()
			        .AddScoped<ManufacturerParser>()
			        .AddScoped<VehicleParser>();

			services.AddScoped<IJsonFileReaderWriter, JsonFileReaderWriter>();

			return services;
		}
	}
}
