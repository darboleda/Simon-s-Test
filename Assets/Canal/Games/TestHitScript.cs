using UnityEngine;
using System.Collections;

using Canal.Unity;

public class TestHitScript : Behavior {
    public MeshRenderer Renderer;
    public void TakeHit()
    {
        Renderer.material.color = Color.red;
    }

    public void Update()
    {
        Renderer.material.color = Color.Lerp(Renderer.material.color, Color.white, 0.1f);
    }
}
