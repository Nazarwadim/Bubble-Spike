using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonActions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent<PointerEventData> ButtonDown;
    public UnityEvent<PointerEventData> ButtonUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonUp?.Invoke(eventData);
    }
}