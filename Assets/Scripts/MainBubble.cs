using System.Collections;
using UnityEngine;

public class MainBubble : MonoBehaviour, IDamageable
{
    [SerializeField] private ScoreProgress _healthProgress;
    private int _health = 100;
    [SerializeField] private float moveDistance = 1.5f;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private float idleTime = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private bool canMove = true;
    private const int MaxDamage = 10000000;
    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, 10000000);
        }
    }
    private bool _isDead = false;
    private bool _isMoving = false;
    private Coroutine moveCoroutine;

    private void Start()
    {
        _healthProgress.Change(Health);
        StartIdle();
    }

    private void Update()
    {
        if (!_isMoving && !_isDead && canMove)
        {
            moveCoroutine = StartCoroutine(MoveRandomly());
        }
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        _healthProgress.Change(Health);
        if (_health <= 0)
        {
            Destroy(gameObject);
            ActionBus.MainBubbleKilled?.Invoke();
            print("Died");
            _isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IKillable component))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                _isMoving = false;
                StartIdle();
            }

            TakeDamage(component.GetDamage());

            if (!_isDead && other.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(MaxDamage);
            }
        }
    }

    private void StartIdle()
    {
        animator.SetTrigger("GoIdle");
    }

    private IEnumerator MoveRandomly()
    {
        _isMoving = true;

        float direction = Random.value > 0.5f ? 1f : -1f;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0);

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);

        animator.SetTrigger("GoMove");

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        StartIdle();

        yield return new WaitForSeconds(idleTime);

        _isMoving = false;
    }
}