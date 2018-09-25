
using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace ChessGame
{
	public enum PieceColor
	{
		NONE = 0,
		White = 1,
		Black = 2
	}

	public enum PieceType
	{
		PAWN, // пешка
		ROOK, // ладья
		KNIGHT, // конь
		BISHOP, // слон
		QUEEN, // королева
		KING, // ферзь
	}
	
	public class Piece : MonoBehaviour
	{
		private Position originPosition;

		public PieceColor Color;
		public PieceType Type;
		public Position position = new Position();
		public GameObject prefab;


		private GameObject fire;
		private ParticleSystem wall;

		private Texture fireTexture;
		private Texture smokeTexture;
		private Material fireMaterial;

		public virtual void Start()
		{
			Vector3 pos = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
			fire = Instantiate(prefab, pos, Quaternion.identity, transform);
			wall = fire.transform.Find("Fire wall").GetComponent<ParticleSystem>();
			var main = wall.main;
			main.simulationSpeed = 4;
			main.startColor = null;
			wall.Stop();
			//
			position = Coord.gameToModel(transform.position);
			originPosition = position.Clone();
			//
			fireMaterial = wall.GetComponent<Renderer>().material;
			fireTexture = fireMaterial.mainTexture;
		}

		private void onChangeTurn(Event e)
		{
			setFireTexture();
		}

		private void setFireTexture()
		{
			if (GameModel.instance.IsMyTurn)
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
				setFireTexture();
				GameModel.instance.on("changeTurn", onChangeTurn);
				wall.Play();
			}
			else
			{
				GameModel.instance.off("changeTurn", onChangeTurn);
				wall.Stop();
			}
		}

		public virtual bool GetValidMove(Position to, Piece toPiece=null)
		{
			return false;
		}

		public virtual void MoveTo(MoveConf moveConf)
		{
			position.Update(moveConf.toPosition);
			transform.DOMove(moveConf.toGamePosition, 0.5f);
			//
			if (moveConf.toPiece != null)
			{
				moveConf.toPiece.piece.Kill();
			}
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
