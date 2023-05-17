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
                Vector3 distToBase = homeBase - transform.position;
                moveDir = distToBase.normalized;

                if (distToBase.magnitude < 0.5f)
                {
                    currentState = EnemyState.SLEEP;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            currentState = EnemyState.CHASE;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            currentState = EnemyState.RETURN;
            target = null;
        }
    }
}
