using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenScene : MonoBehaviour
{
    
    [MenuItem("OpenScene/Start Screen")]
    static void StartScreen()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/StartScreen.unity");
        }
    }

    [MenuItem("OpenScene/Level 1")]
    static void Level1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
        }
    }


}
