using UnityEngine;

using Canal.Unity;
using Canal.Unity.Platformer;

public class SimonAnimator : Behavior
{
    [System.Serializable]
    public struct AnimationMap
    {
        public string AnimationName;
        public AnimatedBoxCollider ColliderAnimation;
    }

    public delegate void AnimationCompletedHandler();

    public tk2dSpriteAnimator Animator;
    public SimonModel Model;
    public AnimationMap[] ColliderMaps;

    private new Renderer renderer;
    public Renderer Renderer
    {
        get
        {
            return (renderer = renderer ?? Animator.Sprite.GetComponent<Renderer>());
        }
    }

    public void SetFacing(bool facingRight)
    {
        Animator.Sprite.FlipX = !facingRight;
    }

    public void Crouch()
    {
        switch (Model.Movement)
        {
            case SimonModel.MovementState.Crouching:
                if (!Animator.IsPlaying("Crouch"))
                {
                    Animator.Play("Crouch");
                }
                break;

            default:
                break;
        }
    }

    public void Stand()
    {
        switch (Model.Movement)
        {
            case SimonModel.MovementState.Standing:
                if (!Animator.IsPlaying("Stand"))
                {
                    Animator.Play("Stand");
                }
                break;

            default:
                break;
        }
    }

    public void Walk()
    {
        switch (Model.Movement)
        {
            case SimonModel.MovementState.Standing:
                if (!Animator.IsPlaying("Walk"))
                {
                    Animator.Play("Walk");
                }
                break;

            default:
                break;
        }
    }

    public void Attack(AnimationCompletedHandler handler)
    {
        if (handler != null)
        {
            Animator.AnimationCompleted = delegate(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
            {
                handler();
            };
        }
        switch (Model.Movement)
        {
            case SimonModel.MovementState.Crouching:
                Animator.Play("Crouch Whip");
                break;

            default:
                Animator.Play("Stand Whip");
                break;
        }
    }

    public void SubWeapon(AnimationCompletedHandler handler)
    {
        if (handler != null)
        {
            Animator.AnimationCompleted = delegate(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
                {
                    handler();
                };
        }
        Animator.Play("Throw Sub");
    }

    public void Fall()
    {
        this.Jump();
    }

    public void Jump()
    {
        if (!Animator.IsPlaying("Jump"))
        {
            Animator.Play("Jump");
        }
    }

    public void Hurt()
    {
        Animator.Play("Hurt");
    }

    public void StartHurtFlash()
    {
        DamageFlash = true;
        this.StopCoroutine("StartDamageFlash");
        this.StartCoroutine("StartDamageFlash");
    }

    public void StopHurtFlash()
    {
        DamageFlash = false;
        Renderer.enabled = true;
    }

    private bool DamageFlash;
    public System.Collections.IEnumerator StartDamageFlash()
    {
        while (DamageFlash)
        {
            Renderer.enabled = !Renderer.enabled;
            yield return null;
        }
    }

    public void Update()
    {
        if (Animator.CurrentClip == null)
        {
            return;
        }
        string currentClip = Animator.CurrentClip.name;
        foreach (AnimationMap map in ColliderMaps)
        {
            if (map.AnimationName == currentClip)
            {
                map.ColliderAnimation.gameObject.SetActive(true);
                map.ColliderAnimation.SetFrame(Animator.CurrentFrame);
                Vector3 localScale = map.ColliderAnimation.transform.localScale;
                localScale.x = (Animator.Sprite.FlipX ? -1 : 1);
                map.ColliderAnimation.transform.localScale = localScale;
            }
            else
            {
                map.ColliderAnimation.gameObject.SetActive(false);
            }
        }
    }
}
