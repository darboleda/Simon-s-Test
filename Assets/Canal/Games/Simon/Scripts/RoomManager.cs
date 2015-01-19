using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Canal.Unity;
using Canal.Unity.Framework;

public class RoomManager : Behavior
{
    public LevelManager LevelManager;
    public PositionModel CharacterPrefab;
    public float FadeTime = 0.3f;
    public Color FadeColor = Color.black;

    private PositionModel character;
    private RoomLevel currentRoom;

    private Texture2D fadeTexture;

    public void Awake()
    {
        fadeTexture = fadeTexture ?? new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.clear);
        fadeTexture.Apply();
    }

    public Coroutine LoadRoom(string roomId)
    {
        return LoadRoomAtEntrance(roomId, 0, Vector2.zero);
    }

    public Coroutine LoadRoomAtEntrance(string roomId, int entranceId, Vector2 entranceOffset)
    {
        return this.StartCoroutine(TransitionRoom(roomId, entranceId, entranceOffset));
    }

    public void Update()
    {
        if (currentRoom == null)
            return;

        foreach (RoomExit exit in currentRoom.Room.Exits)
        {
            Vector2 offset;
            if (exit.CheckExit(character, out offset) && !transitioning)
            {
                this.LoadRoomAtEntrance(exit.TargetRoomId, exit.TargetRoomEntranceIndex, offset);
                break;
            }
        }
    }

    private bool transitioning = false;

    private IEnumerator TransitionRoom(string roomId, int entranceId, Vector2 entranceOffset)
    {
        if (!character)
        {
            character = GameObject.Instantiate(CharacterPrefab) as PositionModel;
            character.gameObject.SetActive(false);
            character.transform.SetParent(this.transform, false);
        }

        if (transitioning) yield break;

        transitioning = true;

        float timeLeft = FadeTime;
        Time.timeScale = 0f;
        while (timeLeft > 0)
        {
            fadeTexture.SetPixel(0, 0, Color.Lerp(FadeColor, fadeTexture.GetPixel(0, 0), timeLeft / FadeTime));
            fadeTexture.Apply();
            timeLeft -= Time.unscaledDeltaTime;
            yield return null;
        }

        fadeTexture.SetPixel(0, 0, FadeColor);
        fadeTexture.Apply();

        character.gameObject.SetActive(false);
        character.transform.SetParent(this.transform, false);

        LevelManager.LevelLoader loader = LevelManager.GetAdditiveLevelLoader(roomId);
        yield return loader.Load();

        currentRoom = loader.GetLoadedLevel<RoomLevel>();
        currentRoom.SetPlayerCharacter(character, entranceId, entranceOffset);
        character.gameObject.SetActive(true);

        timeLeft = FadeTime;
        Time.timeScale = 1;

        while (timeLeft > 0)
        {
            fadeTexture.SetPixel(0, 0, Color.Lerp(Color.clear, FadeColor, timeLeft / FadeTime));
            fadeTexture.Apply();
            timeLeft -= Time.unscaledDeltaTime;
            yield return null;
        }
        fadeTexture.SetPixel(0, 0, Color.clear);
        fadeTexture.Apply();
        transitioning = false;
    }

    public void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture, ScaleMode.StretchToFill, true, 1);
    }
}
