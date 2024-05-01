using System;
using System.Collections.Generic;
using System.Linq;

namespace Loader
{
	public class LocalisationService
	{
		Dictionary<string, string> labels;

		public LocalisationService(Dictionary<string, string> labels)
		{
			this.labels = labels;
		}

		public string GetText(string label, string fallback = null)
		{
			if (label == null) return fallback;

			var key = label.StartsWith("@") ? label.Substring(1) : label;
			var keyP = key+",P";

			if (key == "LOC_EMPTY") return fallback;
			if (key == "LOC_UNINITIALIZED") return fallback;

			if (!labels.ContainsKey(key) && !labels.ContainsKey(keyP)) return fallback;

			var text = labels.ContainsKey(key) ? labels[key] : labels[keyP];
			text = text.Replace("\\n", "\n");

			if (text == "<= PLACEHOLDER =>") return fallback ?? label;
			if (String.IsNullOrWhiteSpace(text)) return fallback ?? label;

			return text;
		}
	}
}
