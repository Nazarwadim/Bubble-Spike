using UnityEngine;

public class BubbleSpawner : BaseSpawner
{
    public GameObject bubblePrefab;
    public float bubbleForce = 2f;

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        throw new System.NotImplementedException();
        // Vector3 spawnPosition = GetRandomPositionOnAllowedSides();

        // GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();

        // Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        // rb.AddForce(randomDirection * bubbleForce, ForceMode2D.Impulse);
    }
}