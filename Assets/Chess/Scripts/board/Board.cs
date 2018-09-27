using UnityEngine;
using DG.Tweening;

namespace ChessGame
{
	public class Board : MonoBehaviour
	{
		[SerializeField]
		private Transform area;
		private ParticleSystem aura;

		private void Start ()
		{
			area.transform.position = new Vector3(0, 13, 0);
			aura = area.GetComponent<ParticleSystem>();
			//
			var objects = GameObject.FindGameObjectsWithTag("piece");
			foreach (var obj in objects)
			{
				Destroy(obj);
			}
		}


		private bool auraValid;
		public void AuraValid(bool visible)
		{
			if (auraValid == visible) return;
			auraValid = visible;
			//
			var main = aura.main;
			main.startColor = visible ? new ParticleSystem.MinMaxGradient(Color.green) : new ParticleSystem.MinMaxGradient(Color.red);
			aura.time = 0;
			aura.Play();
		}

		private readonly Position auraPosition = new Position(-5, -5);
		public void AuraPosition(Position position)
		{
			if (auraPosition.Compare(position)) return;
			auraPosition.Update(position);
			
			var pos = Coordinates.ModelToGame(auraPosition);
			area.transform.DOMove(pos, 0.4f);
		}

		private bool auraVisible;
		public void AuraVisible(bool visible)
		{
			if (auraVisible == visible) return;
			auraVisible = visible;
			
			area.transform.DOScale(auraVisible ? Vector3.one : Vector3.zero, 0.5f);
		}

	}	
}
