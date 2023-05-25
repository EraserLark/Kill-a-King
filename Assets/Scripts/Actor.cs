using UnityEngine;

abstract public class Actor : MonoBehaviour
{
	[SerializeField]
	protected Rigidbody rb;
	[SerializeField]
	protected GameObject body;
	[SerializeField]
	protected float moveSpeed;
	[SerializeField]
	protected float maxSpeed;
    protected Vector3 moveDir;
    protected bool isResting;
    protected Vector3 prevVelocity;

    public void Awake()
    {
        Init();
    }

    public virtual void Init()
	{
		rb = gameObject.GetComponent<Rigidbody>();

		moveSpeed = 10f;
		maxSpeed = 10f;
		moveDir = Vector3.zero;
	}

	public virtual Vector3 CalculateVelocity()
    {
        float currentSpeed = rb.velocity.magnitude;
        Vector3 targetVel = moveDir * Time.deltaTime;

        //Rough speed capping
        if (currentSpeed > maxSpeed)
        {
            float dot = Vector3.Dot(rb.velocity.normalized, moveDir);
            float speedDiff = currentSpeed - maxSpeed;

            if(dot > 0)
            {
                speedDiff = -speedDiff;
            }

            targetVel *= speedDiff;
        }
        else
        {
            targetVel *= moveSpeed;
        }

        //Rotate Body
        Quaternion lookRot = Quaternion.LookRotation(moveDir, Vector3.up);
        body.transform.rotation = lookRot;

        return targetVel;
    }

	public void Move(Vector3 targetVel) { rb.AddForce(targetVel, ForceMode.VelocityChange); }

	public void FixedUpdate()
    {
        if(!isResting)
        {
		    Vector3 velocity = CalculateVelocity();
		    Move(velocity);
            prevVelocity = rb.velocity;
        }
    }
}
