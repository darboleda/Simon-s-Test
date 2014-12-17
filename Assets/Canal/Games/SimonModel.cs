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
        
    public bool Attacking;
    public MovementState Movement;
    public bool FacingRight = true;

	public GameObject HolyWaterPrefab;
}
