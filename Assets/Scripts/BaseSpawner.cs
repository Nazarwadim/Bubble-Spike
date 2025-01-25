using UnityEngine;
using System.Collections.Generic;

public abstract class BaseSpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    [SerializeField]
    protected List<ScreenSide> allowedSides = new List<ScreenSide> { ScreenSide.Left, ScreenSide.Right, ScreenSide.Top, ScreenSide.Bottom };
    protected Camera mainCamera;

    protected enum ScreenSide { Left, Right, Top, Bottom }

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }
    protected abstract void SpawnObject();

    protected Vector3 GetRandomPositionOnAllowedSides()
    {
        float screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float screenHalfHeight = mainCamera.orthographicSize;

        ScreenSide side = allowedSides[Random.Range(0, allowedSides.Count)];

        Vector3 spawnPosition = Vector3.zero;

        switch (side)
        {
            case ScreenSide.Left:
                spawnPosition = new Vector3(-screenHalfWidth, Random.Range(-screenHalfHeight, screenHalfHeight), 0);
                break;
            case ScreenSide.Right:
                spawnPosition = new Vector3(screenHalfWidth, Random.Range(-screenHalfHeight, screenHalfHeight), 0);
                break;
            case ScreenSide.Top:
                spawnPosition = new Vector3(Random.Range(-screenHalfWidth, screenHalfWidth), screenHalfHeight, 0);
                break;
            case ScreenSide.Bottom:
                spawnPosition = new Vector3(Random.Range(-screenHalfWidth, screenHalfWidth), -screenHalfHeight, 0);
                break;
        }

        return spawnPosition;
    }
}