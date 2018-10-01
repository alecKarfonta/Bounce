using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private float player_avg_speed = 0.0f;

	private Vector3 max_zoom = new Vector3(0,0,-150);

	private Vector3 min_zoon = new Vector3(0,0,-70);

	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		// Update camera posistion
		//transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.005f);
		transform.position = player.transform.position + offset;

		float speed = player.GetComponent<Rigidbody> ().velocity.magnitude;

		player_avg_speed = (player_avg_speed * .998f) + (speed * .002f);

		float t = Mathf.InverseLerp( 0, 10, player_avg_speed ); // returns a value between 0-1.
		t = Mathf.SmoothStep( 0, 1, t ); // smooth out the t value so that the camera will ease in and out nicely.
		Vector3 cameraPosition = Vector3.Lerp( min_zoon, max_zoom, t ); // blend between these two points
		transform.Translate(cameraPosition);

	}




	public static float ReMap (float x, float x1, float x2, float y1,  float y2)
	{
		var m = (y2 - y1) / (x2 - x1);
		var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

		return m * x + c;
	}

}