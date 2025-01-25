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
        Health  // Бонус до HP
    }

    public BuffType buffType;      // Тип бафу
    public bool isDebuff;          // Чи це дебаф
    public float value;            // Значення бафу або дебафу
    private float fallSpeed = 2f;  // Швидкість падіння

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

        Destroy(gameObject); // Видаляємо баф після застосування
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
