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

        TestHitScript hit = component.GetComponent<TestHitScript>();
        if (hit != null)
        {
            hit.TakeHit();
        }
    }
}
