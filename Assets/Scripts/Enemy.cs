using UnityEngine;

public class Enemy : Actor
{
    enum EnemyState { SLEEP, CHASE, RETURN };
    private EnemyState currentState;
    private Vector3 homeBase;
    private GameObject target;

    public void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        currentState = EnemyState.SLEEP;
        homeBase = gameObject.transform.position;
    }

    private void Update()
    {
        switch(currentState)
        {
            case EnemyState.SLEEP:
                moveDir = Vector3.zero;
                break;
            case EnemyState.CHASE:
                moveDir = (target.transform.position - transform.position).normalized;
                break;
            case EnemyState.RETURN:
                moveDir = (homeBase - transform.position).normalized;

                Vector3 roundedPos = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
                if (roundedPos == homeBase)
                {
                    currentState = EnemyState.SLEEP;
                    moveDir = Vector3.forward;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            currentState = EnemyState.CHASE;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            currentState = EnemyState.RETURN;
            target = null;
        }
    }
}
