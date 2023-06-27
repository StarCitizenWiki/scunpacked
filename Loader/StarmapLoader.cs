using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Loader
{
	public class StarmapLoader
	{
		struct EntryTuple
		{
			public string ID;
			public string Name;
			public int Count;
		}
		struct IDTuple
		{
			public string Layer0ID;
			public string StarmapID;
		}

		public string OutputFolder { get; set; }
		public string DataRoot { get; set; }

		LocalisationService localisationService;
		bool verbose = false;

		public StarmapLoader(LocalisationService localisationService)
		{
			this.localisationService = localisationService;
		}

		public List<StarmapIndexEntry> Load()
		{
			var index = new List<StarmapIndexEntry>();
			index.AddRange(Load(@"Data\Libs\Foundry\Records\starmap\pu"));

			File.WriteAllText(Path.Combine(OutputFolder, "starmap.json"), JsonConvert.SerializeObject(index));

			return index;
		}

		List<StarmapIndexEntry> Load(string entityFolder)
		{
			var index = new List<StarmapIndexEntry>();

			var parser = new StarmapParser();

			foreach (var entityFilename in Directory.EnumerateFiles(Path.Combine(DataRoot, entityFolder), "*.xml",
				         SearchOption.AllDirectories))
			{
				if (verbose) Console.WriteLine($"StarmapLoader: {entityFilename}");

				var entity = parser.Parse(entityFilename);
				if (entity == null) continue;

				var indexEntry = new StarmapIndexEntry
				{
					name = localisationService.GetText(entity.name),
					description = localisationService.GetText(entity.description),
					callout1 = localisationService.GetText(entity.callout1),
					callout2 = localisationService.GetText(entity.callout2),
					callout3 = localisationService.GetText(entity.callout3),
					type = entity.type,
					navIcon = entity.navIcon,
					hideInStarmap = entity.hideInStarmap,
					jurisdiction = entity.jurisdiction,
					parent = entity.parent,
					size = entity.size,
					reference = entity.__ref,
					path = entity.__path
				};

				index.Add(indexEntry);
			}

			index = AddLayer0(index);

			index = FixIDs(index);

			return index;
		}

		List<StarmapIndexEntry> AddLayer0(List<StarmapIndexEntry> starmap)
		{
			var layer = new Layer0Parser()
				.GetLayers(Path.Combine(DataRoot,
					@"Data\Libs\Subsumption\Platforms\PU\System\Stanton\stantonsystem\Layer0.xml"));

			var variables = layer.Variables;

			var nameMap = new List<EntryTuple>();
			var nameCount = new Dictionary<string, int>();

			foreach (var variable in variables)
			{
				if (variable.EditorName == null) continue;

				if (variable.ID.Equals("4a269932-183e-362d-aacc-188e7debbea9,4cccecbb-9ecd-4b4b-915a-7c58ae41b6b0,4044f14e-6a03-5d6b-4a5e-0f2f6f898da9,4c08201d-be0e-69d2-88c3-02a0f4bbb5b0,468acbad-45c6-4ce8-14dc-8723c8356c89,4b1e8901-8240-f80f-502b-cfddc7551ba1,4fd16a56-cfa0-a078-dd78-52d7e1b3f7ab"))
					variable.EditorName = "REMOVE";

				if (!nameCount.ContainsKey(variable.EditorName))
				{
					nameCount.Add(variable.EditorName, 1);
				}
				else
				{
					nameCount[variable.EditorName] += 1;
				}

				nameMap.Add(new EntryTuple
				{
					ID = variable.DefaultValue ?? variable.ID,// parts.Last(),
					Name = variable.EditorName,
				});

			}

			foreach (var variable in variables)
			{
				var parts = new List<string>(variable.ID.Split(','));

				string parent = null;
				if (parts.Count > 1)
				{
					do
					{
						parts.RemoveAt(parts.Count - 1);
						parent = String.Join(",", parts);
					} while (!nameMap.Exists(x => x.ID == parent) || nameMap.Find(x => x.ID == parent).Name ==variable.EditorName);
				}
				//var parent = variable.ID;

				var name = variable.EditorName;
				if (name == null)
				{
					var entry = nameMap.FirstOrDefault(entry => entry.ID == (variable.DefaultValue ?? variable.ID)); // parts.Last()
					if (!entry.Equals(default(EntryTuple)))
					{
						name = entry.Name;
					}
				}

				if (String.IsNullOrEmpty(name)) continue;

				//...
				if (name == "Ariel") name = "Arial";
				//if (name == "RayariDeltanaResearch") name = "Rayari Deltana Research Outpost";

				var starmapEntry =
					// Name Matches
					starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.name.Trim().ToLower().Equals(name.ToLower())) ??
					// Name Matches without Spaces
					starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.name.Trim().ToLower().Replace(" ", "").Equals(name.Replace("_", "").ToLower())) ??
					// Lagrange Point Match
					starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.name.Contains("-L") && entry.name.Trim().ToLower().Replace(" ", "").Contains(name.Replace("_", "").ToLower())) ??
					// Shubin Mining SM Match
					starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.name.Contains("SM0-") && entry.name.Replace(" ", "").Trim().ToLower().Contains(name.Replace("_", "-").ToLower())) ??
					// Shubin Mining SAL- Match
					starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.name.Contains("SAL-") && entry.name.Replace(" ", "").Trim().ToLower().Contains(name.Replace("_", "-").ToLower())) ??
					// Outpost Suffix
					starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.name.Trim().ToLower().Replace(" ", "").Equals(name.Replace("_", "").ToLower() + "outpost")) ??
					// Patch Check
				    starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.path.Contains(name.Replace("_", "").ToLower())) ??
					// Patch Check 2
				    starmap.Find(entry => !String.IsNullOrEmpty(entry.name) && entry.path.Contains(name.ToLower().Split("mining").Last())) ??
					null;

				if (starmapEntry != null && !starmapEntry.Equals(default(StarmapIndexEntry)))
				{
					starmapEntry.layer0_name = name;
					starmapEntry.layer0_reference = variable.ID;//parts.Last();

					parts = new List<string>(variable.ID.Split(','));

					if (parts.Count > 1)
					{
						do
						{
							parts.RemoveAt(parts.Count - 1);
							parent = String.Join(",", parts);
						} while (!nameMap.Exists(x => x.ID == parent) || nameMap.Find(x => x.ID == parent).Name ==variable.EditorName);
					}

					starmapEntry.layer0_parent = parent;
				}
				else
				{
					starmap.Add(new StarmapIndexEntry
					{
						layer0_name = name,
						layer0_parent = parent,
						layer0_reference = variable.ID,
					});
				}
			}

			return starmap;
		}

		private List<StarmapIndexEntry> FixIDs(List<StarmapIndexEntry> starmap)
		{
			var idDict = new Dictionary<string, string>();

			var layer0Missing = new List<string>();

			foreach (var entry in starmap)
			{
				if (entry.layer0_reference != null && entry.reference != null)
				{
					idDict[entry.layer0_reference] = entry.reference;
				}

				if (entry.reference == null)
				{
					layer0Missing.Add(entry.layer0_reference);
				}
			}

			foreach (var entry in starmap)
			{
				if (entry.layer0_reference == null || entry.reference != null ||
				    !idDict.TryGetValue(entry.layer0_parent, out var value)) continue;

				//entry.name = entry.layer0_name;
				//entry.reference = entry.layer0_reference;
				//entry.parent = value;
				//entry.hideInStarmap = "0";

				idDict[entry.layer0_reference] = entry.reference;

				// entry.layer0_name = null;
				// entry.layer0_reference = null;
				// entry.layer0_parent = null;
			}


			foreach (var missing in layer0Missing)
			{
				var entry = starmap.Find(x => x.layer0_reference == missing);

				if (entry != null && idDict.ContainsKey(entry.layer0_parent))
				{
					entry.name = entry.layer0_name;
					entry.parent = idDict[entry.layer0_parent];
				}

				//
				// if (entry == null)
				// {
				// 	continue;
				// }
				//
				// var parentEntry = starmap.Find(x => x.layer0_parent == entry.layer0_parent);
				//
				// var path = entry.path.Replace(@"libs/foundry/records/starmap/pu/system/stanton/", "").Split("/");
				//
				// var parent = path.First();
				//
				// if (parent == "libs")
				// {
				// 	parent = null;
				// 	path = entry.path.Replace(@"libs/foundry/records/starmap/pu/", "")
				// 		.Replace(".xml", "")
				// 		.Split("_");
				//
				// 	foreach (var part in path)
				// 	{
				// 		if (part.Contains("stanton"))
				// 		{
				// 			parent = part;
				// 			break;
				// 		}
				// 	}
				//
				// 	if (parent == "libs" || String.IsNullOrEmpty(parent))
				// 	{
				// 		// This is something floating in space; Set parent to Stanton star
				// 		if (entry.path.Contains("starmapobject"))
				// 		{
				// 			parent = "stantonstar";
				// 		}
				// 		else
				// 		{
				// 			continue;
				// 		}
				//
				// 	}
				// }
				//
				// var parentEnt = starmap.FirstOrDefault(x =>
				// 	!String.IsNullOrEmpty(x.path) && x.path.Contains($"starmapobject.{parent}"));
				//
				// if (parentEnt == null)
				// {
				// 	Console.WriteLine($"Could not find a candidate for {entry.name}");
				// 	continue;
				// }
				//
				// if (parentEnt.layer0_reference == null)
				// {
				// 	Console.WriteLine($"Parent {parentEnt.name} as no layer0 id");
				// 	continue;
				// }
				//
				// if (idDict.TryGetValue(parentEnt.layer0_reference, out var value))
				// {
				// 	entry.parent = value;
				// }
			}

			return starmap;
		}
	}
}
