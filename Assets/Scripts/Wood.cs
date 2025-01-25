using UnityEngine;

public class Wood : MonoBehaviour
{
    public float minSize = 1f; 
    public float maxSize = 1.5f;   
    public float minSpeed = 1f; 
    public float maxSpeed = 5f;  
    public Vector2 direction = Vector2.down; 
    public float rotationSpeed = 50f; 

    private float speed; 
    private Vector3 initialScale; 
    private Transform childSprite; 

    private void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        initialScale = transform.localScale; 
        transform.localScale = initialScale * randomSize; 

        speed = Random.Range(minSpeed, maxSpeed);

        if (transform.childCount > 0)
        {
            childSprite = transform.GetChild(0); 
        }
        else
        {
            Debug.LogWarning("Child sprite not found. Add sprite as child object.");
        }
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (childSprite != null)
        {
            childSprite.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        if (IsOutOfBounds())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfBounds()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.y < -0.1f; 
    }
}