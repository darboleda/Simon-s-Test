using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;

public class SubweaponController : Behavior {
    public MovingModel Model;
    public tk2dSprite Sprite;

    public void Fire(Vector2 position, Vector2 velocityOffset, bool facingRight)
    {
        Sprite.FlipX = !facingRight;
        Model.Position = position;
        Model.Velocity.x *= (facingRight ? 1 : -1);
        Model.Velocity += velocityOffset;
    }

    public void OnTriggerEnter(Collider collider)
    {
        GameObject.Destroy(this.gameObject);

        Component component = collider;
        if (collider.attachedRigidbody != null)
        {
            component = collider.attachedRigidbody;
        }

        EnemyHurtBox hit = component.GetComponent<EnemyHurtBox>();
        if (hit != null)
        {
            hit.ReceiveHit(new EnemyHurtBox.AttackInfo() { AttackPower = 1 });
        }
    }

    public void OnBecameInvisible()
    {
        GameObject.Destroy(this.gameObject);
    }
}
