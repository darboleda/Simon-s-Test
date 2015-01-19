using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Framework;

public class TitleScreen : Behavior
{	
    public LevelManager Levels;
    public RoomManager Rooms;

    public string StartingRoom = "Jova";

    public void Awake()
    {
        Levels.LoadLevelAdditive<Level>("Title");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Rooms.SetCurrentRoom(StartingRoom);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Levels.LoadLevelAdditive<Level>("Title");
        }
	}
}
