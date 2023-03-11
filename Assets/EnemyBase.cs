using UnityEngine;
using UnityEditor;

public class EnemyBase : MonoBehaviour
{
    private Rigidbody rb;
    GameObject gameObj;
    private GameObject enemyBody;
    [SerializeField]
    private SphereCollider detectColl;
    private enum EnemyState {IDLE, CHASE, RETURN };
    private EnemyState state = EnemyState.IDLE;

    public GameObject player;

    private float moveSpeed = 10f;
    private float maxSpeed = 8f;
    [SerializeField]
    private Vector3 moveDir = Vector3.zero;

    [SerializeField]
    private Vector3 homeBase;
    private Vector3 initialLookDir;
    private float detectRadius = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameObj = GetComponent<GameObject>();
        enemyBody = transform.GetChild(0).gameObject;
        homeBase = transform.position;
        initialLookDir = transform.forward;
        detectColl.radius = detectRadius;
    }

    private void FixedUpdate()
    {
        if(state != EnemyState.IDLE)
        {
            if(state == EnemyState.CHASE)
            {
                moveDir = (player.transform.position - transform.position).normalized;
            }
            else if(state == EnemyState.RETURN)
            {
                moveDir = (homeBase - transform.position).normalized;

                Vector3 roundedPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
                if (roundedPos == homeBase)
                {
                    state = EnemyState.IDLE;
                    moveDir = initialLookDir;
                }
            }

            
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

            Quaternion LookDir = Quaternion.LookRotation(moveDir, Vector3.up);
            enemyBody.transform.rotation = LookDir;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            state = EnemyState.CHASE;
            moveDir = player.transform.position;
        }
    }

    /*
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            state = EnemyState.RETURN;
            moveDir = homeBase;
        }
    }
    */

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + moveDir);

        //To - from
        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + playerDir);

        Vector3 homeDir = (homeBase - transform.position).normalized;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + homeDir);

        detectColl.radius = detectRadius;
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, detectRadius);
    }
}
