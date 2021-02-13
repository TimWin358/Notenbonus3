using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffPlatform : MonoBehaviour
{

    [SerializeField]
    private bool invisible;

    [SerializeField]
    private bool falling;
    private bool fallNow;

    private Transform originalPosition;

    private void Awake()
    {
        originalPosition = this.transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (invisible)
            {
                StartCoroutine(Invisible());
            }
            else if (falling)
            {
                StartCoroutine(Falling());
            }
        }
    }

    private IEnumerator Blinking()
    {
        int blinkCount = 0;

        while (blinkCount < 10)
        {
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;

            if (GetComponent<Renderer>().enabled)
            {
                blinkCount++;
            }

            yield return new WaitForSeconds(0.2f);
        }

    }

    private IEnumerator Invisible()
    {
        yield return StartCoroutine(Blinking());

        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(5);

        GetComponent<Renderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    private IEnumerator Falling()
    {
        yield return StartCoroutine(Blinking());

        Transform ogPos = this.transform;

        this.gameObject.AddComponent<Rigidbody>();

        yield return new WaitForSeconds(5);

        Destroy(this);

    }

}