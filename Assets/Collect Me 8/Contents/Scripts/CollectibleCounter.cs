using UnityEngine;
using System.Collections;

public class CollectibleCounter : MonoBehaviour {

	[SerializeField]
	UnityEngine.UI.Text counterText;

	[SerializeField]
	string prefix;

	[SerializeField]
	string postfix;
	
	int count;

	void Awake () {

		Reset ();
	}

	public int GetValue () {

		return count;
	}

	public void OnCollected () {

		if (counterText == null) {
			
			Debug.LogError ("Counter Text Reference cannot be Null");
			return;
		}

		count++;
		counterText.text = prefix + count + postfix;
	}

	public void Reset () {

		if (counterText == null) {

			Debug.LogError ("Counter Text Reference cannot be Null");
			return;
		}

		count = 0;
		counterText.text = prefix + "0" + postfix;
	}
}
