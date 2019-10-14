using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MobilePlayerController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image joystickBG;
    [SerializeField]
    private Image joystick;
    private Vector2 inputVector;

    private void Start()
    {
        joystickBG = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / joystickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBG.rectTransform.sizeDelta.x);

            inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            Debug.Log(inputVector);

            joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x * (joystickBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystickBG.rectTransform.sizeDelta.y / 2));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }
}
