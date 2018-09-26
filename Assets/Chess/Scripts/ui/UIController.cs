
using UnityEngine;
using UnityEngine.UI;

namespace ChessGame
{
	public class UIController : MonoBehaviour {

		[SerializeField]
		private Text status;

		public void SetStatus(string value)
		{
			status.text = value;
		}

		public void SetVisible(bool visible)
		{
			gameObject.SetActive(visible);
		}
	}


}
