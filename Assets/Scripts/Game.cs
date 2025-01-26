using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool IsPlaying { get; private set; }

    [SerializeField] private WoodSpawner _woodSpawner;
    [SerializeField] private BuffSpawner _buffSpawner;
    [SerializeField] private BubbleSpawner _badBubbleSpawner;

    [SerializeField] private Score _score;

    private int _woodsToSpawnLeft = 0;
    private int _buffToSpawnLeft = 0;
    private int _bubblesToSpawnLeft = 0;

    private void OnEnable()
    {
        ActionBus.MainBubbleKilled += StopPlaying;
        _woodSpawner.ObjectSpawned += WoodSpawned;
        _badBubbleSpawner.ObjectSpawned += BubbleSpawned;
        _buffSpawner.ObjectSpawned += BuffSpawned;

        ActionBus.BadBubbleDestroyed += AddScore;
        ActionBus.WoodDestroyed += AddScore;
    }

    private void OnDisable()
    {
        ActionBus.MainBubbleKilled -= StopPlaying;
        _woodSpawner.ObjectSpawned -= WoodSpawned;
        _badBubbleSpawner.ObjectSpawned -= BubbleSpawned;
        _buffSpawner.ObjectSpawned -= BuffSpawned;

        ActionBus.BadBubbleDestroyed -= AddScore;
        ActionBus.WoodDestroyed -= AddScore;
    }

    private void AddScore(int amount)
    {
        _score.AddScore(amount);
    }

    private void StartPlaying()
    {
        IsPlaying = true;
    }

    private void StopPlaying()
    {
        IsPlaying = false;
        DisableSpawning();
    }

    private void BubbleSpawned()
    {
        _bubblesToSpawnLeft--;
    }

    private void BuffSpawned()
    {
        _buffToSpawnLeft--;
    }

    private void WoodSpawned()
    {
        _woodsToSpawnLeft--;
    }

    private void Awake()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled // TODO: Move to Game.cs Script.
        Application.targetFrameRate = 60;
