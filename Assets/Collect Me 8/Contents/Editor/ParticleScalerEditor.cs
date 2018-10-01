using UnityEngine;
using System.Collections;
using UnityEditor;

namespace C8 {

[DisallowMultipleComponent]
[CustomEditor(typeof(ParticleScaler))]
public class ParticleScalerEditor : Editor {

	SerializedProperty initialWorldScale;
	SerializedProperty initialStartSize;
	SerializedProperty initialStartSpeed;
	SerializedProperty initialLengthScale;
	SerializedProperty initialVelocityScale;

	void OnEnable () {

		initialWorldScale = serializedObject.FindProperty ("initialWorldScale");
		initialStartSize = serializedObject.FindProperty ("initialStartSize");
		initialStartSpeed = serializedObject.FindProperty ("initialStartSpeed");
		//initialLengthScale = serializedObject.FindProperty ("initialLengthScale");
		//initialVelocityScale = serializedObject.FindProperty ("initialVelocityScale");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();

		ParticleScaler scaler = (ParticleScaler) target;

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Get Values")) {
			//Undo.RecordObject (scaler.gameObject, "Initial Values Update");
			scaler.UploadInitialValues ();
		}
		
		if (GUILayout.Button ("Get Values (Children)")) {
			//Undo.RecordObject (scaler.gameObject, "Initial Values Update");
			scaler.UploadInitialValuesChildren ();
		}

		GUILayout.EndHorizontal ();

		initialWorldScale.vector3Value = EditorGUILayout.Vector3Field("Initial World Scale", initialWorldScale.vector3Value);
		initialStartSize.floatValue = EditorGUILayout.FloatField("Init. Start Size", initialStartSize.floatValue);
		initialStartSpeed.floatValue = EditorGUILayout.FloatField("Init. Start Speed", initialStartSpeed.floatValue);
		//initialLengthScale.floatValue = EditorGUILayout.FloatField("Init. Length Scale", initialLengthScale.floatValue);
		//initialVelocityScale.floatValue = EditorGUILayout.FloatField("Init. Velocity Scale", initialVelocityScale.floatValue);

		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Scale")) {
			Undo.RecordObject (scaler.gameObject, "Particle Scaler Update");
			scaler.ApplyScale ();
		}

		if (GUILayout.Button ("Scale (Children)")) {
			scaler.ApplyScaleChildren ();
		}

		GUILayout.EndHorizontal ();

		serializedObject.ApplyModifiedProperties ();
	}
}	

}