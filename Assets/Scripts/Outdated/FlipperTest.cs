using UnityEngine;

public class FlipperTest : MonoBehaviour
{
    private BoxCollider triggerColl;

    private HingeJoint joint;
    private JointMotor motor;

    [SerializeField]
    private float motorForce = 2500f;
    [SerializeField]
    private float motorVel = 900f;
    [SerializeField]
    private bool motorFreeSpin = false;

    private void Awake()
    {
        triggerColl = GetComponents<BoxCollider>()[1];  //Get trigger collider, not other boxColl

        joint = GetComponent<HingeJoint>();
        motor = joint.motor;

        motor.force = motorForce;
        motor.targetVelocity = motorVel;
        motor.freeSpin = motorFreeSpin;
        joint.motor = motor;
    }

    private void OnTriggerEnter(Collider other)
    {
        motor.targetVelocity = motorVel;
        joint.motor = motor;

        joint.useMotor = true;
        triggerColl.enabled = false;

        Invoke("ResetPaddle", 1f);
    }

    private void ResetPaddle()
    {
        motor.targetVelocity = -motorVel;
        joint.motor = motor;
        //joint.useMotor = true;

        triggerColl.enabled = true;
    }
}
