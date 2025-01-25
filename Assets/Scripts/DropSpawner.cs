using UnityEngine;

public class DropSpawner : BaseSpawner
{
    public GameObject dropPrefab;
    public float dropSpeed = 3f;

    protected override void SpawnObject()
    {
        Vector3 spawnPosition = GetRandomPositionOnAllowedSides();

        GameObject drop = Instantiate(dropPrefab, spawnPosition, Quaternion.identity);

        MoveObject moveScript = drop.AddComponent<MoveObject>();
        moveScript.SetDirectionAndSpeed(Vector2.down, dropSpeed);
    }
}