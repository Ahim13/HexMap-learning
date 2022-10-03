using UnityEngine;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "TestHexTileModel", menuName = "Test/HexTileModel", order = 0)]
	public class TestHexModel : ScriptableObject
	{
		[TextArea(100, 300)]
		public string HexTileModelJson;
	}
}