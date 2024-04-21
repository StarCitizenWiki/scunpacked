using System.Collections.Generic;
using System.Linq;

using scdb.Xml.Entities;

namespace Loader
{
	public class MeleeCombatService
	{
		List<MeleeCombatConfig> index;

		public MeleeCombatService(List<MeleeCombatConfig> index)
		{
			this.index = index;
		}

		public MeleeCombatConfig GetByReference(string reference)
		{
			return index.FirstOrDefault(x => x.__ref == reference);
		}
	}
}
