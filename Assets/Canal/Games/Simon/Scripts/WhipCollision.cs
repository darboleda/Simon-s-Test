using UnityEngine;
using System.Collections;

using Canal.Unity;

public class WhipCollision : Behavior {

    public void OnTriggerEnter(Collider collider)
    {
        Component component = collider;
        if (collider.attachedRigidbody != null)
        {
            component = collider.attachedRigidbody;
        }

        EnemyHurtBox hit = component.GetComponent<EnemyHurtBox>();
        if (hit != null)
        {
            hit.ReceiveHit(new EnemyHurtBox.AttackInfo() { AttackPower = 2 });
        }
    }
}
