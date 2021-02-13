using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [SerializeField]
    private Transform point;

    [SerializeField]
    private float X;

    [SerializeField]
    private float Y;

    [SerializeField]
    private float Z;

    [SerializeField]
    private float pointSpeed;

    [SerializeField]
    private float xAxis;
    [SerializeField]
    private float yAxis;
    [SerializeField]
    private float zAxis;

    [SerializeField]
    private bool keepRot;
    
    // Update is called once per frame
    void Update()
    {

        if (point != null)
        {
            Vector3 rot = new Vector3(xAxis * Time.deltaTime, yAxis * Time.deltaTime, zAxis * Time.deltaTime);

            transform.RotateAround(point.position, rot, pointSpeed * Time.deltaTime);
            
        }

        if (keepRot)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.Rotate(X * Time.deltaTime, Y * Time.deltaTime, Z * Time.deltaTime);
        }
        
    }
}
