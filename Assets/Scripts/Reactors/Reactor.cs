using UnityEngine;

public abstract class Reactor : MonoBehaviour
{

}

public abstract class CollideReactor : Reactor
{
    public virtual void CollEnter(Collision collision) { }
    public virtual void CollExit(Collision collision) { }

    private void OnCollisionEnter(Collision collision)
    {
        CollEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        CollExit(collision);
    }
}

public abstract class TriggerReactor : Reactor
{
    public virtual void TrigEnter(Collider other) { }
    public virtual void TrigExit(Collider other) { }

    private void OnTriggerEnter(Collider other)
    {
        TrigEnter(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        TrigExit(other);
    }
}