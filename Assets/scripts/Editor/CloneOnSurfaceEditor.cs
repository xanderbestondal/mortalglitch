using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CloneOnSurface))]
[CanEditMultipleObjects]
public class CloneOnSurfaceEditor : Editor
{

	SerializedProperty scatterobjects;
	SerializedProperty dontRotate;

	private CloneOnSurface cloneOnSurface;

	void OnEnable()
	{
		cloneOnSurface = target as CloneOnSurface;

		//EditorSceneManager.MarkSceneDirty

	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		var controller = target as CloneOnSurface;

		SerializedProperty scatterObj = serializedObject.FindProperty("scatterobjects");
		SerializedProperty dontRot = serializedObject.FindProperty("dontRotate");

		dontRot.arraySize = scatterObj.arraySize;

		// nodig om te kunnen editejn
		EditorGUI.BeginChangeCheck(); 
		EditorGUILayout.PropertyField(scatterObj, true);
		EditorGUILayout.PropertyField(dontRot, true);
		
		if (EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
		// nodig om te kunnen editejn

		if (GUILayout.Button("generate"))
		{
			cloneOnSurface.generate();
		}
	}
}