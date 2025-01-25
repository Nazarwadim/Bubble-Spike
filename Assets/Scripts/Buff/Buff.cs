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

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        Speed,
        Health
    }

    public BuffType buffType;
    public bool isDebuff;
    public float value;
    private float fallSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (IsOutOfBounds())
        {
            Destroy(gameObject);
        }
    }

    public void ApplyBuff(Component target)
    {
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

        Destroy(gameObject);
    }

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }

    private bool IsOutOfBounds()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.y < -1.5f;
    }
}
