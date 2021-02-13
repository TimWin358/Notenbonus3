using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{

    [SerializeField]
    private int sceneIndex;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneIndex);
    }

}
