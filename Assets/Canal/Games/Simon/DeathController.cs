using UnityEngine;
using System.Collections;

using Canal.Unity;

public class DeathController : Behavior {
    public tk2dSpriteAnimator Animator;

    public void BeginAnimation(System.Action callback)
    {
        Animator.AnimationCompleted = delegate(tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
            {
                GameObject.Destroy(this.gameObject);
                if (callback != null)
                {
                    callback();
                }
            };
        Animator.Play();
    }
}
