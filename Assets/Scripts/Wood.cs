using UnityEngine;

public class Wood : MonoBehaviour
{
    public float minSize = 1f; 
    public float maxSize = 1.5f;   
    public float minSpeed = 1f; 
    public float maxSpeed = 5f;  
    public Vector2 direction = Vector2.down; 

    private float speed; 
    private Vector3 initialScale; 

    private void Start()
    {
       
        float randomSize = Random.Range(minSize, maxSize);
        initialScale = transform.localScale; 
        transform.localScale = initialScale * randomSize; 

        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
       
        transform.Translate(direction * speed * Time.deltaTime);

       
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