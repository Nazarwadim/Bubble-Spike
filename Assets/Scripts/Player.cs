using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PositionMover _positionMover;

    private readonly Queue<Vector3> _positionsQueue = new();

    private void Start()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
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
            Debug.Log("Is not moving");
            _positionMover.MoveToPosition(position);
            return;
        }
        Debug.Log("Is moving");
        _positionsQueue.Enqueue(position);
    }

    private void OnReachedPosition()
    {
        if (_positionsQueue.Count > 0)
        {
            _positionMover.MoveToPosition(_positionsQueue.Dequeue());
        }

    }
}
