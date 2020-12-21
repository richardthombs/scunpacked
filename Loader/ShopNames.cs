using System;
using System.Collections.Generic;

namespace Loader
{
	public static class ShopNames
	{
		//
		// Note to contributors: please include the planet / moon / lagrange point name in the description.
		// Eg: "Jumptown, Yela" rather than just "Jumptown"
		//
		// Please sort by internal name
		//
		// Stanton1 = Hurston
		// Stanton1, LEO1 = Everus Harbor
		// Stanton1, L1 = HUR L1

		// Stanton2 = Crusader

		// Stanton3 = ArcCorp
		// Stanton3, LEO1 = Baijini Point

		// Stanton4 = Microtech
		//
		public static Dictionary<string, string> Lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "AdminOffice_Area18", "IO-North Tower, Area 18" },
			{ "AdminOffice_Grimhex", "Admin Office, GrimHEX" },
			{ "AdminOffice_Levski", "Admin Office, Levski" },
			{ "AdminOffice_Lorville", "L19 Admin Office, Lorville" },
			{ "AdminOffice_NewBabbage", "MT Planetary Services, New Babbage" },
			{ "AdminOffice_PortO", "Admin Office, Port Olisar" },
			{ "AdminOffice_Stanton1_L1", "Admin Office, HUR-L1" },
			{ "AdminOffice_Stanton1_L2", "Admin Office, HUR-L2" },
			{ "AdminOffice_Stanton1_L3", "Admin Office, HUR-L3" },
			{ "AdminOffice_Stanton1_L4", "Admin Office, HUR-L4" },
			{ "AdminOffice_Stanton1_L5", "Admin Office, HUR-L5" },
			{ "AdminOffice_Stanton1_LEO1", "Admin Office, Everus Harbor" },
			{ "AdminOffice_Stanton2_L1", "Admin Office, CRU-L1" },
			{ "AdminOffice_Stanton2_L4", "Shallow Fields Station Admin Office, CRU-L4 (removed)" },
			{ "AdminOffice_Stanton2_L5", "Admin Office, CRU-L5" },
			{ "AdminOffice_Stanton3_L1", "Admin Office, ARC-L1" },
			{ "AdminOffice_Stanton3_LEO1", "Admin Office, Baijini Point" },
			{ "AdminOffice_Stanton4_L1", "Admin Office, MIC-L1" },
			{ "AdminOffice_Stanton4_LEO1", "Admin Office, Port Tressler" },
			{ "AEGS", "IAE Expo Anniversary Sales - Aegis" },
			{ "ANVL", "IAE Expo Anniversary Sales - Anvil" },
			{ "Aparelli_NewBabbage", "Aparelli, New Babbage" },
			{ "APOA_BANU_ESPR", "IAE Expo Anniversary Sales - Aopoa, Banu, Esperia" },
			{ "arccorp_cluster_001_frost", "ArcCorp Mining Area 157, Yela" },
			{ "arccorp_cluster_001_sand", "ArcCorp Mining Area 141, Daymar" },
			{ "AstroArmada_Area18", "Astro Armada, Area 18" },
			{ "Bazaar_GadgetStand", "Grand Barter Gadget Stand, Levski" },
			{ "BestInShow", "IAE Expo Anniversary Sales - Best In Show" },
			{ "CafeMusain", "Cafe Musain, Levski" },
			{ "CargoOffice_ARC_LEO1", "Cargo Office, Baijini Point" },
			{ "CargoOffice_HUR_LEO1", "Cargo Office, Everus Harbor" },
			{ "CargoOffice_MIC_LEO1", "Cargo Office, Port Tressler" },
			{ "CasabaOutlet_Area18", "Casaba Outlet, Area 18" },
			{ "CasabaOutlet_PortO", "Casaba Outlet, Port Olisar" },
			{ "CasabaOutlet_Stanton1_L1", "Casaba Outlet, Hurston HUR-L1" },
			{ "CasabaOutlet_Stanton1_L2", "Casaba Outlet, Hurston HUR-L2" },
			{ "CasabaOutlet_Stanton1_L3", "Casaba Outlet, Hurston HUR-L3" },
			{ "CasabaOutlet_Stanton1_L4", "Casaba Outlet, Hurston HUR-L4" },
			{ "CasabaOutlet_Stanton1_L5", "Casaba Outlet, Hurston HUR-L5" },
			{ "CasabaOutlet_Stanton1_LEO1", "Casaba Outlet, Everus Harbour" },
			{ "CasabaOutlet_Stanton2_L1", "Casaba Outlet, CRU-L1" },
			{ "CasabaOutlet_Stanton2_L4", "Casaba Outlet, CRU-L4" },
			{ "CasabaOutlet_Stanton2_L5", "Casaba Outlet, CRU-L5" },
			{ "CasabaOutlet_Stanton3_L1", "Casaba Outlet, ARC-L1" },
			{ "CasabaOutlet_Stanton3_LEO1", "Casaba Outlet, Baijini Point" },
			{ "CasabaOutlet_Stanton4_L1", "Casaba Outlet, Microtech MIC-L1" },
			{ "CasabaOutlet_Stanton4_LEO1", "Casaba Outlet, Port Tressler" },
			{ "Centermass_Area18", "Centermass, Area 18" },
			{ "Centermass_NewBabbage", "Centermass, New Babbage" },
			{ "CNOU_ARGO_GRIN", "IAE Expo Anniversary Sales - Consolidated, Argo, Greycat" },
			{ "ConscientiousObjects_Levski", "Conscientious Objects, Levski" },
			{ "Cordrys_Levski", "Cordry's, Levski" },
			{ "CRUS_TMBL_KRIG", "IAE Expo Anniversary Sales - Crusader, Tumbril, Kruger" },
			{ "CubbyBlast_Area18", "Cubby Blast, Area 18"},
			{ "DRAK", "IAE Expo Anniversary Sales - Drake" },
			{ "drug_farm_001_frost", "Jumptown, Yela" },
			{ "DrugLab_Stanton3a_ParadiseCove", "Paradise Cove, Lyria" },
			{ "DumpersDepot_Area18", "Dumper's Depot, Area 18" },
			{ "DumpersDepot_Grimhex", "Dumper's Depot, GrimHEX" },
			{ "DumpersDepot_Grimhex_old", "Dumper's Depot, GrimHEX" },
			{ "DumpersDepot_Levski", "Dumper's Depot, Levski" },
			{ "DumpersDepot_Levski_old", "Dumper's Depot, Levski" },
			{ "DumpersDepot_PortO", "Dumper's Depot, Port Olisar" },
			{ "DumpersDepot_PortO_old", "Dumper's Depot, Port Olisar" },
			{ "FactoryLine_NewBabbage", "Factory Line, New Babbage" },
			{ "Fence_Junkyard_Stanton1_1", "Fence Junkyard" },
			{ "Fence_Junkyard_Stanton2b_1", "Brio's Breaker Yard" },
			{ "Fence_Junkyard_Stanton3b_1", "Samson & Son's Salvage Center" },
			{ "Fence_Junkyard_Stanton4c_1", "Devlin Scrap & Salvage" },
			{ "Fence_RestStop2_4", "Reclamation & Disposal Orinth, Hurston" },
			{ "Fence_RestStop2_5", "Locker Room, CRU-L5" },
			{ "GarciaGreens", "Garcia's Greens, New Babbage Double Bubble" },
			{ "GarrityDefense_PortO", "Garrity Defense, Port Olisar" },
			{ "Generic_FPSArmor_Stanton1_L1", "FPS Armour, HUR-L1" },
			{ "Generic_FPSArmor_Stanton1_LEO1", "FPS Armour, Everus Harbor" },
			{ "Generic_FPSArmor_Stanton2_L1", "FPS Armour, CRU-L1" },
			{ "Generic_FPSArmor_Stanton2_L4", "FPS Armour, CRU-L4" },
			{ "Generic_FPSArmor_Stanton3_L1", "FPS Armour, ARC-L1" },
			{ "Generic_FPSArmor_Stanton3_LEO1", "FPS Armour, Baijini Point" },
			{ "Generic_FPSArmor_Stanton4_L1", "FPS Armour, MIC-L1" },
			{ "Generic_FPSArmor_Stanton4_LEO1", "FPS Armour, Port Tressler" },
			{ "GiftShop_NewBabbage", "Gift Shop, New Babbage Spaceport" },
			{ "G-Loc", "G-Loc, Area 18 Plaza" },
			{ "HDShowcase_Lorville", "Hurston Dynamics Showcase, Lorville" },
			{ "indy_farmer_001_dust", "Gallete Family Farms, Cellin" },
			{ "indy_farmer_001_sand", "Bountiful Harvest Hydroponics, Daymar" },
			{ "indy_miner_001_dust", "Tram & Meyers Mining, Cellin" },
			{ "indy_miner_001_frost", "Benson Mining Outpost, Yela" },
			{ "indy_miner_001_sand", "Kudre Ore, Daymar" },
			{ "KCTrending_Grimhex", "KC Trending, GrimHEX" },
			{ "Klescher_Aberdeen", "Klescher Prison Commissary, Aberdeen" },
			{ "Klescher_Aberdeen_Commissary", "Klescher Prison Commissary, Aberdeen" },
			{ "LandingServices_Area18", "Landing Services, Area 18" },
			{ "LandingServices_GrimHex", "Landing Services, GrimHEX" },
			{ "LandingServices_Levski", "Landing Services, Levski" },
			{ "LandingServices_Lorville", "Landing Services, Lorville" },
			{ "LandingServices_Olisar", "Landing Services, Port Olisar" },
			{ "LandingServices_Stanton1_L1", "Landing Services, HUR-L1" },
			{ "LandingServices_Stanton1_L2", "Landing Services, HUR-L2" },
			{ "LandingServices_Stanton1_L3", "Landing Services, HUR-L3" },
			{ "LandingServices_Stanton1_L4", "Landing Services, HUR-L4" },
			{ "LandingServices_Stanton1_L5", "Landing Services, HUR-L5" },
			{ "LandingServices_Stanton2_L1", "Landing Services, CRU-L1" },
			{ "LandingServices_Stanton2_L4", "Landing Services, CRU-L4" },
			{ "LandingServices_Stanton2_L5", "Landing Services, CRU-L5" },
			{ "LandingServices_Stanton3_L1", "Landing Services, ARC-L1" },
			{ "LivefireWeapons_PortO","Live Fire Weapons, Port Olisar" },
			{ "LivefireWeapons_RestStop","Live Fire Weapons, MIC-L1" },
			{ "LivefireWeapons_Stanton1_L1","Live Fire Weapons, HUR-L1" },
			{ "LivefireWeapons_Stanton1_L2","Live Fire Weapons, HUR-L2" },
			{ "LivefireWeapons_Stanton1_L3","Live Fire Weapons, HUR-L3" },
			{ "LivefireWeapons_Stanton1_L4","Live Fire Weapons, HUR-L4" },
			{ "LivefireWeapons_Stanton1_L5","Live Fire Weapons, HUR-L5" },
			{ "LivefireWeapons_Stanton2_L1","Live Fire Weapons, CRU-L1" },
			{ "LivefireWeapons_Stanton2_L4","Live Fire Weapons, CRU-L4" },
			{ "LivefireWeapons_Stanton2_L5","Live Fire Weapons, CRU-L5" },
			{ "LivefireWeapons_Stanton3_L1","Live Fire Weapons, ARC-L1" },
			{ "Market_ClothingStand_Levski", "Grand Barter Clothing Stand, Levski" },
			{ "MiningKiosks_Area18", "Refining Terminal, Area 18" },
			{ "MiningKiosks_GrimHex", "Refining Terminal, GrimHEX" },
			{ "MiningKiosks_Levski", "Mining Kiosks, Levski" },
			{ "MiningKiosks_Lorville", "Mining Kiosks, Lorville" },
			{ "MiningKiosks_NewBabbage", "Mining Kiosks, New Babbage" },
			{ "MiningKiosks_Olisar", "Mining Kiosks, Port Olisar" },
			{ "MISC	IAE Expo", "Anniversary Sales - MISC" },
			{ "MVBar", "M&V Bar, Lorville Workers DistrictX" },
			{ "NewDeal_Lorville", "New Deal, Lorville" },
			{ "Old38", "Old '38, GrimHEX" },
			{ "OmegaPro_NewBabbage", "Omega Pro, New Babbage" },
			{ "ORIG", "IAE Expo Anniversary Sales - Origin" },
			{ "PlatinumBay_Stanton1_L1", "Platinum Bay, HUR-L1" },
			{ "PlatinumBay_Stanton1_L2", "Platinum Bay, HUR-L2" },
			{ "PlatinumBay_Stanton1_L3", "Platinum Bay, HUR-L3" },
			{ "PlatinumBay_Stanton1_L4", "Platinum Bay, HUR-L4" },
			{ "PlatinumBay_Stanton1_L5", "Platinum Bay, HUR-L5" },
			{ "PlatinumBay_Stanton1_LEO1", "Platinum Bay, Everus Harbor" },
			{ "PlatinumBay_Stanton2_L1", "Platinum Bay, CRU-L1" },
			{ "PlatinumBay_Stanton2_L4", "Platinum Bay, CRU-L4" },
			{ "PlatinumBay_Stanton2_L5", "Platinum Bay, CRU-L5" },
			{ "PlatinumBay_Stanton3_L1", "Platinum Bay, ARC-L1" },
			{ "PlatinumBay_Stanton3_LEO1", "Platinum Bay, Baijini Point" },
			{ "PlatinumBay_Stanton4_L1", "Platinum Bay, MIC-L1" },
			{ "PlatinumBay_Stanton4_LEO1", "Platinum Bay, Port Tressler" },
			{ "RaceBar_GrimHEX", "Racing bar, GrimHEX" },
			{ "rayari_cluster_001_dust", "Hickes Research Outpost, Cellin" },
			{ "rayari_cluster_001_frost", "Deakins Research Outpost, Yela" },
			{ "RegalLuxury_NewBabbage", "Regal Luxury Rentals, New Babbage" },
			{ "RSI", "IAE Expo Anniversary Sales - RSI" },
			{ "ShipWeapons_Generic_Stanton1_L2", "Ship Weapons, HUR-L2" },
			{ "ShipWeapons_Generic_Stanton1_L3", "Ship Weapons, HUR-L3" },
			{ "ShipWeapons_Generic_Stanton1_L4", "Ship Weapons, HUR-L4" },
			{ "ShipWeapons_Generic_Stanton1_L5", "Ship Weapons, HUR-L5" },
			{ "ShipWeapons_Generic_Stanton2_L5", "Ship Weapons, CRU-L5" },
			{ "shubin_cluster_001_sand", "Shubin Mining Facility SCD-1, Daymar" },
			{ "ShubinInterstellar_NewBabbage", "Shubin Interstellar, New Babbage" },
			{ "Skutters_Armor_Weap_GrimHex", "Skutters, GrimHEX" },
			{ "Skutters_Food_GrimHex", "Skutters, GrimHEX" },
			{ "stanton_1_hrst_001", "HDMS Edmond, Hurston" },
			{ "stanton_1_hrst_002", "HDMS Oparei, Hurston" },
			{ "stanton_1_hrst_003", "HDMS Pinewood, Hurston" },
			{ "stanton_1_hrst_004", "HDMS Thedus, Hurston" },
			{ "stanton_1_hrst_005", "HDMS Hadley, Hurston" },
			{ "stanton_1_hrst_006", "HDMS Stanhope, Hurston" },
			{ "stanton_1a_hrst_001", "HDMS Bezdek, Arial" },
			{ "stanton_1a_hrst_002", "HDMS Lathan, Arial" },
			{ "stanton_1b_hrst_001", "HDMS Norgaard, Aberdeen" },
			{ "stanton_1b_hrst_002", "HDMS Anderson, Aberdeen" },
			{ "stanton_1c_hrst_001", "HDMS Hahn, Magda" },
			{ "stanton_1c_hrst_002", "HMDS Perlman, Magda" },
			{ "stanton_1d_hrst_001", "HDMS Woodruff, Ita" },
			{ "stanton_1d_hrst_002", "HDMS Ryder, Ita" },
			{ "stanton_3a_indy_humboldt", "Humbolt Mines, Lyria" },
			{ "stanton_3a_indy_loveridge", "Loveridge Mineral Reserve, Lyria" },
			{ "stanton_3a_shubin_sal2", "Shubin Mining Facility SAL-2, Lyria" },
			{ "stanton_3a_shubin_sal5", "Shubin Mining Facility SAL-5, Lyria" },
			{ "stanton_3b_arccorp_area045", "ArcCorp mining Area 45, Wala" },
			{ "stanton_3b_arccorp_area048", "ArcCorp mining Area 48, Wala" },
			{ "stanton_3b_arccorp_area056", "ArcCorp mining Area 56, Wala" },
			{ "stanton_3b_arccorp_area061", "ArcCorp mining Area 61, Wala" },
			{ "stanton_3b_indyFarm_001", "Shady Glen Farms" },
			{ "stanton_4_rayari_001", "Rayari Deltana Research Outpost, Microtech" },
			{ "stanton_4_shubin_003", "Shubin Mining Facility SMO-18, Microtech" },
			{ "stanton_4_shubin_004", "Shubin Mining Facility SMO-13, Microtech" },
			{ "stanton_4a_drugfarm_001", "Raven's Roost, Calliope" },
			{ "stanton_4a_rayari_001", "Rayari Anvik Research Outpost, Calliope" },
			{ "stanton_4a_rayari_002", "Rayari Kaltag Research Outpost, Calliope" },
			{ "stanton_4a_shubin_001", "Shubin Mining Facility SMCa-6, Calliope" },
			{ "stanton_4a_shubin_002", "Shubin Mining Facility SMCa-8, Calliope" },
			{ "stanton_4b_rayari_001", "Rayari Cantwell Research Outpost, Clio" },
			{ "stanton_4b_rayari_002", "Rayari McGrath Research Outpost, Clio" },
			{ "stanton_4c_indyFarm_001", "Bud's Growery, Euterpe" },
			{ "StashHouse_Stanton2a_PrivateProperty", "Private Property, Cellin" },
			{ "StashHouse_Stanton2b_NuenWaste", "Nuen Waste Management, Daymar" },
			{ "StashHouse_Stanton2c_NT999XX", "NT-999-XX, Yela" },
			{ "StashHouse_Stanton3a_Orphanage", "The Orphanage, Lyria" },
			{ "StashHouse_Stanton4", "Cellin Stash House" },
			{ "t_mills_cluster_001_dust", "Terra Mills Hydro Farm, Terra" },
			{ "TammanyAndSons_Lorville", "Tammany & Sons, Lorville" },
			{ "TDD_Area18", "Trade & Development Division, Area 18" },
			{ "TDD_NewBabbage", "Trade & Development Division, New Babbage" },
			{ "TeachsShipShop_Levski", "Teach's Ship Shop, Levski" },
			{ "Technotic", "Technotic, GrimHEX" },
			{ "Transfers_Lorville", "Transfers Room, Central Business District, Lorville" },
			{ "TravelerRentals_Area18", "Traveler Rentals, Area 18" },
			{ "Twyns", "Twyn's, New Babbage Double Bubble" },
			{ "VantageRentals_Lorville", "Vantage Rentals, Lorville" },
			{ "Wallys", "Wally's, New Babbage Plaza" },
			{ "Weapons_Items_Armors", "IAE Expo Anniversary Sales - Ship Weapons, Items, Armour" },
			{ "Whammers", "Whammer's, New Babbage Plaza" }
		};
	}

	public class MapEntry
	{
		public string Name;
		public List<MapEntry> OrbitedBy;
		public List<MapEntry> Destinations;
		public List<MapEntry> Shops;
	}

	public static class Map
	{
		public static MapEntry TheUniverse = new MapEntry
		{
			Name = "Stanton",
			OrbitedBy = new List<MapEntry>
			{
				new MapEntry
				{
					Name = "Hurston",
					OrbitedBy = new List<MapEntry>
					{
						new MapEntry { Name = "Arial" },
						new MapEntry { Name = "Magda" },
						new MapEntry { Name = "Aberdeen" },
						new MapEntry { Name = "Ita" }
					}
				},
				new MapEntry
				{
					Name = "HUR L3",
					Destinations = new List<MapEntry>
					{
						new MapEntry
						{
							Name = "R&R",
							Shops = new List<MapEntry>
							{
								new MapEntry { Name = "Casaba Outlet "}
							}
						}
					}
				},
				new MapEntry { Name = "HUR L4" },
				new MapEntry { Name = "HUR L5" },

				new MapEntry
				{
					Name = "Crusader",
					OrbitedBy = new List<MapEntry>
					{
						new MapEntry
						{
							Name = "Port Olisar",
							Shops = new List<MapEntry>
							{
								new MapEntry { Name = "Casaba Outlet" },
								new MapEntry { Name = "Admin Office" }
							}
						},
						new MapEntry
						{
							Name = "Cellin",
							OrbitedBy = new List<MapEntry>
							{
								new MapEntry { Name = "Security Post Kareah" }
							}
						},
						new MapEntry
						{
							Name = "Daymar",
							OrbitedBy = new List<MapEntry>
							{
								new MapEntry { Name = "Covalex" }
							}
						},
						new MapEntry
						{
							Name = "Yela",
							OrbitedBy = new List<MapEntry>
							{
								new MapEntry { Name = "GrimHEX" }
							}
						}
					},
				},
				new MapEntry { Name = "CRU L3" },
				new MapEntry { Name = "CRU L4" },
				new MapEntry { Name = "CRU L5" },

				new MapEntry
				{
					Name = "ArcCorp",
					OrbitedBy = new List<MapEntry>
					{
						new MapEntry { Name = "Wala" },
						new MapEntry { Name = "Lyria" }
					},
					Destinations = new List<MapEntry>
					{
						new MapEntry
						{
							Name ="Area 18",
							Shops = new List<MapEntry>
							{
								new MapEntry { Name = "Casaba Outlet "}
							}
						}
					}
				},
				new MapEntry { Name = "ARC L3" },
				new MapEntry { Name = "ARC L4" },
				new MapEntry { Name = "ARC L5" }
			},
		};
	}
}
