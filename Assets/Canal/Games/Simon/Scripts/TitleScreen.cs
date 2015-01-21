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
        if (Input.GetButtonDown("Start"))
        {
            Rooms.LoadRoom(StartingRoom);
        }
        else if (Input.GetButtonDown("Exit"))
        {
            if (Levels.LastLoadedLevelKey == "Title")
            {
                Application.Quit();
            }
            else
            {
                Levels.GetAdditiveLevelLoader("Title").Load();
            }
        }
	}
}
