using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 speed = new Vector3(0, 0, -6);
    public bool invincible = false;
    private Rigidbody enemyRB;
    public float bumpSpeed = 20;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(speed.x == 0)
        {
            enemyRB.constraints = RigidbodyConstraints.FreezePositionX;
        }
        else if(speed.z == 0)
        {
            enemyRB.constraints = RigidbodyConstraints.FreezePositionZ;
        }


        enemyRB.velocity = new Vector3(speed.x, enemyRB.velocity.y + speed.y, speed.z);
    }

    public void OnDeath()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "End")
        {
            speed *= -1;
        }
    }
}
