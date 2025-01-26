using System.Collections;
using UnityEngine;
public interface IBuffable
{
    void AddSpeedBuff(float speedModifier);
    void AddHealthBuff(float healthBonus);
}

public interface IDebuffable
{
    void AddSpeedDebuff(float speedModifier);
}

public class Buff : MonoBehaviour, IDeathSound
{
    public enum BuffType
    {
        Speed,
        Health
    }
    private AudioSource _audioSource;
    public AudioClip deathSound;

    public BuffType buffType;
    public bool isDebuff;
    public float value;
    private float fallSpeed = 2f;
    private bool soundPlayed = false;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (IsOutOfBounds() && !soundPlayed)
        {
            Destroy(gameObject);
            soundPlayed = true;
        }
    }
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);
        Destroy(gameObject);
    }

    public void PlayDeathSound()
    {
        if (_audioSource != null && deathSound != null && !soundPlayed)
        {
            _audioSource.time = 0f;
            _audioSource.PlayOneShot(deathSound);
        }
    }

    public void ApplyBuff(Component target)
    {
        if (target == null) return;

        if (buffType == BuffType.Speed)
        {
            if (isDebuff && target.TryGetComponent<IDebuffable>(out var debuffable))
            {
                debuffable.AddSpeedDebuff(value);
            }
            else if (!isDebuff && target.TryGetComponent<IBuffable>(out var buffable))
            {
                buffable.AddSpeedBuff(value);
            }
        }
        else if (buffType == BuffType.Health)
        {
            if (!isDebuff && target.TryGetComponent<IBuffable>(out var buffable))
            {
                buffable.AddHealthBuff(value);
            }
        }

        if (!soundPlayed)
        {
            PlayDeathSound();
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(GetComponent<Collider2D>());
            StartCoroutine(DestroyAfterSound());
            soundPlayed = true;
        }
    }

    public void SetFallSpeed(float speed)
    {
        if (speed >= 0)
        {
            fallSpeed = speed;
        }
    }

    private bool IsOutOfBounds()
    {
        if (Camera.main == null) return true;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.y < -1.5f;
    }
}