using DG.Tweening;
using UnityEngine;


public class HexTile : MonoBehaviour
{
	public static int Index = -1;

	[SerializeField] private float _selectedHeight;
	[SerializeField] private float _animationSpeed;
	[SerializeField] private float _animationSpeedDissolve;
	[SerializeField] private Color _selectedColor;
	[SerializeField] private Color _defaultColor;
	[SerializeField]private int _dissolveEndValue;
	
	private float _originalHeight;
	private int _tileIndex;

	private Material _dissolveMaterial;
	private static readonly int DefaultColor = Shader.PropertyToID("_DefaultColor");
	private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
	private static readonly int Amount = Shader.PropertyToID("_Amount");

	private void Awake()
	{
		++Index;
		_originalHeight = transform.position.y;
		_dissolveMaterial = GetComponent<Renderer>().materials[0];
		Material[] materials = { _dissolveMaterial, _dissolveMaterial, _dissolveMaterial };
		GetComponent<Renderer>().materials = materials;
		_dissolveMaterial.SetColor(DefaultColor, _defaultColor);
		_dissolveMaterial.SetColor(EdgeColor, _selectedColor);
		_dissolveMaterial.DOFloat(_dissolveEndValue, Amount, _animationSpeedDissolve);
	}

	public void Init(int idx, Color defaultColor, Color selectedColor)
	{
		_tileIndex = idx;
		_defaultColor = defaultColor;
		_selectedColor = selectedColor;
		_dissolveMaterial.SetColor(DefaultColor, _defaultColor);
		_dissolveMaterial.SetColor(EdgeColor, _selectedColor);
	}

	public void Select()
	{
		transform.DOMoveY(_selectedHeight, _animationSpeed);
		_dissolveMaterial.DOColor(_selectedColor, DefaultColor, _animationSpeed);
	}

	public void DeSelect()
	{
		transform.DOMoveY(_originalHeight, _animationSpeed);
		_dissolveMaterial.DOColor(_defaultColor, DefaultColor, _animationSpeed);
	}

	private void OnDestroy()
	{
		Destroy(_dissolveMaterial);
	}
}