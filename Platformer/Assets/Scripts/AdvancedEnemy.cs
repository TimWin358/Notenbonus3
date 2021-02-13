using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class AdvancedEnemy : MonoBehaviour
{
    [SerializeField]
    private float ChaseSpeed;
    [SerializeField]
    private float NormalSpeed;
    [SerializeField]
    private GameObject Prey;
    private Rigidbody enemyRB;
    private Rigidbody preyRB;

    [SerializeField]
    private List<Waypoints> waypoints;
    private int currentwaypoint = 0;
    [SerializeField]
    private float distanceThreshold;

    [SerializeField]
    private float chaseEvadeDistance;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody>();
        preyRB = Prey.GetComponent<Rigidbody>();
    }

    public enum Behaviour
    {
        LineOfSight,
        Intercept,
        PatternMovement,
        ChasePatternMovement,
        Hide
    }

    public Behaviour behaviour;

    private void FixedUpdate()
    {
        switch (behaviour)
        {
            case Behaviour.LineOfSight: //Exercise 1
                ChaseLineOfSight(Prey.transform.position, ChaseSpeed);
                break;
            case Behaviour.Intercept: //Exercise 2
                Intercept(Prey.transform.position);
                break;
            case Behaviour.PatternMovement: //Exercise 3
                PatternMovement();
                break;
            case Behaviour.ChasePatternMovement: //Exercise 4
                if(Vector3.Distance(transform.position, Prey.transform.position) < chaseEvadeDistance)
                {
                    ChaseLineOfSight(Prey.transform.position, ChaseSpeed);
                }
                else
                {
                    PatternMovement();
                }
                break;
            case Behaviour.Hide: //Exercise 5
                if (PlayerVisible(preyRB.transform.position)) 
                { 
                    ChaseLineOfSight(Prey.transform.position, ChaseSpeed); 
                } 
                else { 
                    //Mach nix
                    //PatternMovement(); 
                }
                break;
        }
    }

    private void ChaseLineOfSight(Vector3 targetPos, float speed)
    {
        //Normalisiert Vector damit Speed nicht whicky whacky ist
        Vector3 direction = (targetPos - transform.position).normalized;

        enemyRB.velocity = new Vector3(direction.x * speed, enemyRB.velocity.y, direction.z * speed);
    }

    private void Intercept(Vector3 targetPos)
    {
        Vector3 enemyPos = transform.position;
        Vector3 predictedInterceptionPoint;
        float timeToClose, distance, relativeVelocity;

        relativeVelocity = (preyRB.velocity - enemyRB.velocity).magnitude;
        distance = (targetPos - enemyPos).magnitude;
        timeToClose = distance / relativeVelocity;

        predictedInterceptionPoint = targetPos + timeToClose * preyRB.velocity;

        Vector3 direction = (predictedInterceptionPoint - enemyPos).normalized;
        enemyRB.velocity = new Vector3(direction.x * ChaseSpeed, enemyRB.velocity.y, direction.z * ChaseSpeed);
        
    }

    private void PatternMovement()
    {
        Vector3 waypointPos = waypoints[currentwaypoint].transform.position;

        ChaseLineOfSight(waypointPos, NormalSpeed);

        if(Vector3.Distance(transform.position, waypointPos) < distanceThreshold)
        {
            currentwaypoint = (currentwaypoint + 1) % waypoints.Count;
        }
    }

    private bool PlayerVisible(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - gameObject.transform.position;
        directionToTarget.Normalize();

        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, directionToTarget, out hit);

        return hit.collider.gameObject.tag.Equals("Player");
    }

    

}
