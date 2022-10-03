

using Newtonsoft.Json;
using UnityEngine;

namespace Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TileModel : IModel
	{
		public TileModel(int index = -1, string clickedColor = default)
		{
			Index = index;
			ClickedColorString = clickedColor;

			ColorUtility.TryParseHtmlString($"#{ClickedColorString}", out ClickedColor);
		}

		[JsonProperty("Index")] public int Index { get; set; }
		[JsonProperty("ClickedColor")] public string ClickedColorString { get; set; }

		public Color ClickedColor;
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.None);
		}
	}
}