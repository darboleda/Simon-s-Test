using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.States;

public class TitleScreen : Behavior
{	
    public GameStateBehavior StateBehavior;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StateBehavior.RequestState("Quest");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateBehavior.RequestState("Title");
        }
	}
}
