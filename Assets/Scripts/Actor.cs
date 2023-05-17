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
            float speedDiff = currentSpeed - maxSpeed;
            targetVel *= speedDiff;
        }
        else
        {
            targetVel *= moveSpeed;
        }

        //Rotate Body
        Quaternion lookRot = Quaternion.LookRotation(moveDir, Vector3.up);
        body.transform.rotation = lookRot;

        //DEBUG
        //rbVelocity = rb.velocity.magnitude;
        //debugPanel.UpdateSpeed(rbVelocity);

        return targetVel;
    }

	public void Move(Vector3 targetVel) { rb.AddForce(targetVel, ForceMode.VelocityChange); }

	public void FixedUpdate()
    {
		Vector3 velocity = CalculateVelocity();
		Move(velocity);
    }
}
