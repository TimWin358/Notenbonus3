using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    private Transform pointA;
    private Transform pointB;
    [SerializeField]
    private float speed;
    private float distance;
    private int counter = 0;

    private void Start()
    {
        pointA = points[0];
        pointB = points[1];
    }

    // Update is called once per frame
    void Update()
    {
        distance += speed * Time.deltaTime;

        transform.position = Vector3.Lerp(pointA.position, pointB.position, distance);

        if(distance > 1)
        {
            distance = 0;

            counter++;

            if(counter == points.Length - 1)
            {
                pointA = points[counter];
                pointB = points[0];

                counter = -1;
            }
            else
            {
                pointA = points[counter];
                pointB = points[counter + 1];
            }

            /*
            Transform pointSwitch = pointA;
            pointA = pointB;
            pointB = pointSwitch;
            */
        }

    }

    //Dieser Teil bewegt den Player mit der Platform mit
    //(Leider um eine Frame Verzögerung aber besser als nichts ;) )
    private Vector3 lastFrame;
    private Vector3 currentFrame;

    private void OnTriggerStay(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            currentFrame = transform.position;

            if (lastFrame != Vector3.zero)
            {
                other.transform.position += currentFrame - lastFrame;
            }

            lastFrame = transform.position;
        }

    }
}
