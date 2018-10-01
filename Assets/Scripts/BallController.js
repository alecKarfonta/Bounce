#pragma strict

var rotationSpeed = 600;
var jumpHeight = 8;
var isJustHitGround = false;

private var isJustHit = false;

var Hit01 : AudioClip;
var Hit02 : AudioClip;
var Hit03 : AudioClip;
 
 var dstGround : float;

private var isFalling = false;

private var isMovingLeft = false;
private var isMovingRight = false;

function Start () {
	dstGround = GetComponent.<Collider>().bounds.extents.y;
}

function Update () {

	// Rotation 
	var rotation : float = Input.GetAxis("Vertical") * rotationSpeed;
	rotation *= Time.deltaTime;
	//rigidbody.AddRelativeTorque (Vector3.back * rotation);
	
	rotation = Input.GetAxis("Horizontal") * rotationSpeed ;
	rotation *= Time.deltaTime;
	GetComponent.<Rigidbody>().AddRelativeTorque (Vector3.left * rotation);
	
	if (Input.GetKeyDown(KeyCode.D)) {
		isMovingRight = true;
	} else if (Input.GetKeyDown(KeyCode.A)) {
		isMovingLeft = true;
	} else if (Input.GetKeyUp(KeyCode.D)) {
		isMovingRight = false;
	} else if (Input.GetKeyUp(KeyCode.A)) {
		isMovingLeft = false;
	}
	
	if (isMovingRight) {
		GetComponent.<Rigidbody>().velocity.x += .2;
	} else if (isMovingLeft) {
		GetComponent.<Rigidbody>().velocity.x -= .2;
	}
	// Jump
	if (Input.GetKeyDown(KeyCode.Space) && isFalling == false) {
	
		GetComponent.<Rigidbody>().velocity.y = jumpHeight;
		
	}
	isFalling = true;
		
}

function IsGrounded () : boolean {
	return Physics.Raycast (transform.position, -Vector3.up, dstGround + .1 );
}

function OnCollisionEnter() {
	var theHit =  Random.Range(0,4);
	switch (theHit) {
		case 0:
		GetComponent.<AudioSource>().clip = Hit01;
		break;
		case 1:
		GetComponent.<AudioSource>().clip = Hit02;
		break;
		default:
		GetComponent.<AudioSource>().clip = Hit03;
		break;
	}
	hit();
		

}

function OnCollisionStay() {

	isFalling = false;
}


function hit() {
	//isJustHit = true;
	GetComponent.<AudioSource>().pitch = Random.Range(0.9, 1.1);
	GetComponent.<AudioSource>().Play();
	
	//yield WaitForSeconds(.3);
	//isJustHit = false;
	
}