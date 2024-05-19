using System;
using System.Collections.Generic;
using System.IO;

using NDesk.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Loader
{
	class Program
	{
		static void Main(string[] args)
		{
			string scDataRoot = null;
			string outputRoot = null;
			var doShips = true;
			var doItems = true;
			var doShops = true;
			var doStarmap = true;
			var doMissions = true;
			var noCache = false;
			var doV2Style = true;
			string typeFilter = null;
			string shipFilter = null;

			var p = new OptionSet
			{
				{ "scdata=", v => scDataRoot = v },
				{ "input=",  v => scDataRoot = v },
				{ "output=",  v => outputRoot = v },
				{ "noships", _ => doShips = false },
				{ "noitems", _ => doItems = false },
				{ "noshops", _ => doShops = false },
				{ "nomap", _ => doStarmap = false },
				{ "noV2", _ => doV2Style = false },
				{ "nomissions", _ => doMissions = false },
				{ "nocache", _ => noCache = true },
				{ "types=", v => typeFilter = v },
				{ "ships=", v=> shipFilter = v }
			};

			var extra = p.Parse(args);

			var badArgs = extra.Count > 0;
			if (badArgs)
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("    Loader.exe -input=<path to extracted Star Citizen data> -output=<path to JSON output folder>");
				Console.WriteLine();
				return;
			}

			var timer = new System.Diagnostics.Stopwatch();
			timer.Start();

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore,
				Converters = new List<JsonConverter> { new StringEnumConverter() }
			};

			bool incremental = !doShips || !doItems || !doShops || !doStarmap;

			// Prep the output folder
			if (Directory.Exists(outputRoot) && !incremental)
			{
				var info = new DirectoryInfo(outputRoot);
				foreach (var file in info.GetFiles()) file.Delete();
				foreach (var dir in info.GetDirectories()) dir.Delete(true);
			}
			else Directory.CreateDirectory(outputRoot);

			var entitySvc = new EntityService
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			entitySvc.Initialise(noCache);

			// Localisation
			Console.WriteLine("Load Localisation");
			var labelLoader = new LabelsLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var labels = labelLoader.Load("english");
			var localisationSvc = new LocalisationService(labels);

			// Manufacturers
			Console.WriteLine("Load Manufacturers");
			var manufacturerLoader = new ManufacturerLoader(localisationSvc)
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var manufacturerIndex = manufacturerLoader.Load();
			var manufacturerSvc = new ManufacturerService(manufacturerIndex);

			// Loot
			Console.WriteLine("Load Loot Archetypes");
			var lootLoader = new LootLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var lootArchetypes = lootLoader.LoadArchetypes();
			var lootTables = lootLoader.LoadTables();
			var lootSvc = new LootService(lootArchetypes, lootTables);

			// Factions
			Console.WriteLine("Load Factions");
			var factionLoader = new FactionLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot,
				locService = localisationSvc
			};
			var factions = factionLoader.LoadFactions();

			// Ammunition
			Console.WriteLine("Load Ammunition");
			var ammoLoader = new AmmoLoader
			{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};
			var ammoIndex = ammoLoader.Load();
			var ammoSvc = new AmmoService(ammoIndex);

			// Insurance
			Console.WriteLine("Load Insurance");
			// var insuranceLoader = new InsuranceLoader()
			// {
			// 	DataRoot = scDataRoot
			// };
			// //var insurancePrices = insuranceLoader.Load();
			// //var insuranceSvc = new InsuranceService(insurancePrices);
			// var insuranceSvc = new InsuranceService(null);


			// Missions
			if (doMissions)
			{
				Console.WriteLine("Load Missions");
				var missionLoader = new MissionLoader
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
					locService = localisationSvc
				};
				var missions = missionLoader.LoadMissions();
				missionLoader.LoadMissionTypes();
				missionLoader.LoadMissionGiver();

				var rewardsLoader = new RewardLoader
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
					locService = localisationSvc
				};
				rewardsLoader.LoadRewards();
				rewardsLoader.LoadStandings();
				rewardsLoader.LoadScopes();
			}

			// PersonalInventories
			Console.WriteLine("Load PersonalInventories");
			var inventoryLoader = new InventoryContainerLoader()
			{
				DataRoot = scDataRoot
			};
			var inventoryIndex = inventoryLoader.Load();
			var inventorySvc = new InventoryContainerService(inventoryIndex);

			var xmlLoadoutLoader = new XmlLoadoutLoader { DataRoot = scDataRoot };
			var manualLoadoutLoader = new ManualLoadoutLoader();
			var loadoutLoader = new LoadoutLoader(xmlLoadoutLoader, manualLoadoutLoader);
			var itemBuilder = new ItemBuilder(localisationSvc, manufacturerSvc, ammoSvc, entitySvc, inventorySvc);
			var itemInstaller = new ItemInstaller(entitySvc, loadoutLoader, itemBuilder);
			var meleeLoader = new MeleeCombatLoader{
				OutputFolder = outputRoot,
				DataRoot = scDataRoot
			};;
			var meleeConfigSvc = new MeleeCombatService(meleeLoader.Load());

			// Items
			if (doItems)
			{
				Console.WriteLine("Load Items");
				var itemLoader = new ItemLoader(itemBuilder, manufacturerSvc, entitySvc, ammoSvc, itemInstaller, loadoutLoader, inventorySvc, meleeConfigSvc, doV2Style)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
				};
				itemLoader.Load(typeFilter);
			}

			// Ships and vehicles
			if (doShips)
			{
				Console.WriteLine("Load Ships and Vehicles");
				var shipLoader = new ShipLoader(itemBuilder, manufacturerSvc, localisationSvc, entitySvc, itemInstaller, loadoutLoader, inventorySvc, doV2Style)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot,
				};
				shipLoader.Load(shipFilter);
			}

			// Prices
			if (doShops)
			{
				Console.WriteLine("Load Shops");
				var shopLoader = new ShopLoader(localisationSvc, entitySvc)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				shopLoader.Load();
			}

			// Starmap
			if (doStarmap)
			{
				Console.WriteLine("Load Starmap");
				var starmapLoader = new StarmapLoader(localisationSvc)
				{
					OutputFolder = outputRoot,
					DataRoot = scDataRoot
				};
				starmapLoader.Load();
			}

			timer.Stop();

			Console.WriteLine($"Finished! Took {timer.Elapsed.TotalMinutes:n1} minutes");
		}
	}
}
