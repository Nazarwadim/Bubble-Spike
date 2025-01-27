using System.Collections;
using UnityEngine;

public class MainBubble : MonoBehaviour, IDamageable
{
    const int MaxMoveIntoDirection = 3;

    [SerializeField] private Transform _centerPosition;
    [SerializeField] private ScoreProgress _healthProgress;
    [SerializeField] private Color _takeDamageColor;
    [SerializeField] private int _health = 100;
    [SerializeField] private int _maxHealth = 200;
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
            _health = Mathf.Clamp(value, 0, _maxHealth);
            _healthProgress.Change(_health);
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
            _isDead = true;
        }
        else
        {
            StartCoroutine(TakedDamage());
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

        float distanceFromCenterIfGoRight = Mathf.Abs(transform.position.x - _centerPosition.position.x + 1f);
        float distanceFromCenterIfGoLeft = Mathf.Abs(transform.position.x - _centerPosition.position.x - 1f);

        float direction;
        if (distanceFromCenterIfGoRight < distanceFromCenterIfGoLeft)
        {
            direction = Random.value > 0.4f ? 1f : -1f;
        }
        else
        {
            direction = Random.value > 0.6f ? 1f : -1f;
        }

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

    private bool _isTakingDamage = false;
    private IEnumerator TakedDamage()
    {
        if (_isTakingDamage)
        {
            yield break;
        }
        _isTakingDamage = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color before = spriteRenderer.color;
        spriteRenderer.color = _takeDamageColor;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = before;
        _isTakingDamage = false;
    }
}