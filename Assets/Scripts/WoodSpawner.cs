using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WoodSpawner : BaseSpawner
{
    public GameObject WoodPrefab;
    public GameObject ArrowPrefab;
    public float MinSize = 1f;
    public float MaxSize = 1.5f;
    public float MinSpeed = 1f;
    public float MaxSpeed = 5f;

    public float MinArrowTime = 0.5f;
    public float MaxArrowTime = 1.2f;

    public Transform ToObject;

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        StartCoroutine(Spawn(spawnPosition));
    }

    private IEnumerator Spawn(Vector3 spawnPosition)
    {
        GameObject arrow = Instantiate(ArrowPrefab);
        arrow.transform.SetParent(transform);
        arrow.transform.position = new Vector3(spawnPosition.x, -4.0f, 0);
        Vector2 direction = ((Vector2)ToObject.position + 5 * Vector2.up - (Vector2)spawnPosition).normalized;
        yield return new WaitForSeconds(Random.Range(MinArrowTime, MaxArrowTime));
        Destroy(arrow);

        GameObject obj = Instantiate(WoodPrefab, spawnPosition, Quaternion.identity);
        Wood wood = obj.GetComponent<Wood>();

        float randomSize = Random.Range(MinSize, MaxSize);

        float speed = Random.Range(MinSpeed, MaxSpeed);

        wood.direction = direction;
        wood.transform.localScale *= randomSize;
        wood.Speed = speed;
    }
}