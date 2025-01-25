using UnityEngine;

public class BuffSpawner : BaseSpawner
{
    public GameObject[] buffPrefabs;
    public float minFallSpeed = 1f;  
    public float maxFallSpeed = 5f;   

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        int randomIndex = UnityEngine.Random.Range(0, buffPrefabs.Length);
        GameObject buffObject = Instantiate(buffPrefabs[randomIndex], spawnPosition, Quaternion.identity);

        Buff buff = buffObject.GetComponent<Buff>();
        if (buff != null)
        {
            float randomSpeed = UnityEngine.Random.Range(minFallSpeed, maxFallSpeed);
            buff.SetFallSpeed(randomSpeed);
        }
    }
}
