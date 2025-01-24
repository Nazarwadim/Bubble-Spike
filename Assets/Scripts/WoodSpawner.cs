using UnityEngine;

public class WoodSpawner : BaseSpawner
{
    public GameObject woodPrefab; 
    public float woodSpeed = 5f;

    protected override void SpawnObject()
    {
        Transform spawnPlane = GetRandomSpawnPlane();
        if (spawnPlane == null) return;

        Vector3 spawnPosition = GetRandomPositionOnPlane(spawnPlane);
        GameObject wood = Instantiate(woodPrefab, spawnPosition, Quaternion.identity);

        Vector2 direction = spawnPlane.position.x > 0 ? Vector2.left : Vector2.right;

        MoveObject moveScript = wood.AddComponent<MoveObject>();
        moveScript.SetDirectionAndSpeed(direction, woodSpeed);
    }
}