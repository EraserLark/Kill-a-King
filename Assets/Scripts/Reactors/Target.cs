using UnityEngine;

public class Target : TriggerReactor
{
    public override void TrigEnter(Collider other)
    {
        ScoreManager.instance.UpdateScore(100);
        Destroy(gameObject);
    }
}
