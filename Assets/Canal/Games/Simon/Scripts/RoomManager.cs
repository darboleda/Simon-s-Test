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

    private PositionModel character;
    private RoomLevel currentRoom;

    private Texture2D fadeTexture;

    public void Awake()
    {
        fadeTexture = fadeTexture ?? new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, Color.clear);
        fadeTexture.Apply();
    }

    public void SetCurrentRoom(string roomId)
    {
        SetCurrentRoomAtEntrance(roomId, 0, Vector2.zero);
    }

    public void SetCurrentRoomAtEntrance(string roomId, int entranceId, Vector2 entranceOffset)
    {
        if (!character)
        {
            character = GameObject.Instantiate(CharacterPrefab) as PositionModel;
            character.gameObject.SetActive(false);
            character.transform.SetParent(this.transform, false);
        }

        this.StartCoroutine(FadeIn(roomId, entranceId, entranceOffset));
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
                this.SetCurrentRoomAtEntrance(exit.TargetRoomId, exit.TargetRoomEntranceIndex, offset);
                break;
            }
        }
    }

    private bool transitioning = false;

    private IEnumerator FadeIn(string roomId, int entranceId, Vector2 entranceOffset)
    {
        transitioning = true;

        float timeLeft = FadeTime;
        Time.timeScale = 0f;
        while (timeLeft > 0)
        {
            fadeTexture.SetPixel(0, 0, Color.Lerp(Color.black, fadeTexture.GetPixel(0, 0), timeLeft / FadeTime));
            fadeTexture.Apply();
            timeLeft -= Time.unscaledDeltaTime;
            yield return null;
        }

        fadeTexture.SetPixel(0, 0, Color.black);
        fadeTexture.Apply();

        character.gameObject.SetActive(false);
        character.transform.SetParent(this.transform, false);

        LevelManager.LoadLevelAdditive<RoomLevel>(roomId, delegate(RoomLevel roomLevel) {
            currentRoom = roomLevel;
            currentRoom.SetPlayerCharacter(character, entranceId, entranceOffset);
            character.gameObject.SetActive(true);

            this.StartCoroutine(FadeOut());
        });
    }

    private IEnumerator FadeOut()
    {
        yield return null;
        yield return LevelManager.UnloadUnusedAssets();
        float timeLeft = FadeTime;
        Time.timeScale = 1;

        while (timeLeft > 0)
        {
            fadeTexture.SetPixel(0, 0, Color.Lerp(Color.clear, Color.black, timeLeft / FadeTime));
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
