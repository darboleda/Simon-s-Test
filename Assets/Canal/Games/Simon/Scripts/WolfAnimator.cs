using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;

public class WolfAnimator : Behavior {

    public tk2dSpriteAnimator Animator;

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
        Renderer.enabled = true;
    }

    private bool DamageFlash;
    public IEnumerator StartDamageFlash()
    {
        while (DamageFlash)
        {
            Renderer.enabled = !Renderer.enabled;
            yield return null;
        }
    }
}
