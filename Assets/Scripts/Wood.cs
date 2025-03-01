using System.Collections;
using UnityEngine;

public class Wood : MonoBehaviour, IDamageable, IKillable, IDeathSound
{
    public const int ScoreToGive = 40;

    [SerializeField] private ParticleSystem _particleSystemPrefab;
    [SerializeField] private int _damage;
    [SerializeField] private Vector2 _gravity;

    private ParticleSystem _particleDie;
    private GameObject _particleParrent;
    public Vector2 direction = Vector2.up;
    public float rotationSpeed = 50f;
    private AudioSource _audioSource;
    public AudioClip deathSound;


    public float Speed;

    private bool _destroyed = false;

    private Vector2 _gravityVelocity;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        Vector3 position = transform.position;
        Vector3 rotation = transform.localRotation.eulerAngles;

        position += (Vector3)(Time.deltaTime * (Speed * direction + _gravityVelocity));
        rotation.z += rotationSpeed * Time.deltaTime;

        transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        if (IsOutOfBounds() && !_destroyed)
        {
            _destroyed = true;
            Destroy(gameObject);
        }

        if (_destroyed && _particleParrent)
        {
            _particleParrent.transform.position = transform.position;
        }
        _gravityVelocity += _gravity * Time.deltaTime;
    }

    private bool IsOutOfBounds()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return Mathf.Abs(viewportPosition.y) > 1.5f || Mathf.Abs(viewportPosition.x) > 1.5f;
    }

    public void TakeDamage(int amount)
    {
        if (_destroyed)
        {
            return;
        }

        _destroyed = true;
        PlayDeathSound();
        StartCoroutine(DestroyAfterSound());

        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<Collider2D>());

        _particleParrent = new GameObject();
        _particleParrent.transform.position = transform.position;
        _particleParrent.transform.localScale = transform.localScale;
        _particleParrent.transform.rotation = transform.rotation;

        _particleDie = Instantiate(_particleSystemPrefab, _particleParrent.transform);
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);
        float duration = _particleDie.main.duration;
        Destroy(gameObject, duration);
        Destroy(_particleParrent.gameObject, duration);
        ActionBus.BadBubbleDestroyed?.Invoke(ScoreToGive);
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void PlayDeathSound()
    {
        if (_audioSource != null && deathSound != null)
        {
            _audioSource.time = 0f;
            _audioSource.PlayOneShot(deathSound);
        }
    }
}