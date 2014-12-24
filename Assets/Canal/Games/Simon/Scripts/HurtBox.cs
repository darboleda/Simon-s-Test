using UnityEngine;
using System.Collections;

using Canal.Unity;

public class HurtBox : Behavior
{
    public struct AttackInfo
    {
        public int AttackPower;
    }

    [InterfaceBehavior(typeof(IDamageable<int>))]
    public Behavior Owner;

    public void ReceiveHit(AttackInfo info)
    {
        ((IDamageable<int>)Owner).TakeDamage(info.AttackPower);
    }
}
