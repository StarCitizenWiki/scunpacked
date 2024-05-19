using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using scdb.Xml.Entities;

namespace Loader
{
	public class EntityService
	{
		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		ConcurrentDictionary<string, string> classNameToFilenameMap;
		ConcurrentDictionary<string, string> referenceToClassNameMap;
		public ConcurrentDictionary<string, string> classNameToTypeMap;
		ConcurrentDictionary<string, EntityClassDefinition> classNameToEntityMap;
		ClassParser<EntityClassDefinition> entityParser = new ();
		bool verbose = true;

		// Avoid filenames that have these endings
		string[] file_avoids =
		{
			//"test",
			"template",
			"s42",
			"tow"
		};

		// Avoid these folders
		string[] folder_avoids =
		{
			"environments",
			"hangar",
			"holoui",
			"innerthought_dummies",
			"mission_entities",
			"missionstorage",
			"placeholder",
			"spawning",
			"starmarine",
			"template"
		};

		public void Initialise(bool rebuildCache)
		{
			var filenameCache = Path.Combine(DataRoot, "classFilenames-scunpacked.json");
			var referenceCache = Path.Combine(DataRoot, "classReferences-scunpacked.json");
			var typeCache = Path.Combine(DataRoot, "classTypes-scunpacked.json");

			classNameToEntityMap = new ConcurrentDictionary<string, EntityClassDefinition>();

			if (!rebuildCache && File.Exists(filenameCache) && File.Exists(referenceCache) && File.Exists(typeCache))
			{
				Console.WriteLine($"EntityService: Using the existing entity cache found in {DataRoot}");

				var filenameContents = File.ReadAllText(filenameCache);
				classNameToFilenameMap = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(filenameContents);

				var referenceContents = File.ReadAllText(referenceCache);
				referenceToClassNameMap = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(referenceContents);

				var typeContents = File.ReadAllText(typeCache);
				classNameToTypeMap = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(typeContents);
			}
			else
			{
				Console.WriteLine("EntityService: Building the entity cache, this takes about 15 minutes on my PC...");
				var timer = new System.Diagnostics.Stopwatch();
				timer.Start();

				classNameToFilenameMap = new ConcurrentDictionary<string, string>();
				referenceToClassNameMap = new ConcurrentDictionary<string, string>();
				classNameToTypeMap = new ConcurrentDictionary<string, string>();

				BuildItemDirectory(Path.Join("Data", "Libs", "Foundry", "Records", "entities", "scitem"));
				//BuildItemDirectory(@"Data\Libs\Foundry\Records\entities\scitem\ships");
				//BuildItemDirectory(@"Data\Libs\Foundry\Records\entities\scitem\ships\weapons");
				//BuildItemDirectory(@"Data\Libs\Foundry\Records\entities\scitem\weapons");

				File.WriteAllText(filenameCache, JsonConvert.SerializeObject(classNameToFilenameMap));
				File.WriteAllText(referenceCache, JsonConvert.SerializeObject(referenceToClassNameMap));
				File.WriteAllText(typeCache, JsonConvert.SerializeObject(classNameToTypeMap));

				timer.Stop();

				Console.WriteLine($"EntityService: Rebuilding the cache took {timer.Elapsed.TotalMinutes:n1} minutes");
				Console.WriteLine($"EntityService: Cache saved to input folder {DataRoot}");
			}
		}

		public EntityClassDefinition GetByClassName(string className)
		{
			if (classNameToEntityMap.ContainsKey(className)) return classNameToEntityMap[className];

			if (classNameToFilenameMap.ContainsKey(className))
			{
				var entity = LoadEntity(classNameToFilenameMap[className]);
				classNameToEntityMap.TryAdd(className, entity);
				return entity;
			}

			Console.WriteLine($"EntityService: Can't find the filename of the EntityClassDefinition for {className}");

			return null;
		}

		public EntityClassDefinition GetByReference(string reference)
		{
			if (!referenceToClassNameMap.ContainsKey(reference)) return null;
			return GetByClassName(referenceToClassNameMap[reference]);
		}

		public EntityClassDefinition GetByFilename(string filename)
		{
			var className = classNameToFilenameMap.FirstOrDefault(x => String.Equals(x.Value, filename, StringComparison.OrdinalIgnoreCase)).Key;
			if (className != null) return GetByClassName(className);

			var entity = LoadEntity(filename);
			if (entity == null) return null;

            if (!classNameToFilenameMap.ContainsKey(entity.ClassName)) {
                classNameToFilenameMap.TryAdd(entity.ClassName, filename);
                referenceToClassNameMap.TryAdd(entity.__ref, entity.ClassName);
                classNameToEntityMap.TryAdd(entity.ClassName, entity);
                classNameToTypeMap.TryAdd(entity.ClassName, entity.Components.SAttachableComponentParams?.AttachDef.Type ?? "");
            }

			return entity;
		}

		public IEnumerable<EntityClassDefinition> GetAll(string typeFilter)
		{
			var types = typeFilter?.Split(',') ?? new string[0];

			foreach (var pair in classNameToTypeMap)
			{
				if (types.Length > 0 && !types.Contains(pair.Value)) continue;

				yield return GetByClassName(pair.Key);
			}
		}

		void BuildItemDirectory(string folder)
		{
			Parallel.ForEach(
				Directory.EnumerateFiles(Path.Combine(DataRoot, folder), "*.xml", SearchOption.AllDirectories),
				filename =>
				{
					if (avoidFile(filename)) return;

					var entity = LoadEntity(filename);

					classNameToFilenameMap.TryAdd(entity.ClassName, filename);
					referenceToClassNameMap.TryAdd(entity.__ref, entity.ClassName);
					classNameToEntityMap.TryAdd(entity.ClassName, entity);
					classNameToTypeMap.TryAdd(entity.ClassName, entity.Components?.SAttachableComponentParams?.AttachDef.Type ?? "");
				});
		}

		EntityClassDefinition LoadEntity(string filename)
		{
			if (File.Exists(filename))
			{
				if (verbose) Console.WriteLine($"EntityService: Loading entity from {filename}");
				return entityParser.Parse(filename);
			}
			else
			{
				if (verbose) Console.WriteLine($"EntityService: Requested file {filename} does not exist");
				return null;
			}
		}

		bool avoidFile(string filename)
		{
			var fileSplit = Path.GetFileNameWithoutExtension(filename).Split('_');
			var avoidFile = fileSplit.Any(part => file_avoids.Contains(part));
			if (avoidFile) return true;

			var folderSplit = Path.GetDirectoryName(filename).Split('\\');
			var avoidFolder = folderSplit.Any(part => folder_avoids.Contains(part));
			if (avoidFolder) return true;

			return false;
		}
	}
}
