using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PositionMover _positionMover;

    private readonly Queue<Vector3> _positionsQueue = new();

    private void Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled // TODO: Move to Game.cs Script.
        Application.targetFrameRate = 60;
#endif
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
        if (other.TryGetComponent(out IDamageable component))
        {
            component.TakeDamage(100);
        }
    }
}
