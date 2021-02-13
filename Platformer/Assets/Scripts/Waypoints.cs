using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
