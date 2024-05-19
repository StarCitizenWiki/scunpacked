using System;
using System.Collections.Concurrent;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using scdb.Xml.Entities;

namespace Loader
{
	public class ItemLoader
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		private readonly ItemBuilder _itemBuilder;
		private readonly ManufacturerService _manufacturerSvc;
		private readonly ItemClassifier _itemClassifier;
		private readonly EntityService _entitySvc;
		private readonly AmmoService _ammoSvc;
		private readonly ItemInstaller _itemInstaller;
		private readonly LoadoutLoader _loadoutLoader;
		private readonly InventoryContainerService _inventoryContainerSvc;
		private readonly MeleeCombatService _meleeConfigSvc;
		private readonly bool _doV2Style;

		// Don't dump items with these types
		private readonly string[] _typeAvoids =
		[
			"UNDEFINED",
			"airtrafficcontroller",
			"button",
			"char_body",
			"char_head",
			"char_hair_color",
			"char_head_eyebrow",
			"char_head_eyelash",
			"char_head_eyes",
			"char_head_hair",
			"char_lens",
			"char_skin_color",
			"cloth",
			"debris",
			"flair_floor",
			"flair_surface",
			"flair_wall",
			"removablechip",
		];

		public ItemLoader(ItemBuilder itemBuilder, ManufacturerService manufacturerSvc, EntityService entitySvc, AmmoService ammoSvc, ItemInstaller itemInstaller, LoadoutLoader loadoutLoader, InventoryContainerService inventoryContainerSvc, MeleeCombatService meleeConfigSvc, bool doV2Style)
		{
			_itemBuilder = itemBuilder;
			_manufacturerSvc = manufacturerSvc;
			_itemClassifier = new ItemClassifier();
			_entitySvc = entitySvc;
			_ammoSvc = ammoSvc;
			_itemInstaller = itemInstaller;
			_loadoutLoader = loadoutLoader;
			_meleeConfigSvc = meleeConfigSvc;
			_inventoryContainerSvc = inventoryContainerSvc;
			_doV2Style = doV2Style;
		}

		public ConcurrentBag<ItemIndexEntry> Load(string typeFilter = null)
		{
			Directory.CreateDirectory(Path.Combine(OutputFolder, "items"));
			if (_doV2Style)
			{
				Directory.CreateDirectory(Path.Combine(OutputFolder, "v2", "items"));
			}

			var damageResistanceMacros = LoadDamageResistanceMacros();

			Console.WriteLine($"ItemLoader: Creating index...");
			var index = CreateIndex(typeFilter);

			// Once all the items have been loaded, we have to spin through them again looking for
			// any that use ammunition magazines so we can load the magazine and then load the ammunition it uses
			Console.WriteLine($"ItemLoader: Creating {index.Count} item files...");
			foreach (var item in index)
			{
				var entity = _entitySvc.GetByClassName(item.className);

				// If uses an ammunition magazine, then load it
				EntityClassDefinition magazine = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemWeaponComponentParams?.ammoContainerRecord))
				{
					magazine = _entitySvc.GetByReference(entity.Components.SCItemWeaponComponentParams.ammoContainerRecord);
				}

				// If it is an ammo container or if it has a magazine then load the ammo properties
				AmmoParams ammoEntry = null;
				var ammoRef = magazine?.Components?.SAmmoContainerComponentParams?.ammoParamsRecord ?? entity.Components?.SAmmoContainerComponentParams?.ammoParamsRecord;
				if (!String.IsNullOrEmpty(ammoRef))
				{
					ammoEntry = _ammoSvc.GetByReference(ammoRef);
				}

				MeleeCombatConfig combatConfig = null;
				var combatRef = entity?.Components?.SMeleeWeaponComponentParams?.meleeCombatConfig;
				if (!String.IsNullOrEmpty(combatRef))
				{
					combatConfig = _meleeConfigSvc.GetByReference(combatRef);
				}

