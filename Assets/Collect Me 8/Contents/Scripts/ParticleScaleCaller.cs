using UnityEngine;
using System.Collections;

namespace C8 {

public class ParticleScaleCaller : MonoBehaviour {

	public void UpdateScaleChildren () 
	{
		var children = gameObject.GetComponentsInChildren<ParticleScaler> ();
		foreach (var child in children) 
			child.ApplyScaleChildren ();
	}

	public void UpdateInitialValuesInChildren () 
	{
		var children = gameObject.GetComponentsInChildren<ParticleScaler> ();
		foreach (var child in children) 
			child.UploadInitialValuesChildren ();
	}
}

}