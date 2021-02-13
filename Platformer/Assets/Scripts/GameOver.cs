using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ScoringSystem.liveScore--;

        if(ScoringSystem.liveScore == 0)
        {
            SceneManager.LoadScene(4);
        }

    }
}
