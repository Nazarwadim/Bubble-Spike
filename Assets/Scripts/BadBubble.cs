using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadBubble : MonoBehaviour, IDamageable, IKillable, IDeathSound
{
    public enum Level { Simple = 10, Armored = 110, Hard = 210 }
    [SerializeField] private List<RuntimeAnimatorController> _animatorControllers = new() { };
    [SerializeField] private Level _level;
    [SerializeField] private Transform _parrent;
    private AudioSource _audioSource;
    public PositionMover Mover;
    public AudioClip deathSound;
    public AudioClip armorDestroySound;

    private int health;
    private int _damage;
    bool _destroyed = false;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        health = (int)_level;

        switch (_level)
        {
            case Level.Simple:
                _animator.runtimeAnimatorController = _animatorControllers[0];
                _damage = 10;
                break;
            case Level.Armored:
                _damage = 20;
                _animator.runtimeAnimatorController = _animatorControllers[1];
                break;
            case Level.Hard:
                _damage = 30;
                _animator.runtimeAnimatorController = _animatorControllers[2];
                break;
        }
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0 && !_destroyed)
        {
            _destroyed = true;
            ActionBus.BadBubbleDestroyed?.Invoke((int)_level / 10);
            if (!_audioSource.isPlaying)
            {
                _audioSource.volume = 1f;
                PlayDeathSound();
            }
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
            if (armorDestroySound)
            {
                _audioSource.volume = 0.2f;
                _audioSource.PlayOneShot(armorDestroySound);
            }
            StartCoroutine(TakedDamage());
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        if (deathSound)
        {
            yield return new WaitForSeconds(deathSound.length);
        }

        Destroy(_parrent.gameObject);
    }

    public int GetDamage()
    {
        return _damage;
    }

    private bool _isTakingDamage = false;
    private IEnumerator TakedDamage()
    {
        if (_isTakingDamage)
        {
            yield break;
        }
        _isTakingDamage = true;
        Color before = _spriteRenderer.color;
        _spriteRenderer.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = before;
        _isTakingDamage = false;
        switch (GetLevel())
        {
            case Level.Simple:
                _animator.runtimeAnimatorController = _animatorControllers[0];
                break;
            case Level.Armored:
                _animator.runtimeAnimatorController = _animatorControllers[1];
                break;
            case Level.Hard:
                _animator.runtimeAnimatorController = _animatorControllers[2];
                break;
        }
    }

    public Level GetLevel()
    {
        if (health <= (int)Level.Simple)
        {
            return Level.Simple;
        }
        else if (health <= (int)Level.Armored)
        {
            return Level.Armored;
        }
        else
        {
            return Level.Hard;
        }
    }
    public void PlayDeathSound()
    {
        if (_audioSource != null && deathSound != null)
        {
            _audioSource.PlayOneShot(deathSound);
        }
    }

}