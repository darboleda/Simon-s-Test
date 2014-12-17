using UnityEngine;
using System.Collections;

using Canal.Unity;

public class TestHitScript : Behavior {
    public MeshRenderer renderer;
    public void TakeHit()
    {
        renderer.material.color = Color.red;
    }

    public void Update()
    {
        renderer.material.color = Color.Lerp(renderer.material.color, Color.white, 0.1f);
    }
}
