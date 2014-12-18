using UnityEngine;
using System.Collections;

using Canal.Unity;

public abstract class EnemyController : Behavior, IDamageable<int> 
{
    public GameObject EnemyDestroyedEffect;

    public virtual void Awake()
    {
        if (this.renderer == null)
        {
            this.gameObject.AddComponent<MeshRenderer>();
        }
    }

    public void OnBecameInvisible()
    {
        Despawn();
    }

    public abstract void TakeDamage(int damage);
    public void Despawn()
    {
        GameObject.Destroy(this.gameObject);
    }

    public virtual void Die()
    {
        GameObject destroyedEffect = GameObject.Instantiate(EnemyDestroyedEffect) as GameObject;
        Transform effectTransform = destroyedEffect.transform;
        effectTransform.parent = this.transform;
        effectTransform.localPosition = Vector3.zero;
        effectTransform.localScale = Vector3.one;
        effectTransform.localEulerAngles = Vector3.zero;
        effectTransform.parent = this.transform.parent;
        DeathController dc = destroyedEffect.GetComponent<DeathController>();
        if (dc != null)
        {
            dc.BeginAnimation(null);
        }
        this.Despawn();
    }
}
