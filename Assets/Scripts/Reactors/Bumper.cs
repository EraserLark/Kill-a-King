using UnityEngine;

public class Bumper : CollideReactor
{
    public override void CollEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 normal = -collision.GetContact(0).normal;
            Vector3 bounceHeight = Vector3.up * 0.5f;   //bounceUp = 0.5f;
            Vector3 bounceVector = (normal + bounceHeight) * 500f * rb.mass;  //bounceForce = 500f;

            rb.AddForce(bounceVector);
        }
    }
}
