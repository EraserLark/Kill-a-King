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
    private Vector3 camPos;

    private bool isBeingHeld = false;
    private float timeElapsed = 0f;
    [SerializeField]
    private float turnTime = 0.3f;
    private IEnumerator currentCoroutine = null;

    [SerializeField]
    private Transform playerTransform;

    Ray lookRay;
    int layerMask;
    RaycastHit hit;
    Solid currentSolid;

    Vector3 gizLookDir;

    private void Awake()
    {
        camPos = new Vector3(camDist, camHeight, camDist);
        UpdateCamLookDir();

        layerMask = 1 << 8;
    }

    private void FixedUpdate()
    {
        Vector3 playerDir = (playerTransform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, playerDir, out hit, camDist, layerMask))
        {
            currentSolid = hit.transform.gameObject.GetComponent<Solid>();
            if(currentSolid != null)
            {
                currentSolid.FadeOut();
            }
        }
        else if(currentSolid)
        {
            currentSolid.FadeIn();
            currentSolid = null;
        }
    }

    public void UpdateCamLookDir()
    {
        Vector3 lookVec = ((Vector3.up * camOffset) - camPos).normalized;
        Quaternion lookDir = Quaternion.LookRotation(lookVec);
        transform.rotation = lookDir;
    }

    public void UpdateCameraRotation(float rotValue)
    {
        Quaternion camAng = Quaternion.AngleAxis(rotValue, Vector3.up);
        camPos = camAng * camPos;   //Rotate camera position by Quaternion
    }

    public void UpdateCamPosition(Transform playerTransform)
    {
        Vector3 playerCamPos = playerTransform.TransformPoint(camPos);
        transform.position = playerCamPos;

        this.playerTransform = playerTransform;
    }

    public void BeginHoldCamera()
    {
        isBeingHeld = true;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
    }
    public void HoldCamera(float rotAmt)
    {
        UpdateCameraRotation(rotAmt); //* turnSpeed)
        UpdateCamLookDir();
    }

    public Vector3 ReleaseCamera()
    {
        isBeingHeld = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency

        Vector3 playerForward = new Vector3(transform.forward.x, 0f, transform.forward.z);
        return playerForward;
    }

    public void ReorientCamera(Vector3 playerMoveDir)
    {
        if(!isBeingHeld)
        {
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            Vector3 camFlatDir = new Vector3(transform.forward.x, 0f, transform.forward.z);
            float dotProd = Vector3.Dot(playerMoveDir.normalized, camFlatDir.normalized);
            float angleDiffRad = Mathf.Acos(dotProd);

            //Determine if camera needs to rotate left/right to realign behind player
            float determinant = (playerMoveDir.x * camFlatDir.z) - (playerMoveDir.z * camFlatDir.x);
            if(determinant < 0)
            {
                angleDiffRad = -angleDiffRad;
            }

            float angDegTotal = angleDiffRad * Mathf.Rad2Deg;

            timeElapsed = 0f;
            currentCoroutine = AutoRepositionCam(angDegTotal);
            StartCoroutine(currentCoroutine);
        }
    }

    public IEnumerator AutoRepositionCam(float angDegTotal)
    {
        float angDegTally = 0f;

        while(timeElapsed < turnTime)
        {
            float angDegAmt = (angDegTotal / turnTime) * Time.deltaTime;
            UpdateCameraRotation(angDegAmt);
            UpdateCamLookDir();
            UpdateCamPosition(playerTransform);

            angDegTally += angDegAmt;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Leads to stutter at the end of turning?
        float angDegRemainder = angDegTotal - angDegTally;
        UpdateCameraRotation(angDegRemainder);
        UpdateCamLookDir();

        timeElapsed = 0f;
        currentCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 playerDir = (playerTransform.position - transform.position).normalized;
        Gizmos.DrawLine(transform.position, transform.position + playerDir * camDist);
    }
}