using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    private PlayerCam playerCam;
    private DebugPanel debugPanel;

    public override void Init()
    {
        base.Init();
        isResting = false;
        moveDir = transform.forward;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            playerCam.HoldCamera();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            moveDir = playerCam.ReleaseCamera();
            body.transform.rotation = Quaternion.LookRotation(moveDir);
        }

        playerCam.UpdateCamPosition(gameObject.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Solid")
        {
            Vector3 normal = collision.GetContact(0).normal;
            float dot = Vector3.Dot(normal, moveDir);

            if (dot < 0)
            {
                //Calculate reflected angle
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
}
