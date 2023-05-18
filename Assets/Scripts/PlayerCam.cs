using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private float camDist = 6f;
    [SerializeField]
    private float camHeight = 5f;
    [SerializeField]
    private float camOffset = 2f;
    [SerializeField]
    private float turnSpeed = 5f;
    private Vector3 camPos;

    //Gizmos
    Vector3 gizLookDir;

    private void Awake()
    {
        camPos = new Vector3(camDist, camHeight, camDist);
        UpdateCamLookDir();
    }

    public void UpdateCamLookDir()
    {
        Vector3 lookVec = ((Vector3.up * camOffset) - camPos).normalized;
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
        return playerForward;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + gizLookDir);
    }
}
