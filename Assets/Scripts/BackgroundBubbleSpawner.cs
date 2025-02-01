using UnityEngine;

public class BackgroundBubbleSpawner : BaseSpawner
{
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _maxScale = 1.5f;
    [SerializeField] private bool _useRandomColor = true;
    [SerializeField] private float _minWaveAmplitude = 0.3f;
    [SerializeField] private float _maxWaveAmplitude = 0.7f;
    [SerializeField] private float _minWaveFrequency = 0.5f;
    [SerializeField] private float _maxWaveFrequency = 1.5f;

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        if (_bubblePrefab == null)
        {
            Debug.LogError("Bubble prefab is not assigned!");
            return;
        }

        // Створюємо бульбашку
        GameObject bubble = Instantiate(_bubblePrefab, new Vector3(spawnPosition.x, spawnPosition.y, 6f), Quaternion.identity);

        BackgroundBubble backgroundBubble = bubble.GetComponent<BackgroundBubble>();
        if (backgroundBubble != null)
        {
            SetRandomSpeed(backgroundBubble);
            SetRandomScale(bubble);
            SetRandomWaveParameters(backgroundBubble);

            if (_useRandomColor)
            {
                SetRandomColor(bubble);
            }
        }
    }

    private void SetRandomSpeed(BackgroundBubble bubble)
    {
        float scale = bubble.transform.localScale.x;
        float speedMultiplier = 1f / scale;
        float randomSpeed = Random.Range(_minSpeed, _maxSpeed) * speedMultiplier;
        bubble.Speed = randomSpeed;
    }

    private void SetRandomColor(GameObject bubble)
    {
        SpriteRenderer renderer = bubble.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f);
            renderer.color = randomColor;
        }
    }

    private void SetRandomScale(GameObject bubble)
    {
        float randomScale = Random.Range(_minScale, _maxScale);
        bubble.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }

    private void SetRandomWaveParameters(BackgroundBubble bubble)
    {
        bubble.WaveAmplitude = Random.Range(_minWaveAmplitude, _maxWaveAmplitude);
        bubble.WaveFrequency = Random.Range(_minWaveFrequency, _maxWaveFrequency);
    }
}