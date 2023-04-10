//Made By Huski

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public class SceneChanger : EditorWindow
{
    [MenuItem("Window/General/Scene Changer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneChanger));
    }
    List<Scene> sceneArray;
    public string[] Paths;
    bool Add;


    [System.Obsolete]
    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            EditorGUILayout.LabelField("Exit play mode to use window");
        }
        else
        {

            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty stringsProperty = so.FindProperty("Paths");
            EditorGUILayout.PropertyField(stringsProperty, true);
            so.ApplyModifiedProperties();

            if (sceneArray.Count != Paths.Length)
            {
                sceneArray.Clear();
                for (int i = 0; i < Paths.Length; i++)
                {
                    sceneArray.Add(new Scene());
                }
            }

            Scene[] scenes = EditorSceneManager.GetAllScenes();

            for (int ii = 0; ii < sceneArray.Count; ii++)
            {
                if (GUILayout.Button((Paths[ii].Replace("Assets/Scenes/", "")).Replace(".unity", "")))
                {
                    if (Event.current.button == 1)
                    {
                        Add = true;
                    }
                    else
                    {
                        Add = false;
                    }


                    bool ContainsInScene = true;
                    foreach (Scene scene in scenes)
                    {
                        if (scene.name == sceneArray[ii].name)
                        {
                            ContainsInScene = false;
                        }
                    }

                    if (Add)
                    {
                        if (ContainsInScene)
                        {
                            sceneArray[ii] = EditorSceneManager.OpenScene(Paths[ii], OpenSceneMode.Additive);
                        }
                        else
                        {
                            if (EditorSceneManager.sceneCount > 1)
                            {
                                EditorSceneManager.CloseScene(sceneArray[ii], true);
                            }
                        }
                    }
                    else
                    {
                        sceneArray[ii] = EditorSceneManager.OpenScene(Paths[ii]);
                    }
                }
            }
        }
    }
}
