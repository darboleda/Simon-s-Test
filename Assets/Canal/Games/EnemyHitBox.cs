using UnityEngine;
using System.Collections;

using Canal.Unity;

public class EnemyHitBox : Behavior {

    public void OnTriggerStay(Collider collider)
    {
        Component component = collider;
        if (collider.attachedRigidbody != null)
        {
            component = collider.attachedRigidbody;
        }

        PlayerHurtBox hit = component.GetComponent<PlayerHurtBox>();
        if (hit != null)
        {
            hit.ReceiveHit(new EnemyHurtBox.AttackInfo() { AttackPower = 2 });
        }
    }
}
