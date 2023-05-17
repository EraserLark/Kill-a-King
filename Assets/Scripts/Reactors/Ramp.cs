using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : CollideReactor
{
    public override void CollEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 rampForce = transform.forward * 3f * rb.mass;
            rb.AddForce(rampForce, ForceMode.VelocityChange);
        }
    }
}
