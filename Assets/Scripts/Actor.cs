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

	public abstract Vector3 CalculateVelocity();
	public void Move(Vector3 targetVel) { rb.AddForce(targetVel, ForceMode.VelocityChange); }

	public void FixedUpdate()
    {
		Vector3 velocity = CalculateVelocity();
		Move(velocity);
    }
}
