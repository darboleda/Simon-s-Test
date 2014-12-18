using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;

public class SimonModel : WalkerModel {
    public enum MovementState
    {
        Standing,
        Crouching,
        InAir
    }
        
    public bool TakingDamage;
    public bool Attacking;
    public MovementState Movement;

	public GameObject HolyWaterPrefab;
}
