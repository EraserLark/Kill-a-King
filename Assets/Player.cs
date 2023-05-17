using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    private PlayerCam playerCam;
    private DebugPanel debugPanel;

    public void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
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

    public override Vector3 CalculateVelocity()
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

        //Rotate Body
        Quaternion lookRot = Quaternion.LookRotation(moveDir, Vector3.up);
        body.transform.rotation = lookRot;

        //DEBUG
        //rbVelocity = rb.velocity.magnitude;
        //debugPanel.UpdateSpeed(rbVelocity);

        return targetVel;
    }
}
