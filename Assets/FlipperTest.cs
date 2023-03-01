using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperTest : MonoBehaviour
{
    private BoxCollider triggerColl;

    private HingeJoint joint;
    private JointMotor motor;

    private void Awake()
    {
        triggerColl = GetComponents<BoxCollider>()[1];  //Get trigger collider, not other boxColl

        joint = GetComponent<HingeJoint>();
        motor = joint.motor;

        motor.force = 1000;
        motor.targetVelocity = 900;
        motor.freeSpin = false;
        joint.motor = motor;
    }

    private void OnTriggerEnter(Collider other)
    {
        joint.useMotor = true;
        triggerColl.enabled = false;

        Invoke("ResetPaddle", 1f);
    }

    private void ResetPaddle()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        print("Exit");
        motor.force = -1000;
        motor.targetVelocity = -900;
        Invoke("StopMotor", 1f);
    }

    private void StopMotor()
    {
        joint.useMotor = false;
    }
}
