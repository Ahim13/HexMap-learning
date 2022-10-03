

using Newtonsoft.Json;
using UnityEngine;

namespace Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class HexModel : IModel
	{
		public HexModel(float tileSize = 0f, float tilePadding = 0f, string defaultTileColor = default, TileModel[] tiles = default)
		{
			TileSize = tileSize;
			TilePadding = tilePadding;
			DefaultTileColorString = defaultTileColor;
			Tiles = tiles;
			
			ColorUtility.TryParseHtmlString($"#{DefaultTileColorString}", out DefaultTileColor);
		}

		[JsonProperty("TileSize")] public float TileSize  { get; set; }
		[JsonProperty("TilePadding")] public float TilePadding  { get; set; }
		[JsonProperty("DefaultTileColor")] public string DefaultTileColorString { get; set; }
		[JsonProperty("Tiles")] public TileModel[] Tiles { get; set; }
		
		public Color DefaultTileColor;
		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.None);
		}
	}
}