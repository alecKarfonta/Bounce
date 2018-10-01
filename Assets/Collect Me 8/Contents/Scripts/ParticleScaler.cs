using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace C8 {

public class ParticleScaler : MonoBehaviour {
	
	public Vector3 initialWorldScale;
	public float initialStartSize;
	public float initialStartSpeed;
	//public float initialLengthScale;
	//public float initialVelocityScale;
	
	public void UploadInitialValues ()
	{
		initialWorldScale = transform.lossyScale;
		ParticleSystem particleSystem = GetComponent<ParticleSystem> ();
		initialStartSize = particleSystem.startSize;
		initialStartSpeed = particleSystem.startSpeed;

		ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
		if (renderer) 
		{	
			//initialLengthScale = renderer.lengthScale;
			//initialVelocityScale = renderer.velocityScale;
		}
	}

	public void UploadInitialValuesChildren ()
	{
		ParticleScaler[] children = GetComponentsInChildren<ParticleScaler> ();
		foreach (var child in children)
			child.UploadInitialValues ();

		UploadInitialValues ();
	}

	public void ApplyScaleChildren ()  
	{
		ParticleScaler[] children = GetComponentsInChildren<ParticleScaler> ();
		foreach (var child in children)
			child.ApplyScale ();
	}
	
	public void ApplyScale () 
	{  
		float lossyFrac = transform.lossyScale.magnitude / initialWorldScale.magnitude;
		ParticleSystem particleSystem = GetComponent<ParticleSystem> ();
		particleSystem.startSize = initialStartSize * lossyFrac;
		particleSystem.startSpeed = initialStartSpeed * lossyFrac;
		ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();

		if (renderer) {

			//renderer.lengthScale = initialLengthScale * lossyFrac;
			//renderer.velocityScale = initialVelocityScale * lossyFrac;
		}
	}
	
}

}