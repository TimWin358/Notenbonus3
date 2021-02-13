using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;
    public static int theScore;
  

    private void Update()
    {
        scoreText.GetComponent<Text>().text = string.Concat("Score: ", theScore); //Update Score on Screen!
    }
}
