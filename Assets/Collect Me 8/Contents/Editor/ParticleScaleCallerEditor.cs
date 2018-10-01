using UnityEngine;
using System.Collections;
using UnityEditor;

namespace C8 {

[DisallowMultipleComponent]
[CustomEditor(typeof(ParticleScaleCaller))]
public class ParticleScaleCallerEditor : Editor {

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button ("Scale Children Particles")) {
			((ParticleScaleCaller) target).UpdateScaleChildren ();
		}

		if (GUILayout.Button ("Update Initial Values of Children Particles")) {
			((ParticleScaleCaller) target).UpdateInitialValuesInChildren ();
		}
	}
}

}