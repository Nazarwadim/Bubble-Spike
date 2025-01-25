using System;
using UnityEngine;

public class DropSpawner : BaseSpawner
{
    public GameObject dropPrefab;
    public float dropSpeed = 3f;

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        throw new NotImplementedException();

        // Vector3 spawnPosition = GetRandomPositionOnAllowedSides();

        // GameObject drop = Instantiate(dropPrefab, spawnPosition, Quaternion.identity);
        
    }
}