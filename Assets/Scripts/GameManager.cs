using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField] private Camera _mainCamera;
	[SerializeField] private HexTileMapGenerator _hexTileMapGenerator;

	private HexTile _selectedHex;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			DetectObject();
		}
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;
	}

	private void Start()
	{
		_ = _hexTileMapGenerator.PlaceTiles();
	}

	private void DetectObject()
	{
		Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			if (hit.collider != null)
			{
				if (_selectedHex != null)
					_selectedHex.DeSelect();

				_selectedHex = hit.transform.GetComponent<HexTile>();
				if (_selectedHex)
					_selectedHex.Select();
			}
		}
	}

	private void OnDrawGizmos()
	{
		Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
		Gizmos.DrawRay(ray);
	}
}