				DamageResistance damageResistances = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemSuitArmorParams?.damageResistance))
				{
					var damageMacro = damageResistanceMacros.Find(y => y.__ref == entity.Components.SCItemSuitArmorParams.damageResistance);
					damageResistances = damageMacro?.damageResistance;
				}

				StandardisedInventoryContainer inventoryContainer = null;
				if (!String.IsNullOrEmpty(entity.Components?.SCItemInventoryContainerComponentParams?.containerParams))
				{
					inventoryContainer = _inventoryContainerSvc.GetInventoryContainer(entity.Components.SCItemInventoryContainerComponentParams.containerParams);
				}

				var stdItem = _itemBuilder.BuildItem(entity);
				var loadout = _loadoutLoader.Load(entity);
				_itemInstaller.InstallLoadout(stdItem, loadout);
				_itemInstaller.InstallLoadout(entity, loadout);

				stdItem.Classification = item.classification;
				item.stdItem = stdItem;

				if (_doV2Style)
				{
					File.WriteAllText(Path.Combine(OutputFolder, "v2", "items", $"{entity.ClassName.ToLower()}.json"), JsonConvert.SerializeObject(stdItem));
					File.WriteAllText(Path.Combine(OutputFolder, "v2", "items", $"{entity.ClassName.ToLower()}-raw.json"), JsonConvert.SerializeObject(entity));
				}

				// Write the JSON of this entity to its own file
				var jsonFilename = Path.Combine(OutputFolder, "items", $"{entity.ClassName.ToLower()}.json");
				var json = JsonConvert.SerializeObject(new
				{
					Raw = new
					{
						Entity = entity,
					},
					damageResistances,
					inventoryContainer,
					combatConfig,
					magazine,
					ammo = ammoEntry,
				});
				File.WriteAllText(jsonFilename, json);
			}

			File.WriteAllText(Path.Combine(OutputFolder, "items.json"), JsonConvert.SerializeObject(index));

			// Create an index file for each different item type
			var typeIndicies = new Dictionary<string, List<ItemIndexEntry>>();
			foreach (var entry in index)
			{
				if (String.IsNullOrEmpty(entry.classification)) continue;

				var type = entry.classification.Split('.')[0];
				if (!typeIndicies.ContainsKey(type)) typeIndicies.Add(type, new List<ItemIndexEntry>());
				var typeIndex = typeIndicies[type];
				typeIndex.Add(entry);
			}
			foreach (var pair in typeIndicies)
			{
				File.WriteAllText(Path.Combine(OutputFolder, pair.Key.ToLower() + "-items.json"), JsonConvert.SerializeObject(pair.Value));
			}

			return index;
		}

		private List<DamageResistanceMacro> LoadDamageResistanceMacros()
		{
			var damageResistanceMacroFolder = Path.Join("Data", "Libs", "Foundry", "Records", "damage");
			var damageResistanceMacros = new List<DamageResistanceMacro>();

			foreach (var damageMacroFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, damageResistanceMacroFolder), "*.xml", SearchOption.AllDirectories))
			{
				var damageResistanceMacroParser = new DamageResistanceMacroParser();
				DamageResistanceMacro entity = damageResistanceMacroParser.Parse(damageMacroFilename);
				if (entity == null) continue;

				damageResistanceMacros.Add(entity);
			}

			return damageResistanceMacros;
		}

		ConcurrentBag<ItemIndexEntry> CreateIndex(string typeFilter)
		{
			var timer = new System.Diagnostics.Stopwatch();
			timer.Start();
			var index = new ConcurrentBag<ItemIndexEntry> ();
			var types = typeFilter?.Split(',') ?? new string[0];

			Parallel.ForEach(_entitySvc.classNameToTypeMap, entity =>
			{
				if (types.Length > 0 && !types.Contains(entity.Value)) return;

				var entityDef = _entitySvc.GetByClassName(entity.Key);
				if (AvoidType(entityDef.Components?.SAttachableComponentParams?.AttachDef.Type)) return;
				var indexEntry = CreateIndexEntry(entityDef);

				// Add it to the item index
				index.Add(indexEntry);
			});

			timer.Stop();

			Console.WriteLine($"ItemLoader: Loading items took {timer.Elapsed.TotalMinutes:n1} minutes");

			return index;
		}

		ItemIndexEntry CreateIndexEntry(EntityClassDefinition entity)
		{
			var classification = _itemClassifier.Classify(entity);

			var indexEntry = new ItemIndexEntry
			{
				className = entity.ClassName,
				reference = entity.__ref,
				itemName = entity.ClassName.ToLower(),
				type = entity.Components?.SAttachableComponentParams?.AttachDef.Type,
				subType = entity.Components?.SAttachableComponentParams?.AttachDef.SubType,
				size = entity.Components?.SAttachableComponentParams?.AttachDef.Size,
				grade = entity.Components?.SAttachableComponentParams?.AttachDef.Grade,
				name = entity.Components?.SAttachableComponentParams?.AttachDef.Localization.Name,
				tags = entity.Components?.SAttachableComponentParams?.AttachDef.Tags,
				manufacturer = _manufacturerSvc.GetManufacturer(entity.Components?.SAttachableComponentParams?.AttachDef.Manufacturer, entity.ClassName)?.Code,
				classification = classification
			};
			return indexEntry;
		}

		private bool AvoidType(string type)
		{
			if (type == null) return true;
			return _typeAvoids.Contains(type, StringComparer.OrdinalIgnoreCase);
		}
	}
}
