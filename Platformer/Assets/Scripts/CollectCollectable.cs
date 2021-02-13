using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCollectable : MonoBehaviour
{
    public AudioSource collectSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            collectSound.Play();
            ScoringSystem.theScore += 50;
            Destroy(gameObject);
        }

    }
}
