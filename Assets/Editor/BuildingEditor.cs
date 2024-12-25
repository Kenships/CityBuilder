using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Pull the latest data from the actual object
        serializedObject.Update();

        // For convenience, get a direct reference to the target
        Building building = (Building) target;

        // A simple header
        EditorGUILayout.LabelField("Configuration (2D Grid)", EditorStyles.boldLabel);

        // Draw a toggle for each element in building._configuration
        for (int row = 0; row < Building.ROW_MAX; row++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int col = 0; col < Building.COL_MAX; col++)
            {
                bool currentVal = building._configuration[row, col];
                bool newVal = EditorGUILayout.Toggle(currentVal, GUILayout.Width(20));
                if (newVal != currentVal)
                {
                    Undo.RecordObject(building, "Changed Configuration Value");
                    building._configuration[row, col] = newVal;
                    EditorUtility.SetDirty(building);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // Apply property modifications to the serializedObject
        // (This triggers OnBeforeSerialize/OnAfterSerialize if needed)
        serializedObject.ApplyModifiedProperties();

        // Draw the rest of the default inspector fields (if any)
        DrawPropertiesExcluding(serializedObject, "_serializedConfiguration");
    }
}