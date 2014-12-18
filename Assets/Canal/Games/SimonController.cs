using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;

public class SimonController : Behavior, IDamageable<int> {

    public SimonModel Model;
    public SimonAnimator Animator;

    private bool startedJump = false;
    private bool previousTakingDamage = false;
    private float hurtTimer;

    public void Update()
    {
        Model.Collision.DisableGroundCling = false;
        if (Model.Attacking)
        {
            Model.Acceleration.x = 0;
            return;
        }

        if (Model.TakingDamage)
        {
            if (Model.Collision.OnGround && Model.Velocity.y <= 0)
            {
                Model.TakingDamage = false;
                hurtTimer = 1.0f;
            }
            Animator.Hurt();
            Model.Acceleration.x = 0;
            return;
        }
        else if (hurtTimer > 0)
        {
            Animator.StartHurtFlash();
            hurtTimer -= Time.deltaTime;
        }
        else
        {
            Animator.StopHurtFlash();
        }

        if (Model.Collision.OnGround)
        {
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                Model.Movement = SimonModel.MovementState.Crouching;
            }
            else
            {
                Model.Movement = SimonModel.MovementState.Standing;
            }
        }
        else
        {
            Model.Movement = SimonModel.MovementState.InAir;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Model.Attacking = true;
            if (Input.GetAxisRaw("Vertical") == 1)
            {
                Animator.SubWeapon(OnAttackAnimationCompleted);

                GameObject holyWater = GameObject.Instantiate(Model.HolyWaterPrefab) as GameObject;
                holyWater.transform.parent = this.transform.parent;
                SubweaponController sub = holyWater.GetComponent<SubweaponController>();
                Vector2 velocityOffset = Vector2.zero;
                if (Model.Movement == SimonModel.MovementState.InAir)
                {
                    float velX = Model.Velocity.x;
                    if (velX > 0)
                    {
                        velocityOffset.x = 2;
                    }
                    else if (velX < 0)
                    {
                        velocityOffset.x = -2;
                    }
                }
                sub.Fire(
                    Model.Position + new Vector2(Model.FacingRight ? 0.25f : -0.25f, 1.2f), 
                    velocityOffset,
                    Model.FacingRight
                );
            }
            else
            {
                Animator.Attack(OnAttackAnimationCompleted);
            }
            return;
        }

        float movement = Input.GetAxisRaw("Horizontal");
        switch (Model.Movement)
        {
            case SimonModel.MovementState.Standing:
                Model.Acceleration.x = movement * (Model.WalkAcceleration + Model.KineticFriction);
                if (Model.Acceleration.x > 0)
                {
                    Model.FacingRight = true;
                }
                else if (Model.Acceleration.x < 0)
                {
                    Model.FacingRight = false;
                }
                Animator.SetFacing(Model.FacingRight);
                break;

            case SimonModel.MovementState.InAir:
                movement = Model.TakingDamage ? 0 : movement;
                Model.Acceleration.x = movement * (Model.WalkAcceleration + Model.KineticFriction);
                break;

            case SimonModel.MovementState.Crouching:
                Model.Acceleration.x = 0;
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            switch (Model.Movement)
            {
                case SimonModel.MovementState.Standing:
                    Model.Velocity.y = Mathf.Sqrt(Model.JumpHeight * 2 * Model.Gravity);
                    break;

                case SimonModel.MovementState.Crouching:
                    if (Model.Collision.BottomCollision == WalkerModel.CollisionState.FloorCollision.Platform)
                    {
                        Model.Velocity.x = 0;
                        Model.Velocity.y = -Mathf.Sqrt(Model.Gravity) * 0.5f;
                        Model.Transform.Translate(Vector2.up * Model.Velocity.y * 0.1f);
                    }
                    break;

                case SimonModel.MovementState.InAir:
                    Model.Velocity.y = Mathf.Sqrt(Model.DoubleJumpHeight * 2 * Model.Gravity);
                    startedJump = true;
                    break;
            }

        }

        if ((!Input.GetButton("Jump") && startedJump) && Model.Velocity.y > 0)
        {
            Model.Velocity.y *= 0.3f;
            startedJump = false;
        }

        switch (Model.Movement)
        {
            case SimonModel.MovementState.InAir:
                if (Model.Velocity.y > 0)
                {
                    Animator.Jump();
                }
                else
                {
                    Animator.Fall();
                }
                break;

            case SimonModel.MovementState.Crouching:
                Animator.Crouch();
                break;

            case SimonModel.MovementState.Standing:
                if (Mathf.Abs(Model.Acceleration.x) > 0)
                {
                    Animator.Walk();
                }
                else
                {
                    Animator.Stand();
                }
                break;
        }
    }

    private void OnAttackAnimationCompleted()
    {
        Model.Attacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (!Model.TakingDamage && hurtTimer <= 0)
        {
            Model.TakingDamage = true;
            Model.Acceleration.x = 0;
            Model.Velocity.x = (Model.FacingRight ? -3 : 3);
            Model.Velocity.y = Mathf.Sqrt(1 * 2 * Model.Gravity);
        }
    }
}
