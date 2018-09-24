using ChessGame;
using UnityEngine;
using DG.Tweening;

public class Board : MonoBehaviour
{
	private Transform area;
	private ParticleSystem aura;

	void Start ()
	{
		DOTween.Init(true, true);
		//
		area = transform.Find("selected_area");
		area.transform.position = new Vector3(0, 13, 0);
		aura = area.transform.GetComponent<ParticleSystem>();
		//
		GameObject[] objects = GameObject.FindGameObjectsWithTag("piece");
		foreach (GameObject obj in objects)
		{
			Piece piece = obj.GetComponent<Piece>();
			piece.Release();
		}
	}


	private bool _auraValid;
	public void AuraValid(bool visible)
	{
		if (_auraValid == visible) return;
		_auraValid = visible;
		//
		var main = aura.main;
		main.startColor = visible ? new ParticleSystem.MinMaxGradient(Color.green) : new ParticleSystem.MinMaxGradient(Color.red);
		aura.time = 0;
		aura.Play();
	}

	private Position _auraPosition = new Position(-5, -5);
	public void AuraPosition(Position position)
	{
		if (!_auraPosition.Compare(position))
		{
			_auraPosition.Update(position);
			//
			Vector3 pos = Coord.modelToGame(_auraPosition);
			area.transform.DOMove(pos, 0.4f);
		}
	}

	private bool _auraVisible;
	public void AuraVisible(bool visible)
	{
		if (_auraVisible == visible) return;
		_auraVisible = visible;
		if (_auraVisible)
		{
			area.transform.DOScale(Vector3.one, 0.5f);
		}
		else
		{
			area.transform.DOScale(Vector3.zero, 0.5f);
		}
	}

}	
