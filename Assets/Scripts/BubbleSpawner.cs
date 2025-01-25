using UnityEngine;

public class BubbleSpawner : BaseSpawner
{
    public GameObject bubblePrefab; // Префаб бульбашки
    public float bubbleForce = 2f; // Сила, яка додається до бульбашки

    protected override void SpawnObject()
    {
        // Отримуємо випадкову позицію на дозволених сторонах екрану
        Vector3 spawnPosition = GetRandomPositionOnAllowedSides();

        // Створюємо об'єкт
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Додаємо Rigidbody2D для фізики
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();

        // Додаємо випадкову силу для руху
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        rb.AddForce(randomDirection * bubbleForce, ForceMode2D.Impulse);
    }
}