using LuduInteraction.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace LuduInteraction.Editor
{
    public class SaveIDEditorWindow : EditorWindow
    {
        [MenuItem("LuduInteraction/Generate Save IDs")]
        public static void ShowWindow()
        {
            GetWindow<SaveIDEditorWindow>("Save ID Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Generate Unique IDs for Interactables", EditorStyles.boldLabel);

            if (GUILayout.Button("Generate IDs for Scene Objects"))
            {
                GenerateIDs();
            }
        }

        private void GenerateIDs()
        {
            BaseInteractable[] interactables = FindObjectsByType<BaseInteractable>(FindObjectsSortMode.None);
            int count = 0;

            foreach (var interactable in interactables)
            {
                // Accessing private field via SerializedObject to support Undo
                SerializedObject so = new SerializedObject(interactable);
                SerializedProperty prop = so.FindProperty("m_SaveID");

                if (string.IsNullOrEmpty(prop.stringValue))
                {
                    prop.stringValue = System.Guid.NewGuid().ToString();
                    so.ApplyModifiedProperties();
                    count++;
                }
            }

            Debug.Log($"Generated IDs for {count} objects.");
        }
    }
}
