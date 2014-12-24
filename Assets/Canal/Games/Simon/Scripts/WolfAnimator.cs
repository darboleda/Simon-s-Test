using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;

public class WolfAnimator : Behavior {

    public tk2dSpriteAnimator Animator;

    public void SetFacing(bool facingRight)
    {
        Animator.Sprite.FlipX = facingRight;
    }

    public void Jump()
    {
        Animator.Play("Jump");
    }

    public void Walk()
    {
        if (!Animator.IsPlaying("Walk"))
        {
            Animator.Play("Walk");
        }
    }

    public void StartTakingDamage()
    {
        DamageFlash = true;
        Animator.Stop();
        this.StopCoroutine("StartDamageFlash");
        this.StartCoroutine("StartDamageFlash");
    }

    public void StopTakingDamage()
    {
        DamageFlash = false;
        Animator.Play();
        Animator.Sprite.renderer.enabled = true;
    }

    private bool DamageFlash;
    public IEnumerator StartDamageFlash()
    {
        while (DamageFlash)
        {
            Animator.Sprite.renderer.enabled = !Animator.Sprite.renderer.enabled;
            yield return null;
        }
    }
}
