using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image buttonImage; // The UI Image component
    public Sprite idleSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    private void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        buttonImage.sprite = idleSprite; // Default to idle sprite
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = idleSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonImage.sprite = clickSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
    }
}
