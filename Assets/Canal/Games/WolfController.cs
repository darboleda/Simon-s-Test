using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;

public class WolfController : EnemyController {

    public WalkerModel Model;
    public HealthModel Health;
    public WolfAnimator Animator;

    public override void Awake()
    {
        base.Awake();
        Model.FacingRight = false;
    } 

    public void Update()
    {
        Model.Collision.DisableGroundCling = false;

        if (Model.FacingRight && Model.Collision.RightCollision)
        {
            Model.FacingRight = false;
        }
        else if (!Model.FacingRight && Model.Collision.LeftCollision)
        {
            Model.FacingRight = true;
        }

        if (Model.Collision.OnGround)
        {
            Animator.Walk();
            if (Random.Range(0f, 1.0f) > 0.99f)
            {
                Model.Velocity.y = Mathf.Sqrt(Model.JumpHeight * 2 * Model.Gravity);
            }
        }
        else
        {
            Animator.Jump();
        }

        if (Model.FacingRight)
        {
            Model.Acceleration.x = Model.WalkAcceleration + Model.KineticFriction;
        }
        else
        {
            Model.Acceleration.x = -(Model.WalkAcceleration + Model.KineticFriction);
        }
        Animator.SetFacing(Model.FacingRight);


    }

    public override void TakeDamage(int damage)
    {
        if (this.Health.ReduceHealth(damage) <= 0)
        {
            this.Die();
        }
        else
        {
            if (damageFreezeTimer <= 0)
            {
                savedVelocity = Model.Velocity;
                savedAcceleration = Model.Acceleration;
                savedGravityMultiplier = Model.GravityMultiplier;
                damageFreezeTimer = 1.0f;
                this.StartCoroutine("DamageFreeze");
            }
            damageFreezeTimer = 0.3f;
        }
    }

    private float damageFreezeTimer;
    private Vector2 savedVelocity;
    private Vector2 savedAcceleration;
    private float savedGravityMultiplier;

    private IEnumerator DamageFreeze()
    {
        Animator.StartTakingDamage();
        while (damageFreezeTimer > 0)
        {
            Model.Velocity = Vector2.zero;
            Model.Acceleration = Vector2.zero;
            Model.GravityMultiplier = 0;
            yield return null;
            damageFreezeTimer -= Time.deltaTime;
        }

        Model.Velocity = savedVelocity;
        Model.Acceleration = savedAcceleration;
        Model.GravityMultiplier = savedGravityMultiplier;
        Animator.StopTakingDamage();
    }
}