#endif
    }

    private void Start()
    {
        DisableSpawning();
        StartPlaying();
        StartCoroutine(SetTrainingWawe());
    }

    private void DisableSpawning()
    {
        _woodSpawner.Spawning = false;
        _buffSpawner.Spawning = false;
        _badBubbleSpawner.Spawning = false;
    }

    private void EnableAllSpawning()
    {
        _woodSpawner.Spawning = true;
        _buffSpawner.Spawning = true;
        _badBubbleSpawner.Spawning = true;
    }

    private IEnumerator SetTrainingWawe()
    {
        _badBubbleSpawner.Spawning = true;

        _bubblesToSpawnLeft = 5;
        _badBubbleSpawner.SimpleProbability = 100;

        var cond = new WaitUntil(() =>
        {
            return _bubblesToSpawnLeft <= 0 || !IsPlaying;
        });
        yield return cond;

        DisableSpawning();


        if (!IsPlaying)
        {
            yield break;
        }

        yield return new WaitForSeconds(7f);

        _woodSpawner.Spawning = true;
        _woodsToSpawnLeft = 3;

        float spawnInterwalMinBefore = _woodSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBefore = _woodSpawner.SpawnIntervalMax;

        _woodSpawner.MinArrowTime = 2;
        _woodSpawner.MaxArrowTime = 3;

        _woodSpawner.SpawnIntervalMin = _woodSpawner.SpawnIntervalMax = 3;

        cond = new WaitUntil(() =>
        {
            return _woodsToSpawnLeft <= 0 || !IsPlaying;
        });
        yield return cond;

        _woodSpawner.SpawnIntervalMin = spawnInterwalMinBefore;
        _woodSpawner.SpawnIntervalMax = spawnInterwalMaxBefore;

        DisableSpawning();

        if (!IsPlaying)
        {
            yield break;
        }

        yield return new WaitForSeconds(7f);

        _buffSpawner.Spawning = true;
        _buffToSpawnLeft = 5;

        spawnInterwalMinBefore = _buffSpawner.SpawnIntervalMin;
        spawnInterwalMaxBefore = _buffSpawner.SpawnIntervalMax;

        _buffSpawner.SpawnIntervalMin = _buffSpawner.SpawnIntervalMax = 5;

        cond = new WaitUntil(() =>
        {
            return _buffToSpawnLeft <= 0 || !IsPlaying;
        });
        yield return cond;
        DisableSpawning();

        _buffSpawner.SpawnIntervalMin = spawnInterwalMinBefore;
        _buffSpawner.SpawnIntervalMax = spawnInterwalMaxBefore;

        if (!IsPlaying)
        {
            yield break;
        }

        yield return new WaitForSeconds(10f);

        StartCoroutine(SetFirstWawe());
    }

    private IEnumerator SetFirstWawe()
    {
        _badBubbleSpawner.Spawning = true;
        _badBubbleSpawner.SimpleProbability = 95;
        _woodSpawner.Spawning = true;

        _bubblesToSpawnLeft = Random.Range(100, 300);
        _woodsToSpawnLeft = 10;

        _woodSpawner.MinArrowTime = 2;
        _woodSpawner.MaxArrowTime = 3;

        float spawnInterwalMinBeforeBub = _badBubbleSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeBub = _badBubbleSpawner.SpawnIntervalMax;

        float spawnInterwalMinBeforeWood = _woodSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeWood = _woodSpawner.SpawnIntervalMax;

        _badBubbleSpawner.SpawnIntervalMin = 0.05f;
        _badBubbleSpawner.SpawnIntervalMax = 0.1f;

        _badBubbleSpawner.MinSpeed = 0.05f;
        _badBubbleSpawner.MaxSpeed = 0.2f;

        _woodSpawner.SpawnIntervalMin = 5;
        _woodSpawner.SpawnIntervalMax = 10;

        var cond = new WaitUntil(() =>
        {
            return (_bubblesToSpawnLeft <= 0 && _woodsToSpawnLeft <= 0) || !IsPlaying;
        });

        yield return cond;
        DisableSpawning();

        _woodSpawner.SpawnIntervalMin = spawnInterwalMinBeforeWood;
        _woodSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeWood;

        _badBubbleSpawner.SpawnIntervalMin = spawnInterwalMinBeforeBub;
        _badBubbleSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeBub;

        _badBubbleSpawner.SimpleProbability = 75;

        _badBubbleSpawner.MinSpeed = 0.1f;
        _badBubbleSpawner.MaxSpeed = 0.5f;

        yield return new WaitForSeconds(15f);

        if (IsPlaying) StartCoroutine(SetSecondWawe());
    }

    private IEnumerator SetSecondWawe()
    {

        EnableAllSpawning();

        _woodSpawner.MinArrowTime = 1f;
        _woodSpawner.MaxArrowTime = 1.5f;

        _bubblesToSpawnLeft = Random.Range(10, 40);
        _woodsToSpawnLeft = Random.Range(10, 20);

        float spawnInterwalMinBeforeBub = _badBubbleSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeBub = _badBubbleSpawner.SpawnIntervalMax;

        float spawnInterwalMinBeforeWood = _woodSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeWood = _woodSpawner.SpawnIntervalMax;

        _badBubbleSpawner.SpawnIntervalMin = 0.5f;
        _badBubbleSpawner.SpawnIntervalMax = 1f;
        _woodSpawner.SpawnIntervalMin = 2f;
        _woodSpawner.SpawnIntervalMax = 3f;

        var cond = new WaitUntil(() =>
        {
            return (_bubblesToSpawnLeft <= 0 && _woodsToSpawnLeft <= 0) || !IsPlaying;
        });

        yield return cond;
        DisableSpawning();

        _woodSpawner.SpawnIntervalMin = spawnInterwalMinBeforeWood;
        _woodSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeWood;

        _badBubbleSpawner.SpawnIntervalMin = spawnInterwalMinBeforeBub;
        _badBubbleSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeBub;

        yield return new WaitForSeconds(10f);

        if (!IsPlaying)
        {
            yield break;
        }
        float randW = Random.Range(0, 10);
        if (randW < 5)
        {
            StartCoroutine(SetThirdWawe());
        }
        else if (randW < 8)
        {
            StartCoroutine(SetFirstWawe());
        }
        else
        {
            StartCoroutine(SetSecondWawe());
        }

    }

    private IEnumerator SetThirdWawe()
    {

        EnableAllSpawning();

        _badBubbleSpawner.SpawnIntervalMin = 0;
        _badBubbleSpawner.SpawnIntervalMax = 0.5f;

        yield return new WaitForSeconds(Random.Range(20, 100));
        DisableSpawning();

        yield return new WaitForSeconds(Random.Range(5, 10));

        float randW = Random.Range(0, 100);

        if (!IsPlaying)
        {
            yield break;
        }

        if (randW < 10)
        {
            StartCoroutine(SetFirstWawe());
        }
        else if (randW < 80)
        {
            StartCoroutine(SetSecondWawe());
        }
        else
        {
            StartCoroutine(SetThirdWawe());
        }
    }
}
