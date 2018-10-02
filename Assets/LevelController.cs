using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public GameObject pickups;
	public GameObject[] rings;
	public GameObject lose_screen;
	public GameObject win_screen;


	// Use this for initialization
	void Start () {
		rings = GameObject.FindGameObjectsWithTag ("Ring");
	}

	public void Reset() {
		Debug.Log ("LevelController.Reset()");
		//pickups.BroadcastMessage ("Restore");

		foreach (GameObject ring in rings) {
			ring.SetActive (true);
			ring.SendMessage ("Restore");
		}

	}

	public void Restart() {
		Application.LoadLevel(Application.loadedLevel);
	}

	// Update is called once per frame
	void Update () {
		
	}


	public void Lose() {
		print ("Level.Lose()");
		
		lose_screen.SetActive (true);
	}

	public void Win() {
		print ("Level.Win()");

		win_screen.SetActive (true);
	}

}
