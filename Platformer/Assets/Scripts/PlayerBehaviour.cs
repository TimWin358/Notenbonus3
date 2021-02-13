using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    public MoveSettings moveSettings;
    public InputSettings inputSettings;

    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private float forwardInput, sidewaysInput, turnInput, jumpInput;

    public Transform[] checkpoints;
    private int currCheckpoint = 0;

    private Collider playerCol;

    private void Awake()
    {
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = 0;
        targetRotation = transform.rotation;

        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerCol = gameObject.GetComponent<Collider>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Turn();
        Run();
        Jump();
    }

    void GetInput()
    {
        if(inputSettings.FORWARD_AXIS.Length != 0)
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS); 
        if (inputSettings.SIDEWAYS_AXIS.Length != 0) 
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS); 
        if (inputSettings.TURN_AXIS.Length != 0) 
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS); 
        if (inputSettings.JUMP_AXIS.Length != 0) 
            jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
    }

    void Run()
    {
        velocity.z = forwardInput * moveSettings.runVelocity; 
        velocity.x = sidewaysInput * moveSettings.runVelocity;
        velocity.y = playerRigidbody.velocity.y;

        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > 0) { 
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVelocity * turnInput * Time.deltaTime, Vector3.up); 
        }

        transform.rotation = targetRotation;
    }

    void Jump()
    {
        if (jumpInput != 0 && Grounded())
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, moveSettings.jumpVelocity, playerRigidbody.velocity.z);
        }

    }

    public bool Grounded()
    {

        return Physics.Raycast(transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
        
    }

    void JumpedOnEnemy(float bumpSpeed)
    {
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, bumpSpeed, playerRigidbody.velocity.z);
    }

    void Spawn()
    {
        playerRigidbody.velocity = Vector3.zero;
        transform.position = checkpoints[currCheckpoint].transform.position;
    }

    void OnDeath()
    {
        Spawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Collider enemyCol = collision.gameObject.GetComponent<Collider>();
            //get collider for player (meh)

            if (enemy.invincible)
            {
                OnDeath();
            }
            else if(playerCol.bounds.center.y - playerCol.bounds.extents.y >
                    enemyCol.bounds.center.y + 0.5 * enemyCol.bounds.extents.y)
            {
                GameData.Instance.Score += 10;
                JumpedOnEnemy(enemy.bumpSpeed);
                enemy.OnDeath();
            }
            else
            {
                OnDeath();
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeathZone")
        {
            Spawn();
        }
        else if(other.tag == "Checkpoint")
        {
            currCheckpoint++;
            other.GetComponent<Collider>().enabled = false;
        }
        else if(other.tag == "Moveable")
        {
            transform.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Moveable")
        {
            transform.parent = null;
        }
    }

}

[System.Serializable]
public class MoveSettings {

    public float runVelocity = 12f;
    public float rotateVelocity = 100f;
    public float jumpVelocity = 8f;
    public float distanceToGround = 1.3f;
    public LayerMask ground;

}
[System.Serializable]
public class InputSettings {

    public string FORWARD_AXIS = "Vertical";
    public string SIDEWAYS_AXIS = "Horizontal";
    public string TURN_AXIS = "Mouse X";
    public string JUMP_AXIS = "Jump";

}
