using UnityEngine;

using Canal.Unity;

public class RoomList : ScriptableObject
{
    [System.Serializable]
    public struct RoomInfo
    {
        public string RoomId;
        public string PrefabPath;
    }

    [SerializeField]
    private string defaultRoom;

    public RoomInfo DefaultRoom
    {
        get { return this[defaultRoom]; }
    }

    [SerializeField]
    private RoomInfo[] rooms;

    public RoomInfo this[int index]
    {
        get { return rooms[index]; }
    }

    public RoomInfo this[string id]
    {
        get
        {
            for (int i = 0, count = rooms.Length; i < count; ++i)
            {
                if (rooms[i].RoomId == id)
                {
                    return rooms[i];
                }
            }
            return new RoomInfo();
        }
    }

    public int Length { get { return rooms.Length; } }
}
