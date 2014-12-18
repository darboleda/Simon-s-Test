using UnityEngine;
using System.Collections;

using Canal.Unity;

public class EnemySpawner : Behavior {
    public RoomModel Owner;
    public Vector2 Position;
    public GameObject EnemyPrefab;

    public void Awake()
    {
        if (this.renderer == null)
        {
            this.gameObject.AddComponent<MeshRenderer>();
        }
    }

    public void OnBecameVisible()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        GameObject enemy = GameObject.Instantiate(EnemyPrefab) as GameObject;
        if (enemy == null)
            return;

        enemy.transform.parent = Owner.transform;
        PositionModel model = enemy.GetComponent<PositionModel>();
        model.Position = Position;
    }
}
