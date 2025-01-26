using System;
using UnityEngine;

public class PositionMover : MonoBehaviour
{
    public const float MinSpeed = 0f;

    [SerializeField] private float _maxSpeed = 64;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _endPosition;

    public bool IsMoving { get; private set; }

    public event Action ReachedPosition;

    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = Mathf.Clamp(value, MinSpeed, _maxSpeed);
        }
    }

    private void Update()
    {
        Vector3 endPosition = new Vector3(_endPosition.x, _endPosition.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, endPosition, Speed * Time.deltaTime);
        if (transform.position == endPosition && IsMoving)
        {
            IsMoving = false;
            ReachedPosition?.Invoke();
        }
    }

    public void MoveToPosition(Vector2 position)
    {
        _endPosition = position;
        IsMoving = true;
        Update();
    }
}
