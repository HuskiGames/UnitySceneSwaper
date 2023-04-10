//Made By Huski

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Events;
using UnityEditor;
using System.Threading.Tasks;

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
    UnityEvent m_MyEvent;
    bool ContainsInScene = true;
    string selpath;
    Scene selscene;
    bool Saving;
    bool ExitSaveButon = true;


    [System.Obsolete]
    private void OnGUI()
    {
        if (Application.isPlaying || Saving)
        {
            if (Saving)
            {
                EditorGUILayout.LabelField("Saving");
                ForceExitSave();
                if (ExitSaveButon)
                {
                    if (GUILayout.Button("Force Exit Save"))
                    {
                        Saving = false;
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("Exit play mode to use window");
            }
        }
        else
        {
            ExitSaveButon = false;
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


                    ContainsInScene = true;
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
                                Saving = true;
                                EditorSceneManager.SaveScenes(scenes);
                                selscene = sceneArray[ii];
                                waitClose();
                            }
                        }
                    }
                    else
                    {
                        Saving = true;
                        EditorSceneManager.SaveScenes(scenes);
                        selpath = Paths[ii];
                        waitSave();
                    }
                }
            }
        }
    }

    async void ForceExitSave()
    {
        await Task.Delay(200);
        if (Saving == true)
        {
            ExitSaveButon = true;
        }
    }

    async void waitSave()
    {
        await Task.Delay(200);
        EditorSceneManager.OpenScene(selpath);
        Saving = false;
    }
    async void waitClose()
    {
        await Task.Delay(200);
        closeScene(selscene);
        Saving = false;
    }

    void closeScene(Scene scene)
    {
        EditorSceneManager.CloseScene(scene, true);
    }
}
