using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject player;
    public float camDist = 5f;
    public float camHeight = 10f;
    public float turnSpeed = 5f;
    Vector3 playerCenter;
    Vector3 camPos;

    //Gizmos
    Vector3 gizLookDir;

    private void Awake()
    {
         camPos = new Vector3(camDist, camHeight, camDist);

        //Copy of code run when swiveling camera, just to set it up right at start
        Vector3 lookVec = ((Vector3.up * 2f) - camPos).normalized;
        Quaternion lookDir = Quaternion.LookRotation(lookVec);
        transform.rotation = lookDir;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Time.timeScale = 0.5f;

            float rotAmt = turnSpeed * Input.GetAxis("Mouse X");
            Quaternion camAng = Quaternion.AngleAxis(rotAmt, Vector3.up);
            camPos = camAng * camPos;   //Rotate camPos by Quaternion

            Vector3 lookVec = ((Vector3.up * 2f) - camPos).normalized;  //2f = offset height off player center
            Quaternion lookDir = Quaternion.LookRotation(lookVec);
            transform.rotation = lookDir;

            Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Time.timeScale = 1f;
            //player.GetComponent<PlayerBase_1>().moveDir = transform.forward;  //Slows down b/c forward is angled towards ground for cam!!
            //How to get the 'forward' direction without caring about the y...
            Vector3 flatForward = new Vector3(transform.forward.x, 0f, transform.forward.z);
            player.GetComponent<PlayerBase_1>().moveDir = flatForward;

            //Rotate playerBody
            Transform playerBody = player.transform.GetChild(0);
            playerBody.rotation = Quaternion.LookRotation(flatForward);

            Time.fixedDeltaTime = 0.02f * Time.timeScale;   //For consistency
        }

        Vector3 trueCamPos = player.transform.TransformPoint(camPos);   //Make relative to player
        transform.position = trueCamPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + gizLookDir);
    }
}
