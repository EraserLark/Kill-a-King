using UnityEngine;

public class Ramp : CollideReactor
{
    public override void CollEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 rampForce = transform.forward * 3f;
            rb.AddForce(rampForce, ForceMode.VelocityChange);
        }
    }
}
