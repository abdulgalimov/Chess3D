
using UnityEngine;
using UnityEngine.UI;

namespace ChessGame
{
	public class UIController : MonoBehaviour {

		private Text status;

		private void Awake()
		{
			status = transform.Find("status").GetComponent<Text>();			
		}

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
