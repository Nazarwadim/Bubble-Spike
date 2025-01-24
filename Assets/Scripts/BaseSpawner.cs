using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    public float spawnInterval = 2f; // Інтервал між спавном
    public Transform[] spawnPlanes; // Масив площин для спавну

    protected virtual void Start()
    {
        // Запускаємо спавн об'єктів з інтервалом
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }

    protected abstract void SpawnObject(); // Абстрактний метод для спавну об'єктів

    protected Vector3 GetRandomPositionOnPlane(Transform plane)
    {
        // Отримуємо випадкову позицію в межах площини
        Vector3 localPosition = new Vector3(
            Random.Range(-0.5f, 0.5f), // Випадкове значення по X
            Random.Range(-0.5f, 0.5f), // Випадкове значення по Y
            0
        );

        // Перетворюємо локальну позицію у світову
        return plane.TransformPoint(localPosition);
    }

    protected Transform GetRandomSpawnPlane()
    {
        // Вибираємо випадкову площину з масиву
        if (spawnPlanes.Length == 0) return null;
        return spawnPlanes[Random.Range(0, spawnPlanes.Length)];
    }
}