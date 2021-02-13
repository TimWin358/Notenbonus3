using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;
    public static int theScore;

    [SerializeField]
    private Text liveText;
    public static int liveScore;

    private void Start()
    {
        theScore = 0;
        liveScore = 3;
    }

    private void Update()
    {
        scoreText.GetComponent<Text>().text = string.Concat("Score: ", theScore); //Update Score on Screen!

        liveText.text = string.Concat("Lives: ", liveScore); //Update Lives on Screen!
    }
}
