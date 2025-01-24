using UnityEngine;

public class DropSpawner : BaseSpawner
{
    public GameObject dropPrefab; // Префаб краплі
    public float dropSpeed = 3f; 

    protected override void SpawnObject()
    {
        Transform spawnPlane = GetRandomSpawnPlane();
        if (spawnPlane == null) return;

        Vector3 spawnPosition = GetRandomPositionOnPlane(spawnPlane);
        GameObject drop = Instantiate(dropPrefab, spawnPosition, Quaternion.identity);
        MoveObject moveScript = drop.AddComponent<MoveObject>();
        moveScript.SetDirectionAndSpeed(Vector2.down, dropSpeed);
    }
}