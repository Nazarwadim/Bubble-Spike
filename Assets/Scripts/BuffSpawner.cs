using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BuffSpawner : BaseSpawner
{
    [System.Serializable]
    public struct BuffSpawnInfo
    {
        public string BuffName;
        public GameObject BuffPrefab;
        public int SpawnProbability;
    }

    public BuffSpawnInfo[] Buffs;

    public float minFallSpeed = 1f;
    public float maxFallSpeed = 5f;

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        int randomIndex = UnityEngine.Random.Range(0, Buffs.Length);
        int randomValue = UnityEngine.Random.Range(0, 100);
        int probabilitySum = 0;
        GameObject spawnPrefab = Buffs[0].BuffPrefab;
        foreach (BuffSpawnInfo buffSpawnInfo in Buffs)
        {
            probabilitySum += buffSpawnInfo.SpawnProbability;
            if (randomValue < probabilitySum)
            {
                spawnPrefab = buffSpawnInfo.BuffPrefab;
                break;
            }
        }

        GameObject buffObject = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);

        Buff buff = buffObject.GetComponent<Buff>();

        float randomSpeed = UnityEngine.Random.Range(minFallSpeed, maxFallSpeed);
        buff.SetFallSpeed(randomSpeed);

    }
}
