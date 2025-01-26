using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BadBubble : MonoBehaviour, IDamageable, IKillable, IDeathSound
{
    public enum Level { Simple = 10, Armored = 150, Hard = 210 }
    [SerializeField] private List<AnimatorController> _animatorControllers = new() { };
    [SerializeField] private Level _level;
    [SerializeField] private Transform _parrent;
    private AudioSource _audioSource;
    public PositionMover Mover;
    public AudioClip deathSound;


    private int health;
    private int _damage;

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
        if (health <= 0)
        {
            ActionBus.BadBubbleDestroyed?.Invoke((int)_level / 10);
            Destroy(_parrent.gameObject);
            PlayDeathSound();
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
            StartCoroutine(TakedDamage());
        }
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);
        Destroy(_parrent.gameObject);
    }

    public int GetDamage()
    {
        return _damage;
    }

    private IEnumerator TakedDamage()
    {
        Color before = _spriteRenderer.color;
        _spriteRenderer.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = before;

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