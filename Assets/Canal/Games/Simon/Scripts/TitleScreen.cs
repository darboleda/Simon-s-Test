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
        Levels.GetAdditiveLevelLoader("Title").Load();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Rooms.LoadRoom(StartingRoom);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Levels.GetAdditiveLevelLoader("Title").Load();
        }
	}
}
