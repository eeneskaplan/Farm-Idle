using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoystickControl : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    [SerializeField] private GameObject joystickObje;
    [SerializeField] private RectTransform joystickDis;
    [SerializeField] private RectTransform joystickIc;
    [SerializeField] private float maksCap = 100f;
                     public Vector2 hareketYonu;
    void Start()
    {

        joystickObje.SetActive(false);

    }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickObje.SetActive(true);
        joystickDis.position = eventData.position;
        joystickIc.anchoredPosition = Vector2.zero; 
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 hedefVector = eventData.position - (Vector2)joystickDis.position;
        hedefVector= Vector2.ClampMagnitude(hedefVector, maksCap);
        joystickIc.anchoredPosition = hedefVector;
        hareketYonu = hedefVector / maksCap;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        joystickObje.SetActive(false);
        joystickIc.anchoredPosition = Vector2.zero;
        hareketYonu = Vector2.zero;
    }
}
