using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SpeedSceneLoader : EditorWindow
{
    private string sceneName;

    [MenuItem("Window/SceneLoader")]
    public static void ShowWindow()
    {
        GetWindow<SpeedSceneLoader>("SpeedSceneLoader");
    }

    private void OnGUI() 
    {
        EditorGUILayout.BeginVertical();
        
        for(int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            string path = EditorBuildSettings.scenes[i].path;
            string[] pathSplit = path.Split('/','.');

            sceneName = pathSplit[pathSplit.Length - 2];
            
            if (GUILayout.Button(sceneName))
            {
                EditorSceneManager.OpenScene(path);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}
