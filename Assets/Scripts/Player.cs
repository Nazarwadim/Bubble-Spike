using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBuffable, IDebuffable
{
    [SerializeField] private PositionMover _positionMover;
    [SerializeField] private MainBubble _mainBubble;
    private float _speedModifier = 1f;
    public float SpeedModifier
    {
        get => _speedModifier;
        set
        {
            _speedModifier = Mathf.Clamp(value, 0.75f, 2.0f);
        }
    }

    private readonly Queue<Vector3> _positionsQueue = new();

    private void OnEnable()
    {
        _positionMover.ReachedPosition += OnReachedPosition;
    }

    private void OnDisable()
    {
        _positionMover.ReachedPosition -= OnReachedPosition;
    }

    public void OnScreenInputClicked(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 position = eventData.position;
        position = Camera.main.ScreenToWorldPoint(position);
        if (!_positionMover.IsMoving)
        {
            _positionMover.MoveToPosition(position);
            return;
        }
        _positionsQueue.Enqueue(position);
    }

    private void OnReachedPosition()
    {
        if (_positionsQueue.Count > 0)
        {
            _positionMover.MoveToPosition(_positionsQueue.Dequeue());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Buff buff))
        {
            buff.ApplyBuff(this); // Передаємо гравцю баф
        }
          if (other.TryGetComponent(out IDamageable component))
        {
            component.TakeDamage(100);
        }
    }

    public void AddSpeedBuff(float speedModifier)
    {
        SpeedModifier += speedModifier;
        _positionMover.Speed *= SpeedModifier; // Оновлюємо швидкість у PositionMover
        Debug.Log($"Speed Buff applied! New speedModifier: {SpeedModifier}");
    }

    public void AddSpeedDebuff(float speedModifier)
    {
        SpeedModifier -= speedModifier;
        _positionMover.Speed *= SpeedModifier; // Оновлюємо швидкість у PositionMover
        Debug.Log($"Speed Debuff applied! New speedModifier: {SpeedModifier}");
    }

   public void AddHealthBuff(float healthBonus)
    {
       _mainBubble.health += (int)healthBonus;
        Debug.Log($"Health Buff applied! New health: {  _mainBubble.health}");
    }
}