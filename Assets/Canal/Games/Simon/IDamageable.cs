using UnityEngine;
using System.Collections;

using Canal.Unity;

public interface IDamageable<TValue>
{
    void TakeDamage(TValue damage);
}
