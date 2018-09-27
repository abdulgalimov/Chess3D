
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ChessGame
{
	public enum PieceColor
	{
		None = 0,
		White = 1,
		Black = 2
	}

	public enum PieceType
	{
		Pawn,
		Rook,
		Knight,
		Bishop,
		Queen,
		King
	}
	
	public class Piece : MonoBehaviour
	{
		public PieceColor Color;
		public PieceType Type;
		public Position Position = new Position();
		
		private static GameObject firePrefab;


		private GameObject fire;
		private ParticleSystem wall;

		private Texture fireTexture;
		private Texture smokeTexture;
		private Material fireMaterial;

		public virtual void Start()
		{
			if (firePrefab == null)
			{
				firePrefab = (GameObject)Resources.Load("FireAura/FirePrefab", typeof(GameObject));
			}
			//
			var pos = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
			fire = Instantiate(firePrefab, pos, Quaternion.identity, transform);
			wall = fire.transform.Find("Fire wall").GetComponent<ParticleSystem>();
			var main = wall.main;
			main.simulationSpeed = 4;
			main.startColor = null;
			wall.Stop();
			//
			Position = Coordinates.GameToModel(transform.position);
			//
			fireMaterial = wall.GetComponent<Renderer>().material;
			fireTexture = fireMaterial.mainTexture;
		}

		private void OnChangeTurn(Event e)
		{
			SetFireTexture();
		}

		private void SetFireTexture()
		{
			if (GameModel.Instance.IsMyTurn)
			{
				fireMaterial.mainTexture = fireTexture;				
			}
			else
			{
				if (smokeTexture == null)
				{
					smokeTexture = (Texture2D) Resources.Load("FireAura/smoke", typeof(Texture2D));
				}
				fireMaterial.mainTexture = smokeTexture;
			}		
		}

		public void SetSelected(bool selected)
		{
			
			if (selected)
			{
				SetFireTexture();
				GameModel.Instance.On("changeTurn", OnChangeTurn);
				wall.Play();
			}
			else
			{
				GameModel.Instance.Off("changeTurn", OnChangeTurn);
				wall.Stop();
			}
		}

		public virtual bool GetValidMove(Position to, Piece toPiece=null)
		{
			return false;
		}

		public virtual void MoveTo(MoveConf moveConf)
		{
			Position.Update(moveConf.ToPosition);
			transform.DOMove(moveConf.ToGamePosition, 0.5f);
			//
			moveConf.ToPiece?.Piece.Kill();
		}


		private static GameObject ashPrefab;
		public void Kill(float timeout=0.1f)
		{
			if (ashPrefab == null)
			{
				ashPrefab = (GameObject)Resources.Load("Ash/AshPrefab", typeof(GameObject));
			}
			enabled = false;
			StartCoroutine(PlayAfterSeconds(timeout));
		}
		IEnumerator PlayAfterSeconds(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			//
			Vector3 pos = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
			GameObject ash = Instantiate(ashPrefab, pos, Quaternion.identity, transform);
			ash.transform.Rotate(new Vector3(1, 0, 0), -90);
			StartCoroutine(RemoveAfterSeconds(0.5f));			
		}
		
		IEnumerator RemoveAfterSeconds(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			gameObject.GetComponent<Renderer>().enabled = false;
		}

		public void Release()
		{
			Destroy(gameObject);
		}
	}
}
