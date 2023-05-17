using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private float camDist = 6f;
    private float camHeight = 5f;
    private float turnSpeed = 5f;
    Vector3 camPos;

    //Gizmos
    Vector3 gizLookDir;

    private void Awake()
    {
        camPos = new Vector3(camDist, camHeight, camDist);
        UpdateCamLookDir();
    }

    public void UpdateCamLookDir()
    {
        Vector3 lookVec = ((Vector3.up * 2f) - camPos).normalized;  //2f = offset height off player center
        Quaternion lookDir = Quaternion.LookRotation(lookVec);
        transform.rotation = lookDir;
    }

    public void UpdateCamPosition(Transform playerTransform)
    {
        Vector3 trueCamPos = playerTransform.TransformPoint(camPos);   //Make relative to player
        transform.position = trueCamPos;
    }

    public void HoldCamera()
    {
        Time.timeScale = 0.5f;

        float rotAmt = turnSpeed * Input.GetAxis("Mouse X");
        Quaternion camAng = Quaternion.AngleAxis(rotAmt, Vector3.up);
        camPos = camAng * camPos;   //Rotate camPos by Quaternion

        UpdateCamLookDir();

        Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
    }

    public Vector3 ReleaseCamera()
    {
        Time.timeScale = 1f;
        Vector3 playerForward = new Vector3(transform.forward.x, 0f, transform.forward.z);

        Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
        //player.GetComponent<PlayerBase_1>().moveDir = flatForward;
        return playerForward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + gizLookDir);
    }
}
