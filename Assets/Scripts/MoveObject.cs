using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private Vector2 direction; // Напрямок руху
    private float speed; // Швидкість руху

    public void SetDirectionAndSpeed(Vector2 newDirection, float newSpeed)
    {
        direction = newDirection;
        speed = newSpeed;
    }

    private void Update()
    {
        // Рухаємо об'єкт у вказаному напрямку
        transform.Translate(direction * speed * Time.deltaTime);

        // Знищуємо об'єкт, якщо він вийшов за межі екрану
        if (Mathf.Abs(transform.position.x) > 10f || Mathf.Abs(transform.position.y) > 6f)
        {
            Destroy(gameObject);
        }
    }
}