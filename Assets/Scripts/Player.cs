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
}
