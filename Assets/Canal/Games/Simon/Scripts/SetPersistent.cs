using UnityEngine;
using System.Collections;

using Canal.Unity;

public class SetPersistent : Behavior
{
    public void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
