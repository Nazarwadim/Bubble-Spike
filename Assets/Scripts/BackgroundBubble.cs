using UnityEngine;

public class BackgroundBubble : MonoBehaviour
{
    public float Speed { get; set; }
    public float WaveAmplitude = 0.5f;
    public float WaveFrequency = 1f;

    private float _initialX;
    private float _time;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _initialX = transform.position.x;
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        _time += Time.deltaTime;
        float waveOffset = Mathf.Sin(_time * WaveFrequency) * WaveAmplitude;
        transform.position = new Vector3(_initialX + waveOffset, transform.position.y, transform.position.z);

        UpdateTransparency();

        if (IsOutOfBounds())
        {
            Destroy(gameObject);
        }
    }

    private void UpdateTransparency()
    {
        if (_renderer != null)
        {
            float scale = transform.localScale.x;
            float alpha = Mathf.Clamp(scale, 0.5f, 1f);
            Color color = _renderer.color;
            color.a = alpha;
            _renderer.color = color;
        }
    }

    private bool IsOutOfBounds()
    {
        if (Camera.main == null) return true;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.y > 1.5f;
    }
}