using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool IsPlaying { get; private set; }

    [SerializeField] private WoodSpawner _woodSpawner;
    [SerializeField] private BuffSpawner _buffSpawner;
    [SerializeField] private BubbleSpawner _badBubbleSpawner;
    [SerializeField] private Animator _transition;

    [SerializeField] private Score _score;

    public AudioSource audioSource;

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
        StartCoroutine(SetTrainingWawe());
    }

    private void StopPlaying()
    {
        IsPlaying = false;
        DisableSpawning();

        _transition.SetTrigger("Died");
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
        audioSource.volume = MusicVolume.Volume;
        DisableSpawning();
        StartPlaying();
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

        yield return new WaitForSeconds(5f);

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

        yield return new WaitForSeconds(8f);

        StartCoroutine(SetFirstWawe());
    }

    private IEnumerator SetFirstWawe()
    {
        _badBubbleSpawner.Spawning = true;
        _badBubbleSpawner.SimpleProbability = 95;
        _woodSpawner.Spawning = true;
        _buffSpawner.Spawning = true;

        _bubblesToSpawnLeft = Random.Range(120, 200);
        _woodsToSpawnLeft = 10;

        _woodSpawner.MinArrowTime = 2;
        _woodSpawner.MaxArrowTime = 3;

        float spawnInterwalMinBeforeBub = _badBubbleSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeBub = _badBubbleSpawner.SpawnIntervalMax;

        float spawnInterwalMinBeforeBuff = _buffSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeBuff = _buffSpawner.SpawnIntervalMax;

        float spawnInterwalMinBeforeWood = _woodSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeWood = _woodSpawner.SpawnIntervalMax;

        _badBubbleSpawner.SpawnIntervalMin = 0.05f;
        _badBubbleSpawner.SpawnIntervalMax = 0.12f;

        _buffSpawner.SpawnIntervalMin = 5f;
        _buffSpawner.SpawnIntervalMax = 15f;

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

        _buffSpawner.SpawnIntervalMin = spawnInterwalMinBeforeBuff;
        _buffSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeBuff;

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

        _bubblesToSpawnLeft = Random.Range(10, 20);
        _woodsToSpawnLeft = Random.Range(8, 15);

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

        _badBubbleSpawner.SpawnIntervalMin = 0.1f;
        _badBubbleSpawner.SpawnIntervalMax = 0.5f;

        yield return new WaitForSeconds(Random.Range(20, 60));
        DisableSpawning();

        yield return new WaitForSeconds(Random.Range(7, 10));

        float randW = Random.Range(0, 100);

        if (!IsPlaying)
        {
            yield break;
        }

        if (randW < 10)
        {
            StartCoroutine(SetFirstWawe());
        }
        else if (randW < 30)
        {
            StartCoroutine(SetSecondWawe());
        }
        else if (randW < 80)
        {
            yield return new WaitForSeconds(Random.Range(5, 6));
            StartCoroutine(SetForthWawe());
        }
        else
        {
            StartCoroutine(SetThirdWawe());
        }
    }

    private IEnumerator SetForthWawe()
    {
        _woodSpawner.Spawning = true;

        _woodSpawner.MinArrowTime = 2f;
        _woodSpawner.MaxArrowTime = 2f;

        float spawnInterwalMinBeforeWood = _woodSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeWood = _woodSpawner.SpawnIntervalMax;

        float spawnInterwalMinBeforeBuff = _buffSpawner.SpawnIntervalMin;
        float spawnInterwalMaxBeforeBuff = _buffSpawner.SpawnIntervalMax;

        _badBubbleSpawner.SpawnIntervalMin = 0;

        _woodSpawner.SpawnIntervalMin = 0f;
        if (_score.Count < 10000)
        {
            _woodSpawner.SpawnIntervalMax = 2f;
            _badBubbleSpawner.MaxSpeed = 0.8f;
            _badBubbleSpawner.SpawnIntervalMax = 0.3f;
        }
        else
        {
            _woodSpawner.SpawnIntervalMax = 1.3f;
            _badBubbleSpawner.MaxSpeed = 1.2f;
            _badBubbleSpawner.SpawnIntervalMax = 0.2f;
        }

        _buffSpawner.SpawnIntervalMin = 2f;
        _buffSpawner.SpawnIntervalMax = 7f;

        _badBubbleSpawner.MinSpeed = 0.2f;
        _badBubbleSpawner.MaxSpeed = 1.1f;

        yield return new WaitForSeconds(Random.Range(10, 16));

        if (IsPlaying)
        {
            EnableAllSpawning();
        }


        _woodSpawner.SpawnIntervalMin = 1f;
        _woodSpawner.SpawnIntervalMax = 3f;

        _woodSpawner.MinArrowTime = 1f;
        _woodSpawner.MaxArrowTime = 1.5f;

        yield return new WaitForSeconds(Random.Range(25, 50));
        DisableSpawning();

        _badBubbleSpawner.MinSpeed = 0.1f;
        _badBubbleSpawner.MaxSpeed = 0.5f;

        _badBubbleSpawner.SpawnIntervalMin = 0;
        _badBubbleSpawner.SpawnIntervalMax = 0.5f;

        _woodSpawner.SpawnIntervalMin = spawnInterwalMinBeforeWood;
        _woodSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeWood;

        _buffSpawner.SpawnIntervalMin = spawnInterwalMinBeforeBuff;
        _buffSpawner.SpawnIntervalMax = spawnInterwalMaxBeforeBuff;

        if (!IsPlaying)
        {
            yield break;
        }

        yield return new WaitForSeconds(Random.Range(7, 10));

        float randW = Random.Range(0, 100);

        if (randW < 20)
        {
            StartCoroutine(SetFirstWawe());
        }
        else
        {
            StartCoroutine(SetSecondWawe());
        }
    }
}
