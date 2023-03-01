using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase_1 : MonoBehaviour
{
    public Rigidbody rb;
    Camera playerCam;
    GameObject playerBody;


    public float turnSpeed = 5f;
    public float moveSpeed = 10f;
    public float maxSpeed = 8f;
    public Vector3 moveDir = Vector3.zero;
    [Header("Stats")]
    public float rbVelocity = 0f;
    public float targetVelMag = 0f;

    //Debug
    public DebugPanel debugPanel;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
        playerBody = transform.GetChild(0).gameObject;
        //playerBody.GetComponent<Rigidbody>().detectCollisions = false;  //I think this works to stop playerBody from going crazy
        //Physics.IgnoreCollision(playerBody.GetComponent<CapsuleCollider>(), gameObject.GetComponent<CapsuleCollider>(), true);

        moveDir = transform.forward;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Method 2 -- Apply force
        float currentSpeed = rb.velocity.magnitude;
        Vector3 targetVel = moveDir * Time.deltaTime;

        //Rough speed capping
        if (currentSpeed > maxSpeed)
        {
            float speedDiff = currentSpeed - maxSpeed;
            targetVel *= speedDiff;
        }
        else
        {
            targetVel *= moveSpeed;
        }

        rb.AddForce(targetVel, ForceMode.VelocityChange);

        //Rotate Body
        Quaternion lookRot = Quaternion.LookRotation(moveDir, Vector3.up);
        playerBody.transform.rotation = lookRot;

        //DEBUG
        rbVelocity = rb.velocity.magnitude;
        debugPanel.UpdateSpeed(rbVelocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);    //DEBUG

        if (collision.gameObject.tag == "Solid")
        {
            //RICOCHET METHOD
            //Store current vel, stop player
            float prevSpeed = rb.velocity.magnitude;
            rb.velocity = Vector3.zero;

            //Calculate reflected angle
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 playerDir = transform.forward;

            float dot = Vector3.Dot(normal, playerDir);
            Vector3 newDir = playerDir - (2 * dot * normal);

            moveDir = newDir;

            //Add back in new speed
            float currentSpeed = prevSpeed;
            Vector3 targetVel = moveDir * Time.deltaTime;

            //Rough speed capping
            if (currentSpeed > maxSpeed)
            {
                float speedDiff = currentSpeed - maxSpeed;
                targetVel *= speedDiff;
            }
            else
            {
                targetVel *= moveSpeed;
            }

            rb.velocity = targetVel;

            //Trying bounce backwards, not working yet...
            //Vector3 bounceForce = newDir * 100f;
            //rb.AddForce(bounceForce);
        }
        else if (collision.gameObject.tag == "Entity")
        {
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 bounceHeight = Vector3.up * 0.5f;   //bounceUp = 0.5f;
            Vector3 bounceVector = (normal + bounceHeight) * 250f;  //bounceForce = 500f;
            rb.AddForce(bounceVector);
        }
        else if (collision.gameObject.tag == "Bumper")
        {
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 bounceHeight = Vector3.up * 0.5f;   //bounceUp = 0.5f;
            Vector3 bounceVector = (normal + bounceHeight) * 500f;  //bounceForce = 500f;
            rb.AddForce(bounceVector);
        }
        //Duplicate for bouncy objects, but have player change direction too!
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
    }
}
