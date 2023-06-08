using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    private PlayerCam playerCam;
    private DebugPanel debugPanel;

    [SerializeField]
    [Range(.01f, 1f)]
    private float swipeSensitivity = 50f;

    public override void Init()
    {
        base.Init();
        isResting = false;
        moveDir = transform.forward;
    }

    private void Update()
    {
        //if(Input.GetButtonDown("Fire1"))
        //{
        //    playerCam.BeginHoldCamera();
        //    playerCam.HoldCamera();
        //}
        //else if (Input.GetButton("Fire1"))
        //{
        //    playerCam.HoldCamera();
        //}
        //else if (Input.GetButtonUp("Fire1"))
        //{
        //    moveDir = playerCam.ReleaseCamera();
        //    body.transform.rotation = Quaternion.LookRotation(moveDir);
        //}

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float rotAmt = touch.deltaPosition.x * swipeSensitivity;

            if (touch.phase == TouchPhase.Began)
            {
                playerCam.BeginHoldCamera();
                playerCam.HoldCamera(rotAmt);
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                playerCam.HoldCamera(rotAmt);
            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                moveDir = playerCam.ReleaseCamera();
                body.transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }

        playerCam.UpdateCamPosition(gameObject.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Solid"))
        {
            Vector3 normal = collision.GetContact(0).normal;
            float dot = Vector3.Dot(normal, moveDir);

            //Only reflect direction if player is facing towards wall
            if (dot < 0)
            {
                Vector3 newDir = moveDir - (2 * dot * normal);
                moveDir = newDir;

                //Negates speed loss while changing direction
                float bounceForce = prevVelocity.magnitude;
                Vector3 bounceVector = moveDir * bounceForce;

                rb.AddForce(bounceVector, ForceMode.VelocityChange);
                playerCam.ReorientCamera(moveDir);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
    }
}
