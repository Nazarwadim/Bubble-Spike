using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour, IBuffable, IDebuffable
{
    [SerializeField] private PositionMover _positionMover;
    [SerializeField] private MainBubble _mainBubble;    

    [SerializeField] private ScoreProgress _speedProgress;

    private float _speedModifier = 1f;
    public float SpeedModifier
    {
        get => _speedModifier;
        set
        {
            _speedModifier = Mathf.Clamp(value, 0.75f, 1.25f);
        }
    }

    private readonly Queue<Vector3> _positionsQueue = new();
    
    private void Start() 
    {
        _speedProgress.Change((int)_positionMover.Speed);
    }

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
            buff.ApplyBuff(this);
        }
        if (other.TryGetComponent(out IDamageable component))
        {
            component.TakeDamage(100);
        }
    }

    public void AddSpeedBuff(float speedModifier)
    {
        if (SpeedModifier < 1) SpeedModifier = 1;
        SpeedModifier += speedModifier;
        _positionMover.Speed *= SpeedModifier;
        _speedProgress.Change((int)_positionMover.Speed);
    }

    public void AddSpeedDebuff(float speedModifier)
    {
        if (SpeedModifier > 1) SpeedModifier = 1;
        SpeedModifier -= speedModifier;
        _positionMover.Speed *= SpeedModifier;
        _speedProgress.Change((int)_positionMover.Speed);
    }

    public void AddHealthBuff(float healthBonus)
    {
        _mainBubble.health += (int)healthBonus;
    }
}