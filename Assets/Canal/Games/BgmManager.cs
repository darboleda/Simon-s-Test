using UnityEngine;
using System.Collections;

using Canal.Unity;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : Behavior
{
    public void SetBgm(AudioClip audioClip)
    {
        AudioSource source = this.GetComponent<AudioSource>();
        if (source.clip != audioClip)
        {
            source.Stop();
            source.clip = audioClip;
            source.Play();
        }
    }
}
