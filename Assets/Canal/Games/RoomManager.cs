using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Canal.Unity;

public class RoomManager : Behavior
{
    public RoomList Rooms;
    public RoomModel StartingRoom;

    public GameObject CharacterPrefab;
    public CameraController CameraController;

    private PositionModel character;

    private Dictionary<string, RoomModel> loadedRooms;
    private Transform roomContainer;
    private RoomModel currentRoom;

    public void Awake()
    {
        loadedRooms = new Dictionary<string, RoomModel>();

        roomContainer = new GameObject().transform;
        roomContainer.name = "Room Container";
        roomContainer.parent = this.transform;
        roomContainer.localPosition = Vector3.zero;
        roomContainer.localScale = Vector3.one;
        roomContainer.localEulerAngles = Vector3.zero;

        if (StartingRoom == null)
        {
            SetCurrentRoom(LoadRoom(Rooms.DefaultRoom));
        }
        else
        {
            SetCurrentRoom(StartingRoom);
            Transform roomTransform = StartingRoom.transform;
            roomTransform.parent = roomContainer;
            roomTransform.localPosition = Vector3.zero;
            roomTransform.localScale = Vector3.one;
            roomTransform.localEulerAngles = Vector3.zero;
        }

        GameObject characterInstance = GameObject.Instantiate(CharacterPrefab) as GameObject;
        character = characterInstance.GetComponent<PositionModel>();
        character.Position = currentRoom.DefaultSpawnPosition;

        CameraController.Target = characterInstance.transform;
        CameraController.CurrentPositioner = currentRoom.DefaultCameraPositioner;
        characterInstance.transform.parent = currentRoom.transform;

    }

    private RoomModel SetCurrentRoom(string roomId)
    {
        RoomModel newRoom = LoadRoom(Rooms[roomId]);
        return SetCurrentRoom(newRoom);
    }

    private RoomModel SetCurrentRoom(RoomModel newRoom)
    {
        if (newRoom == null || newRoom == currentRoom)
        {
            return currentRoom;
        }

        newRoom.gameObject.SetActive(true);
        if (currentRoom != null)
        {
            currentRoom.gameObject.SetActive(false);
        }
        for (int i = 0, count = newRoom.Exits.Length; i < count; ++i)
        {
            LoadRoom(Rooms[newRoom.Exits[i].TargetRoomId]);
        }

        currentRoom = newRoom;

        return newRoom;
    }

    private RoomModel LoadRoom(RoomList.RoomInfo info)
    {
        if (this.loadedRooms.ContainsKey(info.RoomId))
        {
            return this.loadedRooms[info.RoomId];
        }

        GameObject roomPrefab = Resources.Load(info.PrefabPath) as GameObject;
        if (roomPrefab != null)
        {
            GameObject roomInstance = GameObject.Instantiate(roomPrefab) as GameObject;

            RoomModel room = roomInstance.GetComponent<RoomModel>();
            this.loadedRooms[info.RoomId] = room;

            Transform roomTransform = room.transform;
            roomTransform.parent = roomContainer;
            roomTransform.localPosition = Vector3.zero;
            roomTransform.localScale = Vector3.one;
            roomTransform.localEulerAngles = Vector3.zero;
            room.gameObject.SetActive(false);
            return room;
        }
        return null;
    }

    private void UnloadRooms(params string[] roomIds)
    {
        foreach (string id in roomIds)
        {
            if (loadedRooms.ContainsKey(id))
            {
                RoomModel model = loadedRooms[id];
                loadedRooms.Remove(id);
                GameObject.Destroy(model.gameObject);
            }
        }

        this.StartCoroutine(UnloadAssets());
    }

    private IEnumerator UnloadAssets()
    {
        yield return Resources.UnloadUnusedAssets();
    }

    private int currentRoomIndex = 0;
    public void Start()
    {
        SwitchRoom();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            currentRoomIndex++;
            if (currentRoomIndex >= Rooms.Length)
            {
                currentRoomIndex = 0;
            }
            SwitchRoom();
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            currentRoomIndex--;
            if (currentRoomIndex < 0)
            {
                currentRoomIndex = Rooms.Length - 1;
            }
            SwitchRoom();
        }

        foreach (RoomExit exit in currentRoom.Exits)
        {
            Vector2 offset;
            if (exit.CheckExit(character, out offset))
            {
                this.SetCurrentRoom(exit.TargetRoomId);
                character.Position = currentRoom.Entrances[exit.TargetRoomEntranceIndex].GetEntryPosition(offset);
                character.Transform.Translate(Vector2.zero);

                CameraController.Target = character.transform;
                CameraController.CurrentPositioner = currentRoom.DefaultCameraPositioner;
                character.transform.parent = currentRoom.transform;
                Debug.Log(exit.TargetRoomEntranceIndex);
                CameraController.LateUpdate();
                break;
            }
        }
    }

    private void SwitchRoom()
    {
        SetCurrentRoom(Rooms[currentRoomIndex].RoomId);
        character.Position = currentRoom.DefaultSpawnPosition;

        CameraController.Target = character.transform;
        CameraController.CurrentPositioner = currentRoom.DefaultCameraPositioner;
        character.transform.parent = currentRoom.transform;
    }
}
