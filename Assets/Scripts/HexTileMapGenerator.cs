using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using Models;
using UnityEngine;

public class HexTileMapGenerator : MonoBehaviour
{
	private const int HEX_RING_DIVIDER = 6;

	[Header("Debug")] [SerializeField] private bool _debug;
	[SerializeField] private TestHexModel _testHexModel;

	[SerializeField] private Vector3 _centerPiecePosition;

	[Header("Settings")] [SerializeField] private GameObject _tilePrefab;

	[SerializeField] private float _spawnTime = 1;

	[SerializeField] private float _spawnDelay = 0.5f;
	
	private readonly List<HexTile> _tiles = new List<HexTile>();

	private float _tileWidth;

	private HexTile _lastSpawnedHex;
	private HexTile _nextRingStartingHex;
	private Transform _container;
	private HexTile _selectedHex;
	private HexModel _hexModel;

	private float TileWithPAdding => _tileWidth + _hexModel?.TilePadding ?? 0;

	public void DeleteTiles()
	{
		foreach (HexTile t in _tiles)
		{
			Destroy(t.gameObject);
		}

		_tiles.Clear();
		HexTile.Index = -1;
	}

	public async UniTaskVoid PlaceTiles()
	{
		_hexModel = GetHexModel();

		if (_container == null)
			_container = new GameObject("HexContainer").transform;

		int rings = _hexModel.Tiles.Length / HEX_RING_DIVIDER - 1; //Minus Center

		HexTile centerPiece = await AddTile(_centerPiecePosition);
		_nextRingStartingHex = centerPiece;

		var renderer = centerPiece.GetComponent<Renderer>();
		centerPiece.name = $"CenterPieceHex";
		Debug.Assert(renderer);

		_tileWidth = renderer.bounds.size.z;

		for (int i = 1; i <= rings; i++)
		{
			await AddLayer(i);
		}
	}

	private HexModel GetHexModel()
	{
		return _testHexModel.HexTileModelJson.DeserializeJson<HexModel>();
	}

	private async UniTask AddLayer(int i)
	{
		_lastSpawnedHex = _nextRingStartingHex;
		_nextRingStartingHex = await AddTile(_lastSpawnedHex, Direction.North);

		Direction direction = Direction.SouthEast;

		for (int d = 0; d < HEX_RING_DIVIDER; d++)
		{
			int j = direction != Direction.NorthEast ? 0 : 1;
			for (; j < i; j++)
			{
				await AddTile(_lastSpawnedHex, direction);
			}

			direction = GetNextDirection(direction);
		}
	}

	private Vector3 GetSpawnDirection(int dir)
	{
		float angleInRadians = 2 * Mathf.PI / HEX_RING_DIVIDER * dir;

		float vertical = MathF.Sin(angleInRadians);
		float horizontal = MathF.Cos(angleInRadians);

		return new Vector3(vertical, 0, horizontal);
	}

	private Direction GetNextDirection(Direction currentDir)
	{
		switch (currentDir)
		{
			case Direction.North:
				return Direction.NorthEast;
			case Direction.SouthEast:
				return Direction.South;
			case Direction.South:
				return Direction.SouthWest;
			case Direction.SouthWest:
				return Direction.NorthWest;
			case Direction.NorthWest:
				return Direction.North;
			default:
				return Direction.North;
		}
	}

	private async UniTask<HexTile> AddTile(HexTile fromGe, Direction direction)
	{
		HexTile hexTile;

		switch (direction)
		{
			case Direction.North:
				hexTile = InstantiateTile(fromGe.transform.position + GetSpawnDirection((int)Direction.North) * TileWithPAdding);
				break;
			case Direction.NorthEast:
				hexTile = InstantiateTile(fromGe.transform.position + GetSpawnDirection((int)Direction.NorthEast) * TileWithPAdding);
				break;
			case Direction.SouthEast:
				hexTile = InstantiateTile(fromGe.transform.position + GetSpawnDirection((int)Direction.SouthEast) * TileWithPAdding);
				break;
			case Direction.South:
				hexTile = InstantiateTile(fromGe.transform.position + GetSpawnDirection((int)Direction.South) * TileWithPAdding);
				break;
			case Direction.SouthWest:
				hexTile = InstantiateTile(fromGe.transform.position + GetSpawnDirection((int)Direction.SouthWest) * TileWithPAdding);
				break;
			case Direction.NorthWest:
				hexTile = InstantiateTile(fromGe.transform.position + GetSpawnDirection((int)Direction.NorthWest) * TileWithPAdding);
				break;
			default:
				return null;
		}

		_lastSpawnedHex = hexTile;
		_tiles.Add(hexTile);
		await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));
		return hexTile;
	}

	private async UniTask<HexTile> AddTile(Vector3 pos)
	{
		HexTile ge = InstantiateTile(pos);
		_lastSpawnedHex = ge;
		_tiles.Add(ge);
		await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));
		return ge;
	}

	private HexTile InstantiateTile(Vector3 pos)
	{
		var hexTile = Instantiate(_tilePrefab, pos, Quaternion.identity, _container).GetComponent<HexTile>();
		hexTile.transform.localScale = new Vector3(_hexModel.TileSize, hexTile.transform.localScale.y, _hexModel.TileSize);
		hexTile.Init(HexTile.Index, _hexModel.DefaultTileColor, _hexModel.Tiles.FirstOrDefault(tile => tile.Index == HexTile.Index).ClickedColor);
		return hexTile;
	}
}