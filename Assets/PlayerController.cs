using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject level;
	public GameObject player;

	public float move_speed = 1.0f;

	public float init_scale = 1.0f;
	public float scale = 5.0f;

	private float fail_line = -75;
	private Vector3 start_pos = new Vector3(-42,25,0);

	private float power_cooldown = 10;
	private float power_cooldown_timer = 10;
	public float move_max_power = 160.0f;
	private float move_power = 60.0f;
	private bool is_moving = false;

	private float current_scale = 1.0f;
	public float max_scale = 10.0f;

	public float boost_bounce_speed = 600.0f;

	public float scale_power_weight = 5.0f;
	// Set the amount of power that should come form the distance of the input from the player
	public float distance_power_weight = 15.0f;

	// Set the amount of power that should come from the speed of the player
	public float speed_power_weight = .5f;

	private Vector3 targetPosition = new Vector3(0.0f,0.0f,0.0f);

	private float timer;
	public Text power_text;
	public Text timer_text;
	public AudioSource sound_source;
	public AudioSource power_sound_source;
	public AudioClip bounce_sound;
	public AudioClip win_sound;
	public AudioClip die_sound;
	public AudioClip move_sound;
	public Image power_bar_image;

	public ParticleSystem user_input_particles;
	public ParticleSystem move_particles;
	public ParticleSystem win_particles;
	public ParticleSystem death_particles;

	private float power_bar_full_width = 256;

	private float low_pitch_range = .5f;
	private float high_pitch_range = .75f;
	private float velocity_to_volume = .1F;

	public int lives = 1;
	public GameObject gui_frame;
	public GameObject lifeImage;
	private GameObject[] lifeImages;
	private bool is_started = false;
	private bool is_won = false;
	private bool is_dead = false;
	private bool is_lost = false;


	private Rigidbody rb;

	void Init() {
		print ("Player.Init()");
		is_started = false;
		is_moving = false;
		is_won = false;

		// Instantiate lives indicators
		for (int index = 0; index < lifeImages.Length; index++) {
			//Debug.Log ("Destroy " + index);
			lifeImages [index].SetActive (false);
		}

		// If has lives
		if (lives > 0) {
			// Instantiate lives indicators
			for (int index = 1; index < lives; index++) {


				lifeImages [index].SetActive (true);

			}
		} else {
			lifeImage.SetActive (true);
		}
	}

	// Use this for initialization
	void Start () {
		print ("Player.Start()");
		rb = GetComponent<Rigidbody> ();
		start_pos = rb.transform.position;
		rb.isKinematic = true;

		lifeImages = new GameObject[lives];

		// Instantiate lives indicators
		for (int index = 0; index < lives; index++) {
			lifeImages[index] = Instantiate (lifeImage, lifeImage.transform.position + new Vector3 (-25 * index, 0, 0), lifeImage.transform.rotation, gui_frame.transform);
		}

		Init ();

		//move_particles.GetComponent<ParticleSystem>().main.startColor = new Color (1.0f, 1.0f, 1.0f);		
	}

	void Awake() {

		//sound_source = GetComponent<AudioSource> ();

	}

	// Update is called once per frame
	void FixedUpdate () {
		
		if (is_dead == true || is_won == true) {
			return;
		}
		// Check if trying to move
		if (Input.GetMouseButton(0)) {
			if (is_started == false) {
				is_started = true;
				rb.isKinematic = false;
			}
			// Check if has enough power to start move
			if (move_power >= 1.0f) {
				// Check if cooldown timer not finished
				if (power_cooldown == 0) {
					// Confirm can move
					is_moving = true;
					// Play move sound
					power_sound_source.PlayOneShot(move_sound, .07f);
				} else {
					// Confirm can move
					is_moving = false;
				}
				// Else hit power limit
			} else {
				Debug.Log ("Tried to move with no power");
				power_cooldown = power_cooldown_timer;

				is_moving = false;
				power_sound_source.Stop ();

			}
		
		// Else not trying to move
		} else {
			if (is_moving) {
				is_moving = false;
				power_sound_source.Stop ();
			}

			// Cooldown 
			if (power_cooldown > 0) {
				power_cooldown -= 1;
			}
		}

		// Calculate speed
		float xy_speed = Mathf.Sqrt (Mathf.Pow(rb.velocity.x, 2.0f) + Mathf.Pow(rb.velocity.y, 2.0f));


		if (is_moving) {
			//targetPosition = Camera.main.ScreenToWorldPoint (targetPosition);
			//transform.position = Vector3.MoveTowards (transform.position, targetPosition, moveBackSpeed * Time.deltaTime);
			//Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//targetPos.z = transform.position.z;

			// Show user input
			user_input_particles.Play();

			// Get position of touch input
			targetPosition.z = transform.position.z - Camera.main.transform.position.z;
			//targetPosition.z = 0.0f;
			targetPosition.x = Input.mousePosition.x ;
			targetPosition.y = Input.mousePosition.y;
			// Convert touch coords to game coords
			targetPosition = Camera.main.ScreenToWorldPoint (targetPosition);
			targetPosition.z = 0.0f;
			// Move object toward touch
			//targetPosition = Vector3.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);

			// Play user input particles
			user_input_particles.gameObject.transform.position = targetPosition;
			user_input_particles.Play ();
				
			float distance = Mathf.Sqrt((targetPosition - player.transform.position).magnitude);

			// Calculate direction from player to touch
			Vector2 direction = (targetPosition - player.transform.position).normalized;
			//Vector2 direction = (player.transform.position - targetPosition).normalized;

			//Debug.Log ("------------------------------------------------");
			//Debug.Log ("Player Pos: " + player.transform.position);
			//Debug.Log ("Target Pos: " + targetPosition);
			//Debug.Log ("direction: " + direction);
			//Debug.Log ("distance: " + distance);
			//Debug.Log ("------------------------------------------------");

			//Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0);

			float move_force = (move_speed + (distance*distance_power_weight) + (Mathf.Sqrt(xy_speed)*speed_power_weight) + ((max_scale - current_scale) * scale_power_weight)); 

			// Apply force to object
			rb.AddForce (direction * move_force );


			move_power -= 1.0f;
			move_power = Mathf.Max (move_power, 0.0f);

			//Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0);

			//rb.AddForce (movement * speed);

			// Lerp scale to .5
			current_scale = Mathf.Lerp (current_scale, .25f, Time.deltaTime / .350f);


			rb.transform.localScale = new Vector3 (current_scale, current_scale, current_scale);
		// Else not moving
		} else {


			move_power += 1.0f;
			move_power = Mathf.Min(move_power, move_max_power);

			// Lerp scale back to 1.0
			current_scale = Mathf.Lerp(current_scale, max_scale, Time.deltaTime / 1.0f);

			rb.transform.localScale = new Vector3(current_scale, current_scale, current_scale);

			user_input_particles.Stop ();
		}

		if (is_won == false && is_started == true) {
			timer += Time.deltaTime;
		}

		// Update power bar
		float ratio = move_power / move_max_power;
		power_bar_image.fillAmount = ratio;


		// Update power text
		power_text.text = "Move Power: " + move_power + "\n"
		+ "Power Cooldown: " + power_cooldown + "\n"
		//+ "Speed: " + rb.velocity + " - " + xy_speed + "\n"
		//+ "Mass: " + rb.mass  + "\n"
		+ "Position: " + rb.transform.position  + "\n"
		+ "Start Position: " + start_pos  + "\n"
		//	+ "Scale: " + current_scale + "\n"
		//	+ "Scale Force: " + ((max_scale - current_scale) * scale_power_weight) + "\n"
			+ "Lives: " + lives + "\n"
			;

		string minutes = ((int)timer / 60).ToString ();
		string seconds = (timer % 60).ToString ("f2");


		timer_text.text = minutes + ":" + seconds;

		// Check if below fail line
		if (is_dead == false && is_started && rb.position.y < fail_line || rb.position.x < -200 || rb.position.x > 1200) {
			is_dead = true;
			StartCoroutine(die());
		}
	}

	IEnumerator die() 
	{

		print ("Player.Die()");

		is_started = false;

		lives -= 1;


		death_particles.Play ();

		sound_source.PlayOneShot(die_sound,0.5f);

		//reset ();
		StartCoroutine(reset());

		//level.GetComponent<LevelController> ().Reset ();


		yield return null;
	}

	public IEnumerator reset() {

		print ("Player.Reset()");

		//rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		//yield return new WaitForSeconds(.25f);
		//move_particles.Pause ();
		//rb.MovePosition (start_pos);
		//rb.MovePosition (start_pos);

		//rb.MovePosition (start_pos);
		//Invoke("reset", .55f);
	
		// Check if lost
		if (lives < 1) {
			level.GetComponent<LevelController> ().Lose ();
			is_lost = true;
			lifeImage.SetActive (false);

		} else {
			//move_particles.Pause ();
			//move_particles.Play ();
			//rb.position = start_pos;
			//rb.MovePosition (start_pos);

			//rb.isKinematic = false;
			//rb.position = start_pos;
			//rb.isKinematic = true;

			timer = 0.0f;
			Init ();

			rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);

			level.GetComponent<LevelController> ().Reset ();

			is_dead = false;


			current_scale = init_scale;
			max_scale = init_scale;

			move_particles.Stop ();
			yield return new WaitForSeconds (.35f);

			rb.transform.localScale = new Vector3 (init_scale, init_scale, init_scale);

			//rb.position = start_pos;
			rb.MovePosition (start_pos);

			yield return new WaitForSeconds (.05f);
			rb.isKinematic = true;
			yield return new WaitForSeconds (.25f);
			move_particles.Play ();

			//rb.position = start_pos;
			//move_particles.Play ();
			//rb.isKinematic = false;

		}

		yield return null;
	}

	void OnCollisionEnter (Collision coll)
	{
		//Debug.Log ("CollisionEnter()");

		sound_source.pitch = Random.Range (low_pitch_range,high_pitch_range);
		float hitVol = Mathf.Log (coll.relativeVelocity.magnitude * velocity_to_volume) * 0.75f;

		//Debug.Log ("coll.relativeVelocity.magnitude  = " + coll.relativeVelocity.magnitude);
		//Debug.Log ("hitVol  = " + hitVol);
		//Debug.Log ("coll.collider.tag = " + coll.collider.tag);

		// Check if hit the win floor
		//Debug.Log("hit " + coll.gameObject.tag);

		string tag = coll.gameObject.tag;

		// If hit finish
		if (tag == "Finish") {

			is_won = true;

			// Freeze player
			rb.isKinematic = true;
			sound_source.pitch = .7f;
			sound_source.PlayOneShot (win_sound, .35f);
			win_particles.Play ();

			Invoke ("Win", 1.25f);

		
		} else if (tag == "Ring") {
			got_ring ();
			// Else hit something else
		} else if (tag == "Boost") {
			// Bounce player
			Vector3 movement = new Vector3 (1.0f, 1.75f, 0.0f);

			player.GetComponent<Rigidbody> ().AddForce (movement * boost_bounce_speed);

		} else if (tag == "PushDown") {

			// Bounce player
			Vector3 movement = new Vector3 (0.0f, -1.0f, 0.0f);

			player.GetComponent<Rigidbody>().AddForce (movement * boost_bounce_speed);

		} else {

			sound_source.PlayOneShot(bounce_sound,hitVol);
		}
		 
	}

	public void Win() {
		print ("Player.Win()");
		level.GetComponent<LevelController> ().Win ();
	}

	public void got_ring() {
		Debug.Log ("Got ring");
		max_scale += 2.0f;
		move_max_power += 10.0f;
	}
}
