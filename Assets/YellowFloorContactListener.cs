using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowFloorContactListener : MonoBehaviour {

	public float bounce_speed = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col)
	{
		// If hit player
		if(col.gameObject.name == "Player")
		{
			GameObject player = col.gameObject;
			Debug.Log ("Hit Player");
			// Destroy(col.gameObject);

			// Bounce player
		}
	}

}
