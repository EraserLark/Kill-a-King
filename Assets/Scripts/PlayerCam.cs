using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private float camDist = 6f;
    [SerializeField]
    private float camHeight = 5f;
    [SerializeField]
    private float camOffset = 2f;
    [SerializeField]
    private const float turnSpeed = 5f;
    private Vector3 camPos;

    private bool isBeingHeld = false;
    private float timeElapsed = 0f;
    [SerializeField]
    private float turnTime = 0.3f;
    private IEnumerator currentCoroutine = null;

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

    public void UpdateCameraRotation(float rotValue)
    {
        float rotTotal = turnSpeed * rotValue;
        Quaternion camAng = Quaternion.AngleAxis(rotTotal, Vector3.up);
        camPos = camAng * camPos;   //Rotate camPos by Quaternion
    }

    public void UpdateCamPosition(Transform playerTransform)
    {
        Vector3 trueCamPos = playerTransform.TransformPoint(camPos);   //Make relative to player
        transform.position = trueCamPos;
    }

    public void HoldCamera()
    {
        isBeingHeld = true;
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        Time.timeScale = 0.5f;

        UpdateCameraRotation(Input.GetAxis("Mouse X"));
        UpdateCamLookDir();

        Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
    }

    public Vector3 ReleaseCamera()
    {
        isBeingHeld = false;
        Time.timeScale = 1f;

        Vector3 playerForward = new Vector3(transform.forward.x, 0f, transform.forward.z);

        Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
        return playerForward;
    }

    public void ReorientCamera(Vector3 playerMoveDir)
    {
        if(!isBeingHeld)
        {
            Vector3 flatCamDir = new Vector3(transform.forward.x, 0f, transform.forward.z);
            float dotProd = Vector3.Dot(playerMoveDir.normalized, flatCamDir.normalized);
            float angleDiffRad = Mathf.Acos(dotProd);

            float determinant = (playerMoveDir.x * flatCamDir.z) - (playerMoveDir.z * flatCamDir.x);
            if(determinant < 0)
            {
                angleDiffRad = -angleDiffRad;
            }

            float angDegAmt = (angleDiffRad * Mathf.Rad2Deg) / turnTime / turnSpeed;

            timeElapsed = 0f;
            currentCoroutine = AutoMoveCam(angDegAmt);
            StartCoroutine(currentCoroutine);
        }
    }

    public IEnumerator AutoMoveCam(float angDegAmt)
    {
        while(timeElapsed < turnTime)
        {
            UpdateCameraRotation(angDegAmt * Time.deltaTime);
            UpdateCamLookDir();

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        currentCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + gizLookDir);
    }
}
