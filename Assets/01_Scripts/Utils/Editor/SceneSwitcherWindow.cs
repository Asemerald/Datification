using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils.Editor
{
    public class SceneSwitcherWindow : EditorWindow
    {
        private string[] filteredScenePaths;

        [MenuItem("Tools/SceneSwitcher")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneSwitcherWindow>();
            window.titleContent = new GUIContent("Scene Switcher");
            window.Show();
        }

        private void OnEnable()
        {
            RefreshScenes();
        }

        private void RefreshScenes()
        {
            // Find all scenes in the project, excluding "Others" and package folders
            filteredScenePaths = AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => !path.Contains("/Others/") && !path.StartsWith("Packages/"))
                .ToArray();
        }

        private void OnGUI()
        {
            if (filteredScenePaths == null || filteredScenePaths.Length == 0)
            {
                EditorGUILayout.HelpBox("No scenes found. Please add scenes to the project.", MessageType.Info);
                if (GUILayout.Button("Refresh"))
                {
                    RefreshScenes();
                }
                return;
            }

            // Dynamic button layout
            float buttonWidth = 150f;
            float buttonHeight = 30f;
            float spacing = 10f;

            float windowWidth = position.width;
            int buttonsPerRow = Mathf.FloorToInt((windowWidth + spacing) / (buttonWidth + spacing));

            if (buttonsPerRow < 1) buttonsPerRow = 1;

            int currentRow = 0;
            for (int i = 0; i < filteredScenePaths.Length; i++)
            {
                if (i % buttonsPerRow == 0)
                {
                    if (i > 0) GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(10); // Add padding on the left
                    currentRow++;
                }

                string scenePath = filteredScenePaths[i];
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                if (GUILayout.Button(sceneName, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
                {
                    OpenScene(scenePath);
                }
            }

            // Close the last row
            if (filteredScenePaths.Length > 0) GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh Scenes", GUILayout.Width(150f), GUILayout.Height(30f)))
            {
                RefreshScenes();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private static void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}
