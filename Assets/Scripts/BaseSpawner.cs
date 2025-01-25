using UnityEngine;
using System.Collections.Generic;

public abstract class BaseSpawner : MonoBehaviour
{
    private enum SideEnum { Left, Right, Top, Bottom }

    [System.Serializable]
    private struct ScreenSide
    {
        public float Offset;
        public SideEnum Side;
    }

    private void OnEnable()
    {
        ActionBus.MainBubbleKilled += OnPlayerDied;
    }

    private void OnDisable()
    {
        ActionBus.MainBubbleKilled -= OnPlayerDied;
    }

    public bool Spawning;
    public Camera Camera;

    [SerializeField] private List<ScreenSide> _allowedSides = new();
    [SerializeField] private float _spawnIntervalMin = 2f;
    [SerializeField] private float _spawnIntervalMax = 2f;

    private float _timeToSpawnLeft = 0;

    private void Update()
    {
        if (!Spawning)
        {
            return;
        }

        _timeToSpawnLeft -= Time.deltaTime;
        if (_timeToSpawnLeft < 0f)
        {
            SpawnObject(GetRandomPositionOnAllowedSides());
            _timeToSpawnLeft = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        }
    }

    protected abstract void SpawnObject(Vector3 spawnPosition);

    private Vector3 GetRandomPositionOnAllowedSides()
    {
        float screenHalfWidth = Camera.orthographicSize * Camera.aspect;
        float screenHalfHeight = Camera.orthographicSize;

        ScreenSide side = _allowedSides[Random.Range(0, _allowedSides.Count)];

        Vector3 spawnPosition = Vector3.zero;

        switch (side.Side)
        {
            case SideEnum.Left:
                spawnPosition = new Vector3(-(screenHalfWidth + side.Offset), Random.Range(-screenHalfHeight, screenHalfHeight), 0);
                break;
            case SideEnum.Right:
                spawnPosition = new Vector3(screenHalfWidth + side.Offset, Random.Range(-screenHalfHeight, screenHalfHeight), 0);
                break;
            case SideEnum.Top:
                spawnPosition = new Vector3(Random.Range(-screenHalfWidth, screenHalfWidth), screenHalfHeight + side.Offset, 0);
                break;
            case SideEnum.Bottom:
                spawnPosition = new Vector3(Random.Range(-screenHalfWidth, screenHalfWidth), -(screenHalfHeight + side.Offset), 0);
                break;
        }
        return spawnPosition;
    }

    private void OnPlayerDied()
    {
        Spawning = false;
    }
}
