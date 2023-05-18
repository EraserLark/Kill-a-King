using UnityEngine;

public class Bumper : CollideReactor
{
    [SerializeField]
    private float upwardForce = 0.5f;
    [SerializeField]
    private float bounceForce = 500f;

    public override void CollEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 normal = -collision.GetContact(0).normal;
            Vector3 bounceHeight = Vector3.up * upwardForce;
            Vector3 bounceVector = (normal + bounceHeight) * bounceForce;

            rb.AddForce(bounceVector, ForceMode.Acceleration);
        }
    }
}